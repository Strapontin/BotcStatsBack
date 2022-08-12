using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    public class Player
    {
        public long PlayerId { get; set; }
        public string Name { get; set; }
    }
}
