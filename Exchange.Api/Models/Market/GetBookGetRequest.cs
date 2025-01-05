using RestSharp;

namespace Exchange.Api.Models.Market
{
    internal class GetBookGetRequest : GetRequestModel
    {
        public override string Method { get => "public/get-book"; set => base.Method = value; }

        /// <summary>
        /// Number of records to fetch, max 50.
        /// </summary>
        public string Depth { get; set; } = "10";

        /// <summary>
        /// BTCUSD-PERP
        /// </summary>
        public string Instrument { get; set; }
        public override IEnumerable<Parameter> Parameters { get => new List<Parameter>()
        {
            new QueryParameter("instrument_name", Instrument),
            new QueryParameter("depth", Depth)
        }; set => base.Parameters = value; }
    }
}
