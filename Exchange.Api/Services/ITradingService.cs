using Exchange.Api.Models.Trading;

namespace Exchange.Api.Services
{
    public interface ITradingService
    {
        CreateOrderRespose CreateOrder(string instrument, decimal price, decimal quantity, string side = "BUY", string type = "LIMIT", int requestId = 1);
    }
}