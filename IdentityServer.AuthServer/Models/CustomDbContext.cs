using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions opts) : base(opts)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //passwordü crypt edip dbde sakla : md5 sha256 sha512 vb, hashlenen geri döndürülmez o nednele 2 method gerekir örn:
            // 1 - hash() : passwordu hashle kaydet
            // 2 - confirmHash() :  kullanıcıdan alınan şifreyi hashle dbdekiyle karşılaştır

            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser { Id = 1, UserName = "merve.ugursac34", Password = "password", Email = "merve.ugursac34@gmail.com", City = "İstanbul" },
                new CustomUser { Id = 2, UserName = "merve.ugursac35", Password = "password", Email = "merve.ugursac35@gmail.com", City = "Muğla" },
                new CustomUser { Id = 3, UserName = "merve.ugursac36", Password = "password", Email = "merve.ugursac36@gmail.com", City = "NY" }
                );



            base.OnModelCreating(modelBuilder);
        }
    }
}
