using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class RoleEntities
    {
        public RoleEntities() { }

        public RoleEntities(Models.ModelContext db, Models.Role role) : this(db, role, null) { }

        public RoleEntities(Models.ModelContext db, Models.Role role, List<Models.PlayerRoleGame> rolesIdPlayed)
        {
            if (role == null)
                return;

            this.Id = role.RoleId;
            this.Name = role.Name;
            this.CharacterType = role.CharacterType;
            this.Alignment = role.DefaultAlignment;

            if (rolesIdPlayed != null && !rolesIdPlayed.Any(rip => rip.Game == null))
            {
                this.TimesPlayedByPlayer = rolesIdPlayed.Count(rip => rip.RoleId == this.Id);
                this.TimesLostByPlayer = rolesIdPlayed.Count(rip => rip.RoleId == this.Id && rip.Game.WinningAlignment != rip.FinalAlignment);
                this.TimesWonByPlayer = rolesIdPlayed.Count(rip => rip.RoleId == this.Id && rip.Game.WinningAlignment == rip.FinalAlignment);
            }

            this.TimesPlayedTotal = db.PlayerRoleGames.Count(prg => prg.RoleId == this.Id);
            this.TimesWonTotal = db.PlayerRoleGames.Where(prg => prg.RoleId == this.Id && prg.Game.WinningAlignment == prg.FinalAlignment).Count();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public CharacterType CharacterType { get; set; }
        public Alignment Alignment { get; set; }

        public int TimesPlayedByPlayer { get; set; }
        public int TimesWonByPlayer { get; set; }
        public int TimesLostByPlayer { get; set; }


        public int TimesPlayedTotal { get; set; }
        public int TimesWonTotal { get; set; }
    }
}
