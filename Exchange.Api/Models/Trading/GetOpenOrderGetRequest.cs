using RestSharp;

namespace Exchange.Api.Models.Trading
{
    internal class GetOpenOrderGetRequest : GetRequestModel
    {
        public override string Method { get => "private/get-open-orders"; set => base.Method = value; }

        /// <summary>
        /// BTCUSD-PERP
        /// </summary>
        public string Instrument { get; set; }
        public override IEnumerable<Parameter> Parameters { get => new List<Parameter>()
        {
            new QueryParameter("instrument_name", Instrument),
        }; set => base.Parameters = value; }
    }
}
