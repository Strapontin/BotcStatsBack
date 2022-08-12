namespace BotcRoles.Entities
{
    public enum Alignment { Townsfolk, Outsider, Minion, Demon, Traveller }

    public class Role
    {
        public long RoleId { get; set; }
        public string Name { get; set; }
        public Alignment Alignment { get; set; }
    }
}