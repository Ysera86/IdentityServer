namespace IdentityServer.AuthServer.Models
{
    public class CustomUser
    {
        public int Id { get; set; } // best practice Guid
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
    }
}
