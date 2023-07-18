using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityServer.Client1.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
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
    }
}
