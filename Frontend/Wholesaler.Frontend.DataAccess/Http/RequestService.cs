using System.Net.Http.Json;
using Newtonsoft.Json;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.DataAccess.Http;

public abstract class RequestService
{
    protected async Task<ExecutionResultGeneric<TResponseModel>> SendAsync<TRequestModel, TResponseModel>(Request<TRequestModel, TResponseModel> request)
    {
        using (var httpClient = new HttpClient())
        {
            var requestMessage = new HttpRequestMessage(request.Method, request.Path);

            var content = request.Content;

            requestMessage.Content = JsonContent.Create(content);
            var result = await httpClient.SendAsync(requestMessage);
            var resultContent = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<TResponseModel>(resultContent);
                return ExecutionResultGeneric<TResponseModel>.CreateSuccessful(response);
            }

            return ExecutionResultGeneric<TResponseModel>.CreateFailed(resultContent);
        }
    }
}
