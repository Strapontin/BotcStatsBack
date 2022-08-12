using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    public enum Alignment
    {
        Townsfolk = 0,
        Outsider = 1,
        Minion = 2,
        Demon = 3,
        Traveller = 4
    }

    public class Role
    {
        public long RoleId { get; set; }
        public string Name { get; set; }
        public Alignment Alignment { get; set; }
    }
}
