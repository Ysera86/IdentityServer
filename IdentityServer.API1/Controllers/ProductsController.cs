using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize] //  bu yokken direk endpointe bağlanıp sonuç alabiliyorduk.artık token beklior > bknz Program.cs
    public class ProductsController : Controller
    {
        [HttpGet]
        [Authorize(policy: "ReadProduct")]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product { Id=1, Name="Kalem", Price=10, Stock=100 },
                new Product { Id=2, Name="Kitap", Price=20, Stock=500 },
                new Product { Id=3, Name="Silgi", Price=5, Stock=540 },
                new Product { Id=4, Name="Defter", Price=15, Stock=300 },
                new Product { Id=5, Name="Bant", Price=25, Stock=230 }
            };

            return Ok(productList);
        }

        [HttpPost]
        [Authorize(Policy = "UpdateOrCreateProduct")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"idsi {id} olan ürün güncellendi");
        }

        [HttpPost]
        [Authorize(Policy = "UpdateOrCreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
