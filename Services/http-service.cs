using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace clash_up_frontend.Services
{
    public interface IHttpService
    {
        Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null);
    }

    public class HttpService : IHttpService
    {
        private readonly RestClient _client;

        public HttpService(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null)
        {
            var request = new RestRequest(endpoint, Method.Get);

            if (queryParams != null)
                foreach (var param in queryParams)
                    request.AddQueryParameter(param.Key, param.Value);

            var response = await _client.ExecuteAsync<T>(request);
            return HandleResponse(response);
        }

        private static T? HandleResponse<T>(RestResponse<T> response)
        {
            if (!response.IsSuccessful)
                throw new Exception($"HTTP Error {response.StatusCode}: {response.ErrorMessage ?? response.Content}");

            return response.Data;
        }
    }
}
