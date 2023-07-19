using IdentityModel.Client;
using IdentityServer.Client1.Models;
using IdentityServer.Client1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.Json;

namespace IdentityServer.Client1.Controllers
{
    //    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IApiResourceHttpClient _apiResourceHttpClient;
        public ProductsController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        #region Üyeliksiz, ClientId ve ClientSecret ile token alıp APIdan data çekme
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


            List<Product> productList = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                productList = JsonSerializer.Deserialize<List<Product>>(content);


                // https://localhost:7086 /> Client1 url
                // https://localhost:7086/Products adresine gitip breakpoint ile contenti okumam gerek 

            }
            else
            {
                // hata var data alınamadı logla vs
            }


            return View(productList);
        }

        #endregion
      
        #region Üyelikli token ile APIdan data çekme

        [Authorize]
        public async Task<IActionResult> IndexCentralized()
        {
            HttpClient client = new HttpClient();

            #region Artık gidip token çekmeme gerek yok, cookie kayıtlı ve token içinde

            //var discoveryEndpoint = await client.GetDiscoveryDocumentAsync("https://localhost:7112");

            //if (discoveryEndpoint.IsError)
            //{
            //    //discoveryEndpoint.Error...
            //}

            //ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest()
            //{
            //    ClientId = _configuration["Client:ClientId"],
            //    ClientSecret = _configuration["Client:ClientSecret"],
            //    Address = discoveryEndpoint.TokenEndpoint
            //};


            //var token = await client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);


            //if (token.IsError)
            //{
            //    //token.Error...
            //}


            //client.SetBearerToken(token.AccessToken); // headera "Authorization" ile token ekliyor
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue...... demek zorunda kalıp headera eklemek zorunda kalacaktık

            #endregion

            // cookieden token okuyan. üstteki region yerine bu satır ile halledip alltaki satırı değiştirip direk bu tokenı verdik
            var token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            client.SetBearerToken(token);

            // zaten api1.read iznimiz var [Authorize] ekledik, burada okuyabiliriz.

            var response = await client.GetAsync("https://localhost:7007/api/Products/GetProducts");


            List<Product> productList = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                productList = JsonSerializer.Deserialize<List<Product>>(content);


                // https://localhost:7086 /> Client1 url
                // https://localhost:7086/Products adresine gitip breakpoint ile contenti okumam gerek 

            }
            else
            {
                // hata var data alınamadı logla vs
            }


            return View(productList);
        }

        #endregion

        #region Üyelikli token ile APIdan data çekme 2 : Best Practice with HttpContextAccessor

        [Authorize]
        public async Task<IActionResult> IndexCentralizedBestPractice()
        {
            //HttpClient client = new HttpClient();
            //var token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //client.SetBearerToken(token);

            // üstteki 3 satır gitti bu satır geldi! detaylar Notes içinde!
            HttpClient client = await _apiResourceHttpClient.GetHttpClient();

            // Aslında product service içine crud ile DIdan bu işlemi yapmak laxım..
            var response = await client.GetAsync("https://localhost:7007/api/Products/GetProducts");


            List<Product> productList = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                productList = JsonSerializer.Deserialize<List<Product>>(content);
            }
            else
            {
                // hata var data alınamadı logla vs
            }


            return View(productList);
        }

        #endregion



    }
}
