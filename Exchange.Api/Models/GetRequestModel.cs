using RestSharp;

namespace Exchange.Api.Models
{
    [Serializable]
    public class GetRequestModel
    {
        public virtual string Method { get; set; }
        public virtual IEnumerable<Parameter> Parameters { get; set; } = new List<Parameter>();
    }
}
