using Exchange.Api.Models.Market;

namespace Exchange.Api.Services
{
    public interface IMarketService
    {
        GetBookResponse GetBook(string instrument, int requestId = 1);
        List<CandlestickRecord> GetCandlestickTrend(string instrument, int count, string timeframe = "1m", int requestId = 1);
        decimal GetHighestBuyPrice(string instrument, int requestId = 1);
        decimal GetLowestSellPrice(string instrument, int requestId = 1);
    }
}