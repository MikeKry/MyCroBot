namespace Exchange.Api.Models.Trading
{
    public class CancelOrderRespose : ResponseModel<OrderResult>
    {
        public override string Method { get => "private/create-order"; set => base.Method = value; }
        public string Message { get; set; }
    }
}