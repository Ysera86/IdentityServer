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
    }
}
