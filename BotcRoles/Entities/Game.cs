using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public ModuleEntities Module { get; set; }
        public Player StoryTeller { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
        public Alignment WinningAlignment { get; set; }
    }
}
