using RestSharp;

namespace Exchange.Api.Models.Market
{
    internal class GetCandlestickGetRequest : GetRequestModel
    {
        public override string Method { get => "public/get-candlestick"; set => base.Method = value; }

        /// <summary>
        /// Number of records to fetch, default 25.
        /// </summary>
        public int Count { get; set; } = 25;

        /// <summary>
        /// 1m, 5m, 15m, 30m: minutes
        /// 1h, 2h, 4h, 12h
        /// 1D, 7D, 14D
        /// 1M
        /// </summary>
        public string TimeFrame { get; set; } = "1m";

        public DateTimeOffset StartTimeStamp { get; set; }
        public DateTimeOffset EndTimeStamp { get; set; }

        /// <summary>
        /// BTCUSD-PERP
        /// </summary>
        public string Instrument { get; set; }
        public override IEnumerable<Parameter> Parameters { get => new List<Parameter>()
        {
            new QueryParameter("instrument_name", Instrument),
            new QueryParameter("count", Count.ToString()),
            new QueryParameter("timeframe", TimeFrame),
        }; set => base.Parameters = value; }
    }
}
