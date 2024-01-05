using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BotcRoles.Entities
{
    public class EditionEntities
    {
        public EditionEntities(Models.Edition edition, List<Models.Game> gamesWithThisEdition = null)
        {
            this.Id = edition.EditionId;
            this.Name = edition.Name;
            this.Roles = edition.RolesEdition?.Select(rm => new RoleEntities(rm.Role)).ToList();

            if (gamesWithThisEdition == null) { return; }

            this.TimesPlayed = gamesWithThisEdition.Count;
            this.TimesGoodWon = gamesWithThisEdition.Count(g => g.WinningAlignment == Enums.Alignment.Good);
            this.TimesEvilWon = gamesWithThisEdition.Count(g => g.WinningAlignment == Enums.Alignment.Evil);

            var playersWhoPlayedEdition = gamesWithThisEdition
                .SelectMany(g => g.PlayerRoleGames)
                .GroupBy(prg => prg.Player);
            this.PlayersWhoPlayedEdition = new();

            foreach (var player in playersWhoPlayedEdition)
            {
                player.Key.PlayerRoleGames = null;

                this.PlayersWhoPlayedEdition.Add(new Entities.PlayersWhoPlayedEdition()
                {
                    Player = new PlayerEntities(player.Key),
                    TimesPlayedEdition = player.Count(),
                    TimesWon = player.Count(prg => prg.Game.WinningAlignment == prg.FinalAlignment),
                    TimesLost = player.Count(prg => prg.Game.WinningAlignment != prg.FinalAlignment),
                });
            }
        }


        public long Id { get; set; }
        public string Name { get; set; }
        public List<RoleEntities> Roles { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesGoodWon { get; set; }
        public int TimesEvilWon { get; set; }

        public List<PlayersWhoPlayedEdition> PlayersWhoPlayedEdition { get; set; }
    }

    public class PlayersWhoPlayedEdition
    {
        public PlayerEntities Player { get; set; }
        public int TimesPlayedEdition { get; set; }
        public int TimesWon { get; set; }
        public int TimesLost { get; set; }
    }
}
