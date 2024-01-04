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

                this.TimesPlayedByPlayer = prgFilterRole.Count(rip => rip.RoleId == this.Id);
                this.TimesLostByPlayer = prgFilterRole.Count(rip => rip.RoleId == this.Id && rip.Game.WinningAlignment != rip.FinalAlignment);
                this.TimesWonByPlayer = prgFilterRole.Count(rip => rip.RoleId == this.Id && rip.Game.WinningAlignment == rip.FinalAlignment);
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
    }
}
