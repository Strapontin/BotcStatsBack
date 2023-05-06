namespace BotcRoles.Entities
{
    public class Player
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public int NbGamesPlayed { get; set; }
        public Dictionary<RoleEntities, int> TimesPlayedRole { get; set; }
        public Dictionary<ModuleEntities, int> TimesPlayedModule { get; set; }
    }
}
