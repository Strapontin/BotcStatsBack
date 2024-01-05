using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class RoleEntities
    {
        public RoleEntities() { }


        public RoleEntities(Models.Role role, List<Models.PlayerRoleGame> prgFilterRole = null)
        {
            if (role == null)
                return;

            this.Id = role.RoleId;
            this.Name = role.Name;
            this.CharacterType = role.CharacterType;

            if (prgFilterRole != null)
            {
                this.TimesPlayedTotal = prgFilterRole.Count;
                this.TimesWonTotal = prgFilterRole.Count(prg => prg.Game.WinningAlignment == prg.FinalAlignment);
                this.TimesLostTotal = prgFilterRole.Count(prg => prg.Game.WinningAlignment != prg.FinalAlignment);

                this.TimesPlayedByPlayer = prgFilterRole.Count(prg => prg.RoleId == this.Id);
                this.TimesLostByPlayer = prgFilterRole.Count(prg => prg.RoleId == this.Id && prg.Game.WinningAlignment != prg.FinalAlignment);
                this.TimesWonByPlayer = prgFilterRole.Count(prg => prg.RoleId == this.Id && prg.Game.WinningAlignment == prg.FinalAlignment);

                var playersWhoPlayedRole = prgFilterRole.GroupBy(prg => prg.Player);
                this.PlayersWhoPlayedRole = new();

                foreach (var player in playersWhoPlayedRole)
                {
                    player.Key.PlayerRoleGames = null;

                    this.PlayersWhoPlayedRole.Add(new PlayersWhoPlayedRole()
                    {
                        Player = new PlayerEntities(player.Key),
                        TimesPlayedRole = player.Count(),
                        TimesWon = player.Count(prg => prg.Game.WinningAlignment == prg.FinalAlignment),
                        TimesLost = player.Count(prg => prg.Game.WinningAlignment != prg.FinalAlignment),
                    });
                }
            }
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public CharacterType CharacterType { get; set; }

        public int TimesPlayedByPlayer { get; set; }
        public int TimesWonByPlayer { get; set; }
        public int TimesLostByPlayer { get; set; }

        public int TimesPlayedTotal { get; set; }
        public int TimesWonTotal { get; set; }
        public int TimesLostTotal { get; set; }

        public List<PlayersWhoPlayedRole> PlayersWhoPlayedRole { get; set; }
    }

    public class PlayersWhoPlayedRole
    {
        public PlayerEntities Player { get; set; }
        public int TimesPlayedRole { get; set; }
        public int TimesWon { get; set; }
        public int TimesLost { get; set; }
    }
}
