using Exchange.Api.Models;
using Exchange.Api.Models.History;

namespace Exchange.Api.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IRequestBuilder _requestBuilder;
        public HistoryService(IRequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
        }

        public GetOrderHistoryResponse GetOrderHistory(string instrument, DateTime? from = null, int limit = 50, int requestId = 1)
        {
            var requestBody = new GetOrderHistoryRequest()
            {
                GetOrderHistoryParameters = new GetOrderHistoryParameters()
                {
                    instrument_name = instrument,
                    start_time = from.HasValue ? ((DateTimeOffset)from.Value.ToUniversalTime()).ToUnixTimeMilliseconds().ToString() : string.Empty,
                    end_time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                    ////limit = limit.ToString(),
                },
            };

            var result = _requestBuilder.SendPostRequest<GetOrderHistoryRequest, GetOrderHistoryResponse, OrderHistoryResult>(requestBody);
            return result;
        }

        /// <summary>
        /// todo: this method cannot distinguish when trading with multiple pairs, for example XLM-USD, XLM-USDT
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="availableBalance"></param>
        /// <returns></returns>
        public List<BalanceRecord> GetFIFOMatchedBalance(string instrument, decimal availableBalance)
        {
            var latestHistory = GetAllOrdersByStatus(instrument, "FILLED", "BUY").OrderByDescending(p => p.update_time);
            var balanceSheet = new List<BalanceRecord>();

            foreach (var order in latestHistory)
            {
                if (availableBalance > 0)
                {
                    // todo: handle fee - can be from other instrument!
                    decimal quantity = NumberHelper.ParseDecimal(order.quantity) - NumberHelper.ParseDecimal(order.cumulative_fee);
                    if (quantity > availableBalance)
                    {
                        quantity = availableBalance;
                    }

                    balanceSheet.Add(new BalanceRecord()
                    {
                        balance = quantity,
                        buyPrice = NumberHelper.ParseDecimal(order.avg_price),
                        instrument = order.instrument_name,
                    });

                    availableBalance -= quantity;
                }
                else
                {
                    break;
                }
            }

            if (availableBalance > 0)
            {
                Console.WriteLine($"WARNING: THERE IS UNMATCHED BALANCE OF {availableBalance} {instrument}. These funds could be older than 6 months or sent from another wallet or another instument pair, please proceed manually with them.");
            }

            return balanceSheet;
        }

        private IEnumerable<OrderHistoryItem> GetAllOrdersByStatus(string instrument, string status = "FILLED", string side = "")
        {
            var history = GetOrderHistory(instrument, DateTime.UtcNow.AddMonths(-6), 50);
            return history.Result.data.Where(p => p.status == status && (p.side == side || string.IsNullOrEmpty(side)));
        }
    }
}
