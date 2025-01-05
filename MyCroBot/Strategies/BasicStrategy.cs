using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchange.Api.Services;

namespace MyCroBot.Strategies
{
    /// <summary>
    /// Input:
    /// - instrument
    /// - start price
    /// - take profit percents
    /// - stop loss percents
    /// - stop loss gamble percents
    /// - buy step size
    /// 
    /// Algo:
    /// - buys 30% instantly
    /// - in case there is movement UP >= buy step size, buys another 30%
    /// - if profit target is reached, waits until price stops moving up, sells and waits for drop or another increase >= buy step size
    /// - if price falls down, waits for the bottom line, until at least 3/4 of buy step size UP trend is available, then places order
    /// - if price is <= stop loss percents, tries to sell a bit above last buy order (stop loss gamble percents)
    /// </summary>
    internal class BasicStrategy : StrategyBase
    {
        private const decimal _priceTolerance = 1.05m;
        private decimal _stopLossGamblePercents;
        private decimal _buyStepSize;
        private readonly IUserService _userService;
        private readonly IMarketService _marketService;
        private readonly ITradingService _tradingService;
        private readonly IHistoryService _historyService;

        public BasicStrategy(IUserService userService, IMarketService marketService, ITradingService tradingSevice, IHistoryService historyService) : base(new BasicSettings())
        {
            _userService = userService;
            _marketService = marketService;
            _tradingService = tradingSevice;
            _historyService = historyService;
        }

        public void Setup(BasicSettings basicSettings, decimal stopLossGamblePercents, decimal buyStepSize)
        {
            _basicSettings = basicSettings;
            _stopLossGamblePercents = stopLossGamblePercents;
            _buyStepSize = buyStepSize;
        }

        public void UpdatePrice(string price)
        {
            _basicSettings.StartPrice = Decimal.Parse(price, CultureInfo.InvariantCulture);
            Console.WriteLine($"Adjusted price to: {_basicSettings.StartPrice.ToString(CultureInfo.InvariantCulture)}");
        }

        public bool Prepare()
        {
            Console.WriteLine("--- PREPARE STRATEGY: BASIC ---");
            Console.WriteLine($"Instrument: {_basicSettings.Instrument}");
            Console.WriteLine($"Start price: {_basicSettings.StartPrice}");
            Console.WriteLine($"Take profit percents: {_basicSettings.TakeProfitPercents}");
            Console.WriteLine($"Stop loss percents: {_basicSettings.StopLossPercents}");
            Console.WriteLine($"Stop loss GAMBLE percents: {_stopLossGamblePercents}");
            Console.WriteLine($"Buy step size: {_buyStepSize}");
            Console.WriteLine($"Investment amount: {_basicSettings.InvestmentAmount}");
            Console.WriteLine("--- CURRENT TRADED PRICE FOR INSTRUMENT ---");

            return Confirm("ARE YOU OK WITH THESE SETTINGS? y/n");
        }

