namespace Exchange.Api.Models.Trading
{
    public class CreateOrderRespose : ResponseModel<OrderResult>
    {
        public override string Method { get => "private/create-order"; set => base.Method = value; }
    }
}