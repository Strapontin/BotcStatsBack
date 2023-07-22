using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BotcRoles
{
    public class IsStoryTellerRequirement : IAuthorizationRequirement
    {
        public IsStoryTellerRequirement() { }
    }


    public class IsStoryTellerAuthorizationHandler : AuthorizationHandler<IsStoryTellerRequirement>
    {
        const string _tbaServerId = "765137571608920074";
        const string storyTellerRoleId = "797739056406069279";

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStoryTellerRequirement requirement)
        {
            string? bearer = ((DefaultHttpContext)context.Resource).Request?.Headers[HeaderNames.Authorization].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(bearer))
            {
                context.Fail();
                return;
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                $"https://discord.com/api/users/@me/guilds/{_tbaServerId}/member")
            {
                Headers = { { HeaderNames.Authorization, bearer } }
            };

            var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                var userGuildDetails = JsonSerializer.Deserialize<UserGuildDetails>(contentStream);

                if (userGuildDetails == null || userGuildDetails.user == null)
                {
                    context.Fail();
                    return;
                }

                if (userGuildDetails.roles.Contains(storyTellerRoleId))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail();
            return;
        }
    }
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
