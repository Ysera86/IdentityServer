using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//.. Bu k�sm� ekleyerek merkezi bir �yelik sistemine �evirmi� olduk
// �yelik sistemlerini (cookie mekanizmalar�n� ay�rmak i�in �emalar kullan�l�r, �r. bayii ve kullan�c� i�in mesela. Ayr� �emalar gerekecekti.

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "mySiteCookies";  // A�a��dakilerle e�le�meli
    options.DefaultChallengeScheme = "oidc"; // A�a��dakilerle e�le�meli // open Id connect
}).AddCookie("mySiteCookies").AddOpenIdConnect("oidc", opts =>
{
    opts.SignInScheme = "mySiteCookies";
    opts.Authority = "https://localhost:7112"; // Auth Server yetkili
    opts.ClientId = "Client1-Mvc";
    opts.ClientSecret = "secret";
    opts.ResponseType = "code id_token"; // Notes.Return Type

    opts.GetClaimsFromUserInfoEndpoint = true; //kullan�c�lara verdi�imiz ek Claimleri IdentityServer direk Coolie i�ine eklemez �i�irmemek i�in (UserInfoEndpointinden eri�ebiliriz) ya da bu �ekilde IdentityServer'�n direk bu bilgileri al�p Claims i�inde getirmesini sa�layabiliriz. > User.Claims i�ine ekler

    opts.SaveTokens = true; // ba�ar�l� bir authorizationdan snr access ve refresh tokenlar� AuthenticationProperties i�ine kaydetmesini sa�lar. Varsay�lan olarak cookie �i�mesin diye kaydolmazlar

    opts.Scope.Add("api1.read"); // IdentityServer (AuthServer) Client�nda (Config. Client1-Mvc) claimlerinde "api1.read varsa, bu sat�r sayesinde eri�ebilir art�k client API1'e read izniyle
     //opts.Scope.Add("api1.write"); // mesla bu ekli de�il buna izin vermeyecek hata verir eklersem buraya

    opts.Scope.Add("offline_access"); // Config. Client1-Mvc i�in refresh token eklendi scope da eklendi, buraya da eklendi.

    opts.Scope.Add("CountryAndCity");  // scope eklemek yetmez burada talep ettim. custom claim olduklar� i�in de maplemem laz�m
    opts.ClaimActions.MapUniqueJsonKey("country", "country");
    opts.ClaimActions.MapUniqueJsonKey("city", "city");

});

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

app.UseAuthentication();

//..
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
