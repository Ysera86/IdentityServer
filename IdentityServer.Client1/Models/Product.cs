using System.Text.Json.Serialization;

namespace IdentityServer.Client1.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")] 
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
        [JsonPropertyName("stock")]
        public int Stock { get; set; }
    }
}
