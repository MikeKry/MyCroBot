namespace Exchange.Api.Models.Account
{
    internal class UserBalanceRequest : RequestModel
    {
        public override string Method { get => "private/user-balance"; set => base.Method = value; }
        public override Dictionary<string, object> Params { get => new Dictionary<string, object>(); set => base.Params = value; }
    }
}
