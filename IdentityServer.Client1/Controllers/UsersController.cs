using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            //User.Claims
            //HttpContext.AuthenticateAsync() 
            // Controller.HttpContext == View.Context

            //User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value // vs şeklinde de erişebiliriz.

            return View();
        }

        public async Task LogOut()
        {
            // şemalar birer cookie instanceı :  bayii kullanıcılları mı normal kullanıcılar mı çıkış yapıyor?

            //  => Client1.çıkış
            await HttpContext.SignOutAsync("mySiteCookies");  // program.cs içinde belirttiğim default şema 

            // => IdentityServer.çıkış
            // eğer bunu kapatırsak Identity servdan çıkmaz ve otomatik yönlendirme yapmaz, sadece client logout olur ve yeniden Users a erişrsem Identity arkada hala bağlı olduğu için otomatik olarak sayfa görüntülenir, login sayfası gelmez, tabi logout sonrası otomatik yönlendirmeyi yapan yer de burası olduğu için client logout olduktan snr sayfa yönlenmesi de olmaz o zaman en alta return redirecturl vb ile yönlendirmem uygun olurdu.
            await HttpContext.SignOutAsync("oidc"); // Identity atafında (AuthServer)  Config içinde PostLogoutRedirectUris=new List<string>{"\"https://localhost:7086/signout-callback-oidc"} belirttiğimiz adrese yönlendirir böylece

        }


        public async Task<IActionResult> GetRefreshToken()
        {
            // refresh tokenın cookie içinde  bilgileri var, yenisi alınınca onlar da güncellenmeli RefreshTokenUsage, RefreshTokenExpiration, AbsoluteRefreshTokenLifetime vb gibi yani tekrar signin gerek

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            HttpClient httpClient = new HttpClient();


            var discoveryEndpoint = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7112");

            if (discoveryEndpoint.IsError)
            {
                //discoveryEndpoint.Error...
            }

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = _configuration["Client1Mvc:ClientId"];// "Client1-Mvc";
            refreshTokenRequest.ClientSecret = _configuration["Client1Mvc:ClientSecret"]; //"secret";
            refreshTokenRequest.RefreshToken = refreshToken;
            refreshTokenRequest.Address = discoveryEndpoint.TokenEndpoint;

            var tokenResponse = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (tokenResponse.IsError)
            {
                // hata varsa yönlendir ....
            }

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name = OpenIdConnectParameterNames.AccessToken , Value = tokenResponse.AccessToken },
                new AuthenticationToken{Name = OpenIdConnectParameterNames.IdToken , Value = tokenResponse.IdentityToken },
                new AuthenticationToken{Name = OpenIdConnectParameterNames.RefreshToken , Value = tokenResponse.RefreshToken },
                new AuthenticationToken{Name = OpenIdConnectParameterNames.ExpiresIn , Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("O", CultureInfo.InvariantCulture) },
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties; // Authentication propertyeri al

            properties.StoreTokens(tokens); // token güncelleme

            await HttpContext.SignInAsync("mySiteCookies", authenticationResult.Principal, properties);


            return RedirectToAction("Index");

        }

        [Authorize(Roles ="admin")]
        public IActionResult AdminAction()
        {
            return View();
        }
        [Authorize(Roles ="customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
        [Authorize(Roles ="admin,customer")]
        public IActionResult AdminCustomerAction()
        {
            return View();
        }
    }
}
