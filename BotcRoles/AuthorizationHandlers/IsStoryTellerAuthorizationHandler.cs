using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace BotcRoles.AuthorizationHandlers
{
    public class IsStoryTellerRequirement : IAuthorizationRequirement
    {
        public IsStoryTellerRequirement() { }
    }


    public class IsStoryTellerAuthorizationHandler : AuthorizationHandler<IsStoryTellerRequirement>
    {
        const string _tbaServerId = "765137571608920074";

        const string _storyTellerRoleId = "797739056406069279";
        const string _staffTBARoleId = "895968259201982484";
        const string _neoConteurRoleId = "1082696028404318370";

        //string[] authorizedRolesId = new string[] { _storyTellerRoleId };
        string[] authorizedRolesId = new string[] { _storyTellerRoleId, _staffTBARoleId, _neoConteurRoleId };

        List<BearerStoryTeller> _bearerIsStoryTeller = new();

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStoryTellerRequirement requirement)
        {
            string? bearer = (context.Resource as DefaultHttpContext).Request?.Headers[HeaderNames.Authorization].FirstOrDefault();

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
                // If user has the rights
                if (_bearerIsStoryTeller.Find(b => b.Bearer == bearer).IsStoryTeller)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
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
                    // If we're on the recette, the user doesn't have to be a storyteller
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Recette" ||
                        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        context.Succeed(requirement);
                        _bearerIsStoryTeller.Add(new(bearer, true, DateTime.Now));
                        return;
                    }

                    var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"Serializing User Data : '{contentStream}'");
                    var userGuildDetails = JsonSerializer.Deserialize<UserGuildDetails>(contentStream);

                    if (userGuildDetails == null || userGuildDetails.user == null)
                    {
                        isUserStoryTeller = false;
                        context.Fail();
                    }
                    //else if (userGuildDetails.roles.Contains(_storyTellerRoleId))
                    else if (userGuildDetails.roles.Any(r => authorizedRolesId.Contains(r)))
                    {
                        isUserStoryTeller = true;
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
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
            public int? accent_color { get; set; }
            public string? global_name { get; set; }
            public string? avatar_decoration { get; set; }
            public string? display_name { get; set; }
            public string? banner_color { get; set; }
        }
    }
}