        public void ExecuteStep()
        {
            var balance = _userService.GetUserPositionBalance();
            var instrumentPrice = _marketService.GetLowestSellPrice(_basicSettings.Instrument);
            if (balance > _basicSettings.InvestmentAmount)
            {
                // todo: replace console with more general concept.
                Console.WriteLine("You have enough balance on account to create more buy orders.");
                Console.WriteLine($"Desired entry price: {_basicSettings.StartPrice}");
                Console.WriteLine($"Current price: {instrumentPrice}");
                if (instrumentPrice <= _basicSettings.StartPrice * _priceTolerance)
                {
                    var amount = GetTradeAmountFromPrice(instrumentPrice, _basicSettings.InvestmentAmount, 0); // todo: pass instrument dec. places
                    var shouldBuy = Confirm($"DO YOU WANT TO PLACE ORDER AT {instrumentPrice} for USD {instrumentPrice * amount} / instrument: {amount}? y/n");
                    if (shouldBuy)
                    {
                        var result = _tradingService.CreateOrder(_basicSettings.Instrument, instrumentPrice, amount, "BUY", "LIMIT");
                        Console.WriteLine($"PLACED ORDER {result.Result.order_id}");
                    }
                    else
                    {
                        Console.WriteLine("NO, PASSING THIS TRADE");
                    }
                }
                else
                {
                    Console.WriteLine("Buy criteria were not met.");
                }
            }
            else
            {
                Console.WriteLine("Low position balance for insrument pair");
            }

            var candlestickTrend = _marketService.GetCandlestickTrend(_basicSettings.Instrument, 150, "1m");
            //foreach (var candlestick in candlestickTrend)
            //{
            //    Console.WriteLine($"Trend: {candlestick.ChangePerc}");
            //}

            var avgChange = candlestickTrend.Average(p => p.ChangePerc);
            var avg5 = candlestickTrend.TakeLast(5).Average(p => p.ChangePerc);
            var avg10 = candlestickTrend.TakeLast(10).Average(p => p.ChangePerc);
            var avg25 = candlestickTrend.TakeLast(25).Average(p => p.ChangePerc);
            var avg50 = candlestickTrend.TakeLast(50).Average(p => p.ChangePerc);
            Console.WriteLine($"AVG: {avgChange}");
            Console.WriteLine($"AVG5: {avg5}");
            Console.WriteLine($"AVG10: {avg10}");
            Console.WriteLine($"AVG25: {avg25}");
            Console.WriteLine($"AVG50: {avg50}");

            var sellSignal = false;
            var diffFromMaxPrice = NumberHelper.GwtPercentDifference(candlestickTrend.Max(p => p.Price), candlestickTrend.Last().Price);

            // averages going down more than gamble percents 
            if (avg5 < 0 && diffFromMaxPrice <= -_stopLossGamblePercents)
            {
                sellSignal = true;
                Console.WriteLine("MaxPrice sell signal");
            }

            // averages going down
            if (avg5 < avg10 && avg25 < avg50)
            {
                sellSignal |= true;
                Console.WriteLine("AVGs sell signal");
            }

            // todo: split to separate settings?
            var instrumentBalance = _userService.GetUserPositionBalance("XLM");
            Console.WriteLine($"Instrument balance: {instrumentBalance}");
            var balanceRecords = _historyService.GetFIFOMatchedBalance(_basicSettings.Instrument, instrumentBalance);

            foreach (var balanceRecord in balanceRecords)
            {
                var percentIncrease = NumberHelper.GwtPercentDifference(balanceRecord.buyPrice, instrumentPrice);
                Console.WriteLine($"Balance: {balanceRecord.balance} {balanceRecord.balance} for {balanceRecord.buyPrice}");
                Console.WriteLine($"Price increase {percentIncrease} %");

                // todo..
                if (balanceRecord.balance < 1)
                {
                    Console.WriteLine("Under tradeable balance");
                    continue;
                }

                if (sellSignal && percentIncrease >= _basicSettings.TakeProfitPercents)
                {
                    var sell = Confirm("Do you want to sell at this increase?");
                    if (sell)
                    {
                        // todo.. this is trick to get correct instrument units.
                        var result = _tradingService.CreateOrder(_basicSettings.Instrument, instrumentPrice, Math.Floor(balanceRecord.balance), "SELL", "LIMIT");
                        Console.WriteLine($"PLACED ORDER {result.Result.order_id}");
                    }
                    else
                    {
                        Console.WriteLine("NO, PASSING THIS TRADE");
                    }
                }
            }

            var buySignal = false;
            // averages going up more than gamble percents 
            if (avg5 > 0 && diffFromMaxPrice >= _stopLossGamblePercents || candlestickTrend.TakeLast(5).Sum(p => p.ChangePerc) >= _stopLossGamblePercents)
            {
                buySignal = true;
                Console.WriteLine("MaxPrice buy signal");
            }

            // averages going up
            if (avg5 > avg10)
            {
                buySignal |= true;
                Console.WriteLine("AVGs buy signal");
            }

            if (buySignal && balance > _basicSettings.InvestmentAmount)
            {
                var buyAmount = GetTradeAmountFromPrice(instrumentPrice, _basicSettings.InvestmentAmount, 0); // todo: pass instrument dec. places
                var buy = Confirm($"DO YOU WANT TO PLACE ORDER AT {instrumentPrice} for USD {instrumentPrice * buyAmount} / instrument: {buyAmount}? y/n");
                if (buy)
                {
                    var result = _tradingService.CreateOrder(_basicSettings.Instrument, instrumentPrice, buyAmount, "BUY", "LIMIT");
                    Console.WriteLine($"PLACED ORDER {result.Result.order_id}");
                }
            }
        }
    }
}
