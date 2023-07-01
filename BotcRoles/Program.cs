using BotcRoles.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.

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
            //.WithOrigins("http://192.168.1.48:3000",
            //    "https://192.168.1.48:3000",
            //    "http://192.168.1.48:3000/*",
            //    "https://192.168.1.48:3000/*")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddOAuth();

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

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] { new Claim("user_id", Guid.NewGuid().ToString()) },
            "cookie"
        )
    ),
    authenticationScheme: "cookie"
));

app.MapGet("/yt/info", (IHttpClientFactory clientFactory) =>
{

});

app.Run();

public partial class Program { } // utile pour exposer la class Program au projet de tests


/*
 
 using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

const string tbaServerId = "765137571608920074";
const string storyTellerRoleId = "797739056406069279";

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddOAuth("discord", o =>
    {
        o.SignInScheme = "cookie";
        o.ClientId = config.GetValue<string>("Discord:ClientId");
        o.ClientSecret = config.GetValue<string>("Discord:ClientSecret");

        o.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
        o.TokenEndpoint = "https://discord.com/api/oauth2/token";
        o.CallbackPath = "/auth/discord/callback";
        o.SaveTokens = true;

        o.UserInformationEndpoint = "https://discord.com/api/users/@me";

        o.ClaimActions.MapJsonKey("sub", "id");
        o.ClaimActions.MapJsonKey(ClaimTypes.Name, "global_name");
        o.ClaimActions.MapJsonKey("roles", "roles");

        o.Events.OnCreatingTicket = async ctx =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint + $"/guilds/{tbaServerId}/member");
            //using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using var result = await ctx.Backchannel.SendAsync(request);
            var user = await result.Content.ReadFromJsonAsync<JsonElement>();

            ctx.RunClaimActions(user);
        };

        o.Scope.Add("identify");
        //o.Scope.Add("guilds");
        o.Scope.Add("guilds.members.read");
    });

var app = builder.Build();

app.UseAuthentication();


app.MapGet("/", (HttpContext ctx) =>
{
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});

app.MapGet("/login", () =>
{
    return Results.Challenge(
        new AuthenticationProperties()
        {
            RedirectUri = "https://localhost:7108/"
        },
        authenticationSchemes: new List<string>() { "discord" });
});

app.Run();


 
 */