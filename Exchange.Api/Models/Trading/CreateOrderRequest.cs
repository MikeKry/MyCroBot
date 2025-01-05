using System.ComponentModel.DataAnnotations;

namespace Exchange.Api.Models.Trading
{
    internal class CreateOrderRequest : RequestModel
    {
        public override string Method { get => "private/create-order"; set => base.Method = value; }
        public override Dictionary<string, object> Params { get => CreateOrderParameters.ObjectToDictionary(); set => base.Params = value; }
        
        [Required]
        public CreateOrderParameters CreateOrderParameters { get; set; }
    }

    public class CreateOrderParameters
    {
        [Required]
        public string instrument_name { get; set; }
        [Required]
        public string side { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public string price { get; set; }
        [Required]
        public string quantity { get; set; }
        public string client_oid { get; set; }
    }
}
