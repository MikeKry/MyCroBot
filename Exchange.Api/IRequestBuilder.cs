using Exchange.Api.Models;

namespace Exchange.Api
{
    public interface IRequestBuilder
    {
        string GetSign(RequestModel request);
        RE SendGetRequest<RQ, RE, RED>(RQ requestParams)
            where RQ : GetRequestModel
            where RE : ResponseModel<RED>;
        RE SendPostRequest<RQ, RE, RED>(RQ requestBody)
            where RQ : RequestModel
            where RE : ResponseModel<RED>;
    }
}