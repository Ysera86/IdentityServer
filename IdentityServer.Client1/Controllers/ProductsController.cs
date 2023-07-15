using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();

            var discoveryEndpoint = await client.GetDiscoveryDocumentAsync("https://localhost:7112");

            if (discoveryEndpoint.IsError)
            {
                //discoveryEndpoint.Error...
            }

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest() 
            { 
                ClientId = _configuration["Client:ClientId"],
                ClientSecret = _configuration["Client:ClientSecret"],
                Address = discoveryEndpoint.TokenEndpoint
            };

            
            var token = await client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (token.IsError)
            {
                //token.Error...
            }
            

            client.SetBearerToken(token.AccessToken); // headera "Authorization" ile token ekliyor
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue...... demek zorunda kalıp headera eklemek zorunda kalacaktık

            var response = await client.GetAsync("https://localhost:7007/api/Products/GetProducts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();


                // https://localhost:7086 /> Client1 url
                // https://localhost:7086/Products adresine gitip breakpoint ile contenti okumam gerek 

            }
            else
            {
                // hata var data alınamadı logla vs
            }


            return View();
        }
    }
}
