using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;

namespace Exchange.Api.Models.Market
{
    public class GetCandlestickResponse : ResponseModel<CandlestickResponse>
    {
        public override string Method { get => "public/get-candlestick"; set => base.Method = value; }
    }

    public class CandlestickResponse
    {
        public string interval { get; set; }
        public CandlestickData[] data { get; set; }
        public string instrument_name { get; set; }
    }

    public class CandlestickData
    {
        [JsonProperty("o")]
        public string OpenPrice { get; set; }
        [JsonProperty("h")]
        public string HighPrice { get; set; }
        [JsonProperty("l")]
        public string LowPrice { get; set; }
        [JsonProperty("c")]
        public string ClosePrice { get; set; }
        [JsonProperty("v")]
        public string Volume { get; set; }
        [JsonProperty("t")]
        public long StartTime { get; set; }
    }

}