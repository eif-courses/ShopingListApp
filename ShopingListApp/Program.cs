
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
        policy.WithOrigins("http://localhost:3000", "http://localhost:63342")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});



var app = builder.Build();


using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var context = services.GetRequiredService<MyDb>();
    context.Database.Migrate();
    
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await SeedData.Initialize(userManager, roleManager);
    
}


app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();


app.Run();