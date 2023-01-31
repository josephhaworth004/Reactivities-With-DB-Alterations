using API.Extensions;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

// Below creates a Kestrel Server and reads from config files.
// Kestrel is the web server that's included and enabled by default in ASP.NET Core project templates.
var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.

// Services are things used in the application logic. 
// Services give more functionality to the app logic.
// Using Dependency Injection to inject these services into other classes in.
// Will be scoped to the HTTP requests
// Much of the code that would be here has been placed in AddApplicationServices.cs

builder.Services.AddControllers(opt =>{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// Much of the code that would be here has been placed in AddApplicationServices.cs
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();



// Referred to as "Middleware". Can do things with HTTP requests on their way in or out
// Ordering is important in middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS = Cross Origin Resource Sharing - for when data coming from different ports
app.UseCors("CorsPolicy"); // needs adding before UserAuthorization() The browser will check this first

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Refers to API Controllers

// Create database and auto clean up when finished.
using var scope = app.Services.CreateScope(); // Create a scope to access a service 
var services = scope.ServiceProvider;

// dotnet ef for Entity framework. 
// database: commands to mange the db
// dbcontext: commands to manage DBContext type
// migrations: commands to manage migrations

// Get access to our DataContext service specified above
try
{
    var context = services.GetRequiredService<Persistence.DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager); // Seeds the db with the data provided in Seed.cs
    // await will wait on an async class response
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error during migration");
}

app.Run();

