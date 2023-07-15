using IdentityServer.API2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize] //  bu yokken direk endpointe bağlanıp sonuç alabiliyorduk.artık token beklior > bknz Program.cs
    public class PicturesController : Controller
    {
        [HttpGet]
        //  [Authorize] // ben controller bazında eklemek istedim
        public IActionResult GetPictures()
        {
            var picturesList = new List<Picture>()
            {
                new Picture { Id=1, Name="Doğa resmi", Url="dogaresmi.jpg"},
                new Picture { Id=2, Name="Aslan resmi", Url="aslanresmi.jpg" },
                new Picture { Id=3, Name="Fare resmi", Url="fateresmi.jpg"},
                new Picture { Id=4, Name="Fil resmi", Url="filresmi.jpg"},
                new Picture { Id=5, Name="Kurt resmi", Url="kurtresmi.jpg"},
                new Picture { Id=5, Name="Köpek resmi", Url="köpekresmi.jpg"}
            };

            return Ok(picturesList);
        }
    }
}
