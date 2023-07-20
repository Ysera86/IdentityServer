using IdentityServer.AuthServer;
using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repositories;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICustomUserRepository,CustomUserRepository>();

builder.Services.AddDbContext<CustomDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"));
});

//..
builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())

    .AddInMemoryIdentityResources(Config.GetIdentityResources())    
    .AddTestUsers(Config.GetTestUsers().ToList())
    
    //.AddSigningCredential() // proda çýkarken aç
    .AddDeveloperSigningCredential(); // proda çýkarken kapat  => projeye eklendi :  tempkey.jwk


// SymmetricKey:  Jwt hem doðrulamak hem imzalamak için ayný þifreyi kullanýyorsak
// ASymmetricKey:  public key ve private key var, privste tutulur, public key þifreyi çözeceklere verilir, private ile gelen datayý public sahipleri doðrulayabilir.

// IdentityServer asimetrik þifrelemeke kullanýr
// ilgili tokený privateKey ile þifreler,   authorizationServerýn endpointi var herkese açýk olan, apilar burdan publik key öðreniyorlar. gelen tokený bu publick key ile doðruluyorlar, otomatik olarak.

// AddDeveloperSigningCredential > uygulama geliþtirme esnasýnda private/public oluþturmakla uðraþma, development için imzalama credntialý oluþturucam onu kullan demek - credential bilgileri uygulama içinde-, proda geçerken AddSigningCredential kullan

// credential bilgileri uygulama içinde olmamalý prodda, uygulamayý nerede host ediyorsak oradan almalý

//..



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//..

app.UseIdentityServer();

//..

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
