using System.Globalization;
using Exchange.Api.Models.Trading;

namespace Exchange.Api.Services
{
    public class TradingService : ITradingService
    {
        private readonly IRequestBuilder _requestBuilder;
        public TradingService(IRequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
        }

        public CreateOrderRespose CreateOrder(string instrument, decimal price, decimal quantity, string side = "BUY", string type = "LIMIT", int requestId = 1)
        {
            var requestBody = new CreateOrderRequest()
            {
                CreateOrderParameters = new CreateOrderParameters()
                {
                    instrument_name = instrument,
                    type = type,
                    price = price.ToString(CultureInfo.InvariantCulture), // can be also handled by json converter
                    quantity = quantity.ToString(CultureInfo.InvariantCulture), // can be also handled by json converter
                    side = side,
                    client_oid = Guid.NewGuid().ToString(),
                },
            };

            var result = _requestBuilder.SendPostRequest<CreateOrderRequest, CreateOrderRespose, OrderResult>(requestBody);
            return result;
        }
    }
}
