using System.ComponentModel.DataAnnotations;

namespace Exchange.Api.Models.Trading
{
    internal class CancelOrderRequest : RequestModel
    {
        public override string Method { get => "private/cancel-order"; set => base.Method = value; }
        public override Dictionary<string, object> Params { get => CancelOrderParameters.ObjectToDictionary(); set => base.Params = value; }
        
        [Required]
        public CancelOrderParameters CancelOrderParameters { get; set; }
    }

    public class CancelOrderParameters
    {
        [Required]
        public string order_id { get; set; }
    }
}
