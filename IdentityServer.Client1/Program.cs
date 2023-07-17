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
