namespace BotcRoles.Entities
{
    public class ModuleEntities
    {
        public ModuleEntities(Models.Module module)
        {
            this.Id = module.ModuleId;
            this.Name = module.Name;
            this.Roles = module.RolesModule.Select(rm => new RoleEntities(rm)).ToList();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public List<RoleEntities> Roles { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesGoodWon { get; set; }
    }
}
