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
