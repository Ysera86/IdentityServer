namespace IdentityServer.Client1.Services
{
    public interface IApiResourceHttpClient
    {
        Task<HttpClient> GetHttpClient();
    }
}
