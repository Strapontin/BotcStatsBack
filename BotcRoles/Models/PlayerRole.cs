using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    [Keyless]
    public class PlayerRole
    {
        //public int IdPlayer { get; set; }
        //public int IdRole { get; set; }

        public Player Player { get; set; }
        public Role Role { get; set; }
        public int Count { get; set; }
    }
}
