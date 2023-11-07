using BotcRoles.AuthorizationHandlers;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ModelContext>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{

//});

builder.Services.AddSingleton<IAuthorizationHandler, IsStoryTellerAuthorizationHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //options.Authority = "https://localhost:5000"; // URL of Identity Server; use IConfiguration instead of hardcoding 
    //options.Audience = "client.mydomain.com"; // ID of the client application; either hardcoded or configureable via IConfiguration if needed 
    options.RequireHttpsMetadata = true; // require HTTPS (may be disabled in development, but advice against it)
    options.SaveToken = true; // cache the token for faster authentication
    options.IncludeErrorDetails = true; // get more details on errors; may be disabled in production 
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    //options.AddPolicy("IsConnectedToDiscord", policy => policy.Requirements.Add(new IsConnectedToDiscordRequirement()));
    options.AddPolicy("IsStoryTeller", policy => policy.Requirements.Add(new IsStoryTellerRequirement()));
});


//builder.Services.AddSingleton<Database>();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();

public partial class Program { } // Usefull to expose class Program to test project

