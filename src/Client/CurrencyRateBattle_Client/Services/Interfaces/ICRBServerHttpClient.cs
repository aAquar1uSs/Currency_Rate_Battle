namespace CRBClient.Services.Interfaces;

public interface ICRBServerHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);

    Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T content, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAsync(string requestUrl, CancellationToken cancellationToken);

    void ClearHeader();
}
