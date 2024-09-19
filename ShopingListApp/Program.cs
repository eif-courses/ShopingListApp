
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopingListApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDb>(options =>
{
    options.UseSqlite("Data Source=MyDatabase.db");
});

// LOGIN FORM
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MyDb>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(60));
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});



var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();


app.Run();