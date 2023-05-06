using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class RoleEntities
    {
        public RoleEntities(Models.RoleModule roleModule)
        {
            this.Id = roleModule.Role.RoleId;
            this.Name = roleModule.Role.Name;
            this.CharacterType = roleModule.Role.CharacterType;
            this.Alignment = roleModule.Role.DefaultAlignment;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public CharacterType CharacterType { get; set; }
        public Alignment Alignment { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesWon { get; set; }
    }
}
