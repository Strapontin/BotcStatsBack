using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
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
        const string _storyTellerRoleId = "797739056406069279";
        List<BearerStoryTeller> _bearerIsStoryTeller = new();

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStoryTellerRequirement requirement)
        {
            string? bearer = ((DefaultHttpContext)context.Resource).Request?.Headers[HeaderNames.Authorization].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(bearer))
            {
                context.Fail();
                return;
            }

            List<string> bearerExpired = new();
            foreach (var b in _bearerIsStoryTeller)
            {
                if (DateTime.Now.Subtract(b.DateChecked).TotalMinutes >= 10)
                {
                    bearerExpired.Add(b.Bearer);
                }
            }
            _bearerIsStoryTeller.RemoveAll(b => bearerExpired.Contains(b.Bearer));

            // If the user has already authenticated 
            if (_bearerIsStoryTeller.Any(b => b.Bearer == bearer))
            {
                ResolveContext(context, requirement, bearer);
                return;
            }
            else
            {
                bool isUserStoryTeller = false;

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
                        isUserStoryTeller = false;
                        context.Fail();
                    }
                    else if (userGuildDetails.roles.Contains(_storyTellerRoleId))
                    {
                        isUserStoryTeller = true;
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    context.Fail();
                }

                _bearerIsStoryTeller.Add(new(bearer, isUserStoryTeller, DateTime.Now));
                return;
            }
        }

        private void ResolveContext(AuthorizationHandlerContext context, IsStoryTellerRequirement requirement, string bearer)
        {
            // If user has the rights
            if (_bearerIsStoryTeller.Find(b => b.Bearer == bearer).IsStoryTeller)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }


        private class BearerStoryTeller
        {
            public BearerStoryTeller(string bearer, bool isStoryTeller, DateTime datechecked)
            {
                Bearer = bearer;
                IsStoryTeller = isStoryTeller;
                DateChecked = datechecked;
            }

            public string Bearer { get; set; }
            public bool IsStoryTeller { get; set; }
            public DateTime DateChecked { get; set; }
        }

        private class UserGuildDetails
        {
            public string[] roles { get; set; }
            public DiscordUser user { get; set; }
        }

        private class DiscordUser
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
    }
}
