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
    options.Authority = "https://localhost:7112"; // kim yay�nl�yor? kim sahibi jwtnin? =Z authserver�m�z
    options.Audience = "resource_api1"; // kimi kabul ediyorum ( token�n aud k�sm�ndaki)

});

builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("ReadProduct", policy =>
    {
        policy.RequireClaim("scope", "api1.read");
    });

    configure.AddPolicy("UpdateOrCreateProduct", policy =>
    {
        policy.RequireClaim("scope", new[] { "api1.update", "api1.write" });
    });
});
// 2si ayn� olmal�

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
