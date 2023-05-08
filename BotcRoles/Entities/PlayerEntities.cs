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

            if (player.PlayerRoleGames == null)
                return;

            this.NbGamesPlayed = player.PlayerRoleGames.Count;

            this.NbGamesGood = player.PlayerRoleGames.Count(prg => prg.FinalAlignment == Enums.Alignment.Good);
            this.NbGamesEvil = player.PlayerRoleGames.Count(prg => prg.FinalAlignment == Enums.Alignment.Evil);

            if (player.PlayerRoleGames.Any(prg => prg.Game == null))
            {
                return;
            }

            this.NbGamesWon = player.PlayerRoleGames.Count(prg => prg.Game.WinningAlignment == prg.FinalAlignment);
            this.NbGamesLost = player.PlayerRoleGames.Count(prg => prg.Game.WinningAlignment != prg.FinalAlignment);

            this.TimesPlayedRole = db.PlayerRoleGames
                .Where(prg => prg.PlayerId == this.Id)
                .Include(prg => prg.Role).ToList()
                .GroupBy(prg => prg.Role)
                .Select(r => new RoleEntities(db, r.Key, player.PlayerRoleGames))
                .OrderByDescending(re => re.TimesPlayedByPlayer)
                .ThenByDescending(re => re.TimesWonByPlayer)
                .ThenBy(re => re.Name)
                .ToList();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int NbGamesPlayed { get; set; }

        public int NbGamesGood { get; set; }
        public int NbGamesEvil { get; set; }

        public int NbGamesWon { get; set; }
        public int NbGamesLost { get; set; }


        public List<RoleEntities> TimesPlayedRole { get; set; }
    }
}
