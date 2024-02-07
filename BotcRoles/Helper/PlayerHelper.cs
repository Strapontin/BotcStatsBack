using BotcRoles.Models;

namespace BotcRoles.Helper
{
    public static class PlayerHelper
    {
        public static string GetFullName(this Player player)
        {
            var result = player.Name + (!string.IsNullOrWhiteSpace(player.Pseudo) ?
            $" ({player.Pseudo})" : "");

            return result;
        }
    }
}



