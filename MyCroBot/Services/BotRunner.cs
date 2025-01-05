using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exchange.Api.Services;
using MyCroBot.Strategies;

namespace MyCroBot.Services
{
    internal class BotRunner
    {
        private readonly IUserService _userService;
        private readonly BasicStrategy _basicStrategy;
        private readonly IMarketService _marketService;
        private int requestId = 1;
        public BotRunner(IUserService userService, IMarketService marketService, BasicStrategy basicStrategy)
        {
            _userService = userService;
            _basicStrategy = basicStrategy; // move to factory or smth.
            _marketService = marketService;
        }

        public void Run()
        {
            Console.WriteLine("--- Checking balance ---");
            var balanceResult = _userService.GetUserBalance(requestId++);
            var balance = balanceResult.Result.data.First();
            Console.WriteLine($"Total available balance: {balance.total_available_balance}");
            Console.WriteLine($"Total cash balance: {balance.total_cash_balance}");
            Console.WriteLine($"Instrument: {balance.instrument_name}");
            Console.WriteLine("--- Picking strategy ---");

            _basicStrategy.Setup(new BasicSettings()
            {
                Instrument = "XLM_USD",
                StartPrice = 0.35m,
                TakeProfitPercents = 5,
                StopLossPercents = 5,
                InvestmentAmount = 5,
            }, 0.5m, 0.5m);

            if (_basicStrategy.Prepare())
            {
                Console.WriteLine("--- ACCEPTED ---");

                var book = _marketService.GetBook(_basicStrategy.GetBasicSettings().Instrument);
                Console.WriteLine($"Lowest ask: {book.Result.data.First().asks.OrderBy(p => p[0]).First()[0]}");
                Console.WriteLine($"Highest bid: {book.Result.data.First().bids.OrderByDescending(p => p[0]).First()[0]}");

                Console.WriteLine("Customize entry price? y/n");
                var customize = Console.ReadLine();
                if (customize == "y")
                {
                    Console.WriteLine("Enter new entry price");
                    var price = Console.ReadLine();
                    _basicStrategy.UpdatePrice(price);
                }

                Console.WriteLine("--- RUNNING ---");

                while (true)
                {
                    Thread.Sleep(5000);
                    _basicStrategy.ExecuteStep();
                }
            } else
            {
                Console.WriteLine("TERMINATED");
            }
        }
    }
}
