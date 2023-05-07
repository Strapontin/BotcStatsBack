using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BotcRoles.Entities
{
    public class PlayerEntities
    {
        public PlayerEntities(Models.ModelContext db, Models.Player player)
        {
            if (player == null)
                return;

            this.Id = player.PlayerId;
            this.Name = player.Name;
            this.NbGamesPlayed = player.NbGamesPlayed;

            this.TimesPlayedRole = db.PlayerRoleGames
                .Where(prg => prg.PlayerId == this.Id)
                .Include(prg => prg.Role).ToList()
                .GroupBy(prg => prg.Role)
                .Select(r => new RoleEntities(db, r.Key))
                .OrderByDescending(re => re.TimesPlayed)
                .ThenBy(re => re.Name)
                .ToList();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int NbGamesPlayed { get; set; }

        public List<RoleEntities> TimesPlayedRole { get; set; }
    }
}
