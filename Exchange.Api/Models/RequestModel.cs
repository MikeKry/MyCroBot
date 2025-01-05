using Newtonsoft.Json;

namespace Exchange.Api.Models
{
    [Serializable]
    public class RequestModel
    {
        [JsonProperty("id", Order = 0)]
        public long Id { get; set; }
        [JsonProperty("method", Order = 1)]
        public virtual string Method { get; set; }
        [JsonProperty("api_key", Order = 2)]
        public virtual string ApiKey { get; set; }
        [JsonProperty("params", Order = 3)]
        public virtual Dictionary<string, object> Params { get; set; }
        [JsonProperty("nonce", Order = 3)]
        public long Nonce { get; set; }
        [JsonProperty("sig", Order = 4)]
        public virtual string Sig { get; set; }
    }
}
