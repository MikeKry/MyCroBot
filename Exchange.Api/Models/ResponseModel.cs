using System.Text.Json.Serialization;

namespace Exchange.Api.Models
{
    [Serializable]
    public class ResponseModel<T>
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName ("method")]
        public virtual string Method { get; set; }
        [JsonPropertyName("code")]
        public virtual int Code { get; set; }
        [JsonPropertyName("result")]
        public virtual T Result { get; set; }
    }
}
