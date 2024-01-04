using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class GameEntities
    {
        public GameEntities(Models.Game game, List<Models.Game> allGames = null)
        {
            this.Id = game.GameId;

            if (game.Edition != null)
            {
                this.Edition = new EditionEntities(game.Edition, allGames?.Where(g => g.EditionId == game.EditionId).ToList());
            }

            this.Storyteller = new PlayerEntities(game.Storyteller);

            this.DatePlayed = game.DatePlayed;
            this.Notes = game.Notes;
            this.WinningAlignment = game.WinningAlignment;

            if (game.PlayerRoleGames != null)
            {
                this.PlayerRoles = game.PlayerRoleGames
                    .Select(prg => new PlayerRoleEntities(prg))
                    .ToList();
            }

            if (game.DemonBluffs != null)
            {
                this.DemonBluffs = game.DemonBluffs
                    .Select(demonBluff => new RoleEntities(demonBluff.Role))
                    .OrderBy(role => role.CharacterType)
                    .ToList();
            }
        }

        public long Id { get; set; }
        public EditionEntities Edition { get; set; }
        public PlayerEntities Storyteller { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Notes { get; set; }
        public Alignment WinningAlignment { get; set; }

        public List<PlayerRoleEntities> PlayerRoles { get; set; }
        public List<RoleEntities> DemonBluffs { get; set; }
    }
}
