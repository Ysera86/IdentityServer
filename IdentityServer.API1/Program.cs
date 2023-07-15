using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//..

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:7112"; // kim yayýnlýyor? kim sahibi jwtnin? =Z authserverýmýz
    options.Audience = "resource_api1"; // kimi kabul ediyorum ( tokenýn aud kýsmýndaki)

});
// 2si ayný olmalý

//..

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//..

app.UseAuthentication();

//...

app.UseAuthorization();

app.MapControllers();

app.Run();
