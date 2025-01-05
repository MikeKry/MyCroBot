using System.Globalization;
using Exchange.Api.Models.Market;

namespace Exchange.Api.Services
{
    public class MarketService : IMarketService
    {
        private readonly IRequestBuilder _requestBuilder;
        public MarketService(IRequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
        }

        public GetBookResponse GetBook(string instrument, int requestId = 1)
        {
            var requestBody = new GetBookGetRequest()
            {
                Instrument = instrument,
            };

            var result = _requestBuilder.SendGetRequest<GetBookGetRequest, GetBookResponse, BookResponse>(requestBody);
            return result;
        }

        public GetCandlestickResponse GetCandlesticks(string instrument, int count, string timeframe = "1m", int requestId = 1)
        {
            var requestBody = new GetCandlestickGetRequest()
            {
                Instrument = instrument,
                Count = count,
                TimeFrame = timeframe,
            };

            var result = _requestBuilder.SendGetRequest<GetCandlestickGetRequest, GetCandlestickResponse, CandlestickResponse>(requestBody);
            return result;
        }

        public decimal GetLowestSellPrice(string instrument, int requestId = 1)
        {
            var apiResult = GetBook(instrument, requestId);
            return decimal.TryParse(apiResult.Result.data.First().asks.OrderBy(p => p[0]).First()[0], CultureInfo.InvariantCulture, out decimal ask) ? ask : 0m;
        }

        public decimal GetHighestBuyPrice(string instrument, int requestId = 1)
        {
            var apiResult = GetBook(instrument, requestId);
            return decimal.TryParse(apiResult.Result.data.First().bids.OrderByDescending(p => p[0]).First()[0], CultureInfo.InvariantCulture, out decimal bid) ? bid : 0m;
        }

        public List<CandlestickRecord> GetCandlestickTrend(string instrument, int count, string timeframe = "1m", int requestId = 1)
        {
            var apiResult = GetCandlesticks(instrument, count, timeframe, requestId);
            var sticks = apiResult.Result.data;
            var result = new List<CandlestickRecord>();

            for (int i = 0; i < sticks.Length; i++)
            {
                if (i == 0)
                {
                    result.Add(new CandlestickRecord()
                    {
                        ChangePerc = 0,
                        IsDown = false,
                        IsUp = false,
                        Price = NumberHelper.ParseDecimal(sticks[i].ClosePrice),
                    });
                }
                else
                {
                    var prevClosePrice = NumberHelper.ParseDecimal(sticks[i - 1].ClosePrice);
                    var closePrice = NumberHelper.ParseDecimal(sticks[i].ClosePrice);
                    result.Add(new CandlestickRecord()
                    {
                        ChangePerc = NumberHelper.GwtPercentDifference(prevClosePrice, closePrice),
                        IsDown = false,
                        IsUp = false,
                        Price = closePrice,
                    });
                }
            }

            return result;
        }
    }
}
