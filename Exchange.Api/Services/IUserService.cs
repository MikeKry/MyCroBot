using Exchange.Api.Models.Account;

namespace Exchange.Api.Services
{
    public interface IUserService
    {
        UserBalanceResponse GetUserBalance(int requestId = 1);
        decimal GetUserCashBalance(int requestId = 1);
        decimal GetUserPositionBalance(string instrument = "USD", int requestId = 1);
    }
}