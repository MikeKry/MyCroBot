using System.ComponentModel.DataAnnotations;

namespace Exchange.Api.Models.History
{
    internal class GetOrderHistoryRequest : RequestModel
    {
        public override string Method { get => "private/get-order-history"; set => base.Method = value; }
        public override Dictionary<string, object> Params { get => GetOrderHistoryParameters.ObjectToDictionary(); set => base.Params = value; }

        [Required]
        public GetOrderHistoryParameters GetOrderHistoryParameters { get; set; }
    }

    public class GetOrderHistoryParameters
    {
        public string instrument_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        ////public string limit { get; set; }
    }
}
