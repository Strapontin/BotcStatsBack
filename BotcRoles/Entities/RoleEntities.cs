using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class RoleEntities
    {
        public RoleEntities(Models.ModelContext db, Models.Role role)
        {
            if (role == null)
                return;

            this.Id = role.RoleId;
            this.Name = role.Name;
            this.CharacterType = role.CharacterType;
            this.Alignment = role.DefaultAlignment;

            this.TimesPlayed = db.PlayerRoleGames.Count(prg => prg.RoleId == this.Id);
            this.TimesWon = db.PlayerRoleGames.Where(prg => prg.RoleId == this.Id && prg.Game.WinningAlignment == prg.Role.DefaultAlignment).Count();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public CharacterType CharacterType { get; set; }
        public Alignment Alignment { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesWon { get; set; }
    }
}
