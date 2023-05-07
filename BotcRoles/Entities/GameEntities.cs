using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class GameEntities
    {
        public GameEntities(Models.ModelContext db, Models.Game game)
        {
            this.Id = game.GameId;
            this.Module = new ModuleEntities(db, game.Module);
            this.StoryTeller = new PlayerEntities(db, game.StoryTeller);
            this.CreationDate = game.CreationDate;
            this.Notes = game.Notes;
            this.WinningAlignment = game.WinningAlignment;

            this.PlayerRoles = game.PlayerRoleGames
                .Select(prg => new PlayerRoleEntities(db, prg))
                .ToList();
        }

        public long Id { get; set; }
        public ModuleEntities Module { get; set; }
        public PlayerEntities StoryTeller { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
        public Alignment WinningAlignment { get; set; }

        public List<PlayerRoleEntities> PlayerRoles { get; set; }
    }
}
