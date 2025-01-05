using Exchange.Api.Models;
using Exchange.Api.Models.History;

namespace Exchange.Api.Services
{
    public interface IHistoryService
    {
        List<BalanceRecord> GetFIFOMatchedBalance(string instrument, decimal availableBalance);
        GetOrderHistoryResponse GetOrderHistory(string instrument, DateTime? from = null, int limit = 50, int requestId = 1);
    }
}