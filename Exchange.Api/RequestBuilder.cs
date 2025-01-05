using System.Security.Cryptography;
using System.Text;
using Exchange.Api.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Exchange.Api
{
    public partial class RequestBuilder : IRequestBuilder
    {
        private readonly string _apiRoot = "https://api.crypto.com/exchange/v1/";
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public RequestBuilder(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public RE SendGetRequest<RQ, RE, RED>(RQ requestParams)
            where RQ : GetRequestModel
            where RE : ResponseModel<RED>
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var client = new RestClient();
            var request = new RestRequest(_apiRoot + requestParams.Method, Method.Get);
            request.AddOrUpdateParameters(requestParams.Parameters);
            Console.WriteLine("params:" + string.Join(", ", request.Parameters.Select(p => p.Name + p.Value)));
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var responseModel = JsonConvert.DeserializeObject<RE>(response.Content);
                if (responseModel != null && responseModel.Code == 0)
                {
                    return responseModel;
                }
                else
                {
                    throw new Exception(responseModel?.Method ?? "Unknown error occurred.");
                }
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, {response.Content}");
            }
        }

        public RE SendPostRequest<RQ, RE, RED>(RQ requestBody)
            where RQ : RequestModel
            where RE : ResponseModel<RED>
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            requestBody.Nonce = timestamp;
            requestBody.ApiKey = _apiKey;

            var client = new RestClient();
            var request = new RestRequest(_apiRoot + requestBody.Method, Method.Post);
            string sign = GetSign(requestBody);
            requestBody.Sig = sign;

            request.AddJsonBody(JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
            {
                Converters = { new FormatNumbersAsTextConverter() }
            }), ContentType.Json);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var responseModel = JsonConvert.DeserializeObject<RE>(response.Content);
                if (responseModel != null && responseModel.Code == 0)
                {
                    return responseModel;
                }
                else
                {
                    throw new Exception(responseModel?.Method ?? "Unknown error occurred.");
                }
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, {response.Content}");
            }
        }


        public string GetSign(RequestModel request)
        {
            // Ensure the params are alphabetically sorted by key
            string parameters = request.Params is null ? string.Empty : RequestBuilderHelper.DictionaryToString(request.Params);

            string sigPayload = request.Method + request.Id + _apiKey + parameters + request.Nonce;

            Console.WriteLine(sigPayload);
            var hash = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret));
            var computedHash = hash.ComputeHash(Encoding.UTF8.GetBytes(sigPayload));
            return Convert.ToHexString(computedHash).ToLower();
        }
    }
}