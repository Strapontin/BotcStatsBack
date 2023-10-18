using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class GameEntities
    {
        public GameEntities(Models.ModelContext db, Models.Game game)
        {
            this.Id = game.GameId;

            if (game.Edition != null)
            {
                this.Edition = new EditionEntities(db, game.Edition);
            }

            this.StoryTeller = new PlayerEntities(db, game.StoryTeller);

            this.DatePlayed = game.DatePlayed;
            this.Notes = game.Notes;
            this.WinningAlignment = game.WinningAlignment;

            if (game.PlayerRoleGames != null)
            {
                this.PlayerRoles = game.PlayerRoleGames
                    .Select(prg => new PlayerRoleEntities(db, prg))
                    .OrderBy(prg => prg.Role.Alignment)
                    .ToList();
            }

            if (game.DemonBluffs != null)
            {
                this.DemonBluffs = game.DemonBluffs
                    .Select(demonBluff => new RoleEntities(db, demonBluff.Role))
                    .OrderBy(role => role.Alignment)
                    .ToList();
            }
        }

        public long Id { get; set; }
        public EditionEntities Edition { get; set; }
        public PlayerEntities StoryTeller { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Notes { get; set; }
        public Alignment WinningAlignment { get; set; }

        public List<PlayerRoleEntities> PlayerRoles { get; set; }
        public List<RoleEntities> DemonBluffs { get; set; }
    }
}
