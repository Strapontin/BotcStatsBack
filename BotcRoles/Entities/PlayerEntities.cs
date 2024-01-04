using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BotcRoles.Entities
{
    public class PlayerEntities
    {
        public PlayerEntities(Models.Player player)
        {
            if (player == null)
                return;

            this.Id = player.PlayerId;
            this.Name = player.Name;
            this.Pseudo = player.Pseudo;

            if (player.PlayerRoleGames == null)
                return;

            this.NbGamesPlayed = player.PlayerRoleGames.Count;

            var gamesGood = player.PlayerRoleGames.Where(prg => prg.FinalAlignment == Enums.Alignment.Good);
            var gamesEvil = player.PlayerRoleGames.Where(prg => prg.FinalAlignment == Enums.Alignment.Evil);

            this.NbGamesGood = gamesGood.Count();
            this.NbGamesEvil = gamesEvil.Count();

            if (player.PlayerRoleGames.Any(prg => prg.Game == null))
            {
                return;
            }

            this.NbGamesWon = player.PlayerRoleGames.Count(prg => prg.Game.WinningAlignment == prg.FinalAlignment);
            this.NbGamesLost = player.PlayerRoleGames.Count(prg => prg.Game.WinningAlignment != prg.FinalAlignment);

            this.NbGamesGoodWon = gamesGood.Count(prg => prg.Game.WinningAlignment == Enums.Alignment.Good);
            this.NbGamesEvilWon = gamesEvil.Count(prg => prg.Game.WinningAlignment == Enums.Alignment.Evil);

            this.TimesPlayedRole = player.PlayerRoleGames
                //.Where(prg => prg.PlayerId == this.Id)
                //.Include(prg => prg.Role).ToList()
                .GroupBy(prg => prg.Role)
                .Select(r => new RoleEntities(r.Key, player.PlayerRoleGames))
                .OrderByDescending(re => re.TimesPlayedByPlayer)
                .ThenByDescending(re => re.TimesWonByPlayer)
                .ThenBy(re => re.CharacterType)
                .ThenBy(re => re.Name)
                .ToList();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Pseudo { get; set; }
        public int NbGamesPlayed { get; set; }

        public int NbGamesGood { get; set; }
        public int NbGamesEvil { get; set; }

        public int NbGamesWon { get; set; }
        public int NbGamesLost { get; set; }

        public int NbGamesGoodWon { get; set; }
        public int NbGamesEvilWon { get; set; }


        public List<RoleEntities> TimesPlayedRole { get; set; }
    }
}
