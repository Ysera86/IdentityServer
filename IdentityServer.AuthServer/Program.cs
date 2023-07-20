using IdentityServer.AuthServer;
using IdentityServer.AuthServer.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    
    //.AddSigningCredential() // proda ��karken a�
    .AddDeveloperSigningCredential(); // proda ��karken kapat  => projeye eklendi :  tempkey.jwk


// SymmetricKey:  Jwt hem do�rulamak hem imzalamak i�in ayn� �ifreyi kullan�yorsak
// ASymmetricKey:  public key ve private key var, privste tutulur, public key �ifreyi ��zeceklere verilir, private ile gelen datay� public sahipleri do�rulayabilir.

// IdentityServer asimetrik �ifrelemeke kullan�r
// ilgili token� privateKey ile �ifreler,   authorizationServer�n endpointi var herkese a��k olan, apilar burdan publik key ��reniyorlar. gelen token� bu publick key ile do�ruluyorlar, otomatik olarak.

// AddDeveloperSigningCredential > uygulama geli�tirme esnas�nda private/public olu�turmakla u�ra�ma, development i�in imzalama credntial� olu�turucam onu kullan demek - credential bilgileri uygulama i�inde-, proda ge�erken AddSigningCredential kullan

// credential bilgileri uygulama i�inde olmamal� prodda, uygulamay� nerede host ediyorsak oradan almal�

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
