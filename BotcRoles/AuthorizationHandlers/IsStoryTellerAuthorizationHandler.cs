using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace BotcRoles.AuthorizationHandlers
{
    public class IsStorytellerRequirement : IAuthorizationRequirement
    {
        public IsStorytellerRequirement() { }
    }


    public class IsStorytellerAuthorizationHandler : AuthorizationHandler<IsStorytellerRequirement>
    {
        const string _tbaServerId = "765137571608920074";

        const string _storytellerRoleId = "797739056406069279";
        const string _staffTBARoleId = "895968259201982484";
        const string _neoConteurRoleId = "1082696028404318370";

        //string[] authorizedRolesId = new string[] { _storytellerRoleId };
        string[] authorizedRolesId = new string[] { _storytellerRoleId, _staffTBARoleId, _neoConteurRoleId };

        List<BearerStoryteller> _bearerIsStoryteller = new();

        public string GetDiscordUsernameByBearer(string bearer)
        {
            return _bearerIsStoryteller.FirstOrDefault(s => s.Bearer == bearer)?.Username;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStorytellerRequirement requirement)
        {
            string? bearer = (context.Resource as DefaultHttpContext).Request?.Headers[HeaderNames.Authorization].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(bearer))
            {
                context.Fail();
                return;
            }

            List<string> bearerExpired = new();
            foreach (var b in _bearerIsStoryteller)
            {
                if (DateTime.Now.Subtract(b.DateChecked).TotalMinutes >= 10)
                {
                    bearerExpired.Add(b.Bearer);
                }
            }
            _bearerIsStoryteller.RemoveAll(b => bearerExpired.Contains(b.Bearer));

            // If the user has already authenticated 
            if (_bearerIsStoryteller.Any(b => b.Bearer == bearer))
            {
                // If user has the rights
                if (_bearerIsStoryteller.Find(b => b.Bearer == bearer).IsStoryteller)
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
                bool isUserStoryteller = false;
                string? username = "";

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
                    Console.WriteLine($"Serializing User Data : '{contentStream}'");
                    var userGuildDetails = JsonSerializer.Deserialize<UserGuildDetails>(contentStream);
                    username = userGuildDetails.user.username;

                    // If we're on the recette, the user doesn't have to be a storyteller
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Recette" ||
                        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        context.Succeed(requirement);
                        _bearerIsStoryteller.Add(new(bearer, true, DateTime.Now, username));
                        return;
                    }

                    if (userGuildDetails == null || userGuildDetails.user == null)
                    {
                        isUserStoryteller = false;
                        context.Fail();
                    }
                    //else if (userGuildDetails.roles.Contains(_storytellerRoleId))
                    else if (userGuildDetails.roles.Any(r => authorizedRolesId.Contains(r)))
                    {
                        isUserStoryteller = true;
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

                _bearerIsStoryteller.Add(new(bearer, isUserStoryteller, DateTime.Now, username));
                return;
            }
        }


        private class BearerStoryteller
        {
            public BearerStoryteller(string bearer, bool isStoryteller, DateTime datechecked, string username)
            {
                Bearer = bearer;
                IsStoryteller = isStoryteller;
                DateChecked = datechecked;
                Username = username;
            }

            public string Bearer { get; set; }
            public bool IsStoryteller { get; set; }
            public DateTime DateChecked { get; set; }
            public string Username { get; set; }
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