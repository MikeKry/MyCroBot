using System.Globalization;
using Exchange.Api.Models.Account;

namespace Exchange.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IRequestBuilder _requestBuilder;

        public UserService(IRequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
        }

        public UserBalanceResponse GetUserBalance(int requestId = 1)
        {
            var requestBody = new UserBalanceRequest()
            {
                Id = requestId,
            };

            var result = _requestBuilder.SendPostRequest<UserBalanceRequest, UserBalanceResponse, UserBalanceResult>(requestBody);
            return result;
        }

        public decimal GetUserCashBalance(int requestId = 1)
        {
            var apiResult = GetUserBalance();
            return decimal.TryParse(apiResult.Result.data.First().total_cash_balance, CultureInfo.InvariantCulture, out decimal parsed) ? parsed : 0.0m;
        }

        public decimal GetUserPositionBalance(string instrument = "USD", int requestId = 1)
        {
            // probably wrong, need to check collateral
            var apiResult = GetUserBalance();
            return decimal.TryParse(apiResult.Result.data.First().position_balances.First(p => p.instrument_name == instrument).quantity, CultureInfo.InvariantCulture, out decimal parsed) ? parsed : 0.0m;
        }
    }
}
