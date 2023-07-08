using BotcRoles.Models;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Web;

const string tbaServerId = "765137571608920074";
const string storyTellerRoleId = "797739056406069279";

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


builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", o =>
    {
        o.LoginPath = "/login";
        var del = o.Events.OnRedirectToAccessDenied;

        o.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.RedirectUri = $"/unauthorized";
            //ctx.RedirectUri = "/";  // TODO : Create an access denied page ?
            return del(ctx);
        };
    })
    .AddOAuth("discord", o =>
    {
        o.SignInScheme = "cookie";
        o.ClientId = config.GetValue<string>("DiscordAuth:ClientId");
        o.ClientSecret = config.GetValue<string>("DiscordAuth:ClientSecret");

        o.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
        o.TokenEndpoint = "https://discord.com/api/oauth2/token";
        o.CallbackPath = "/auth/discord/callback";
        o.SaveTokens = true;

        o.UserInformationEndpoint = "https://discord.com/api/users/@me";

        o.Scope.Clear();
        o.Scope.Add("identify");
        o.Scope.Add("guilds.members.read");

        o.Events.OnCreatingTicket = async ctx =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint + $"/guilds/{tbaServerId}/member");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using var result = await ctx.Backchannel.SendAsync(request);
            var user = await result.Content.ReadFromJsonAsync<JsonElement>();
            var userGuildDetails = JsonSerializer.Deserialize<UserGuildDetails>(user);

            if (userGuildDetails == null || userGuildDetails.user == null)
            {
                return;
            }

            var userId = userGuildDetails.user.id;
            var db = ctx.HttpContext.RequestServices.GetRequiredService<Database>();
            db[userId] = ctx.AccessToken!;

            var identity = ctx.Principal.Identities.First();
            identity.AddClaim(new Claim("ds-role-storyteller", userGuildDetails.roles.Contains(storyTellerRoleId) ? "y" : "n"));
            identity.AddClaim(new Claim("user_id", userId));
        };
    });


builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("storyteller", pb =>
    {
        pb.AddAuthenticationSchemes("cookie")
            .RequireClaim("ds-role-storyteller", "y");
    });
});


builder.Services.AddSingleton<Database>();
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


app.MapGet("/login", (HttpContext ctx) =>
{
    var returnUrl = HttpUtility.ParseQueryString(ctx.Request.QueryString.Value).Get("ReturnUrl");
    var redirectUri = $"https://{ctx.Request.Host}{returnUrl}";

    return Results.Challenge(
        new AuthenticationProperties()
        {
            RedirectUri = redirectUri
        },
        authenticationSchemes: new List<string>() { "discord" });
});

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync("cookie");

    return "logged out";
});

app.MapGet("/unauthorized", (HttpContext ctx) =>
{
    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
    return "Unauthorized";
});

app.Run();

public partial class Program { } // Usefull to expose class Program to test project


// Data to log users
public class Database : Dictionary<string, string>
{

}

public class UserGuildDetails
{
    public string[] roles { get; set; }
    public DiscordUser user { get; set; }
}

public class DiscordUser
{
    public string id { get; set; }
    public string? username { get; set; }
    public string? avatar { get; set; }
    public string? discriminator { get; set; }
    public int public_flags { get; set; }
    public int flags { get; set; }
    public string? banner { get; set; }
    public string? accent_color { get; set; }
    public string? global_name { get; set; }
    public string? avatar_decoration { get; set; }
    public string? display_name { get; set; }
    public string? banner_color { get; set; }
}

