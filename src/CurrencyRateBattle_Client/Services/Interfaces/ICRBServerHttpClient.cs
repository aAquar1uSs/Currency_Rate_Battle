namespace CRBClient.Services.Interfaces;

public interface ICRBServerHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);

    Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content);

    Task<HttpResponseMessage> GetAsync(string requestUrl);

    void ClearHeader();
}
