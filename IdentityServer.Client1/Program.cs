using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//.. Bu kýsmý ekleyerek merkezi bir üyelik sistemine çevirmiþ olduk
// üyelik sistemlerini (cookie mekanizmalarýný ayýrmak için þemalar kullanýlýr, ör. bayii ve kullanýcý için mesela. Ayrý þemalar gerekecekti.

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "mySiteCookies";  // Aþaðýdakilerle eþleþmeli
    options.DefaultChallengeScheme = "oidc"; // Aþaðýdakilerle eþleþmeli // open Id connect
}).AddCookie("mySiteCookies").AddOpenIdConnect("oidc", opts =>
{
    opts.SignInScheme = "mySiteCookies";
    opts.Authority = "https://localhost:7112"; // Auth Server yetkili
    opts.ClientId = "Client1-Mvc";
    opts.ClientSecret = "secret";
    opts.ResponseType = "code id_token"; // Notes.Return Type

    opts.GetClaimsFromUserInfoEndpoint = true; //kullanýcýlara verdiðimiz ek Claimleri IdentityServer direk Coolie içine eklemez þiþirmemek için (UserInfoEndpointinden eriþebiliriz) ya da bu þekilde IdentityServer'ýn direk bu bilgileri alýp Claims içinde getirmesini saðlayabiliriz. > User.Claims içine ekler

    opts.SaveTokens = true; // baþarýlý bir authorizationdan snr access ve refresh tokenlarý AuthenticationProperties içine kaydetmesini saðlar. Varsayýlan olarak cookie þiþmesin diye kaydolmazlar

    opts.Scope.Add("api1.read"); // IdentityServer (AuthServer) Clientýnda (Config. Client1-Mvc) claimlerinde "api1.read varsa, bu satýr sayesinde eriþebilir artýk client API1'e read izniyle
     //opts.Scope.Add("api1.write"); // mesla bu ekli deðil buna izin vermeyecek hata verir eklersem buraya

    opts.Scope.Add("offline_access"); // Config. Client1-Mvc için refresh token eklendi scope da eklendi, buraya da eklendi.

    opts.Scope.Add("CountryAndCity");  // scope eklemek yetmez burada talep ettim. custom claim olduklarý için de maplemem lazým
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
