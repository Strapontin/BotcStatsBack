﻿using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class GameEntities
    {
        public GameEntities(Models.ModelContext db, Models.Game game)
        {
            this.Id = game.GameId;
            this.Edition = new EditionEntities(db, game.Edition);
            this.StoryTeller = new PlayerEntities(db, game.StoryTeller);
            this.DatePlayed = game.DatePlayed;
            this.Notes = game.Notes;
            this.WinningAlignment = game.WinningAlignment;

            this.PlayerRoles = game.PlayerRoleGames
                .Select(prg => new PlayerRoleEntities(db, prg))
                .OrderBy(prg => prg.Role.Alignment)
                .ToList();
        }

        public long Id { get; set; }
        public EditionEntities Edition { get; set; }
        public PlayerEntities StoryTeller { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Notes { get; set; }
        public Alignment WinningAlignment { get; set; }

        public List<PlayerRoleEntities> PlayerRoles { get; set; }
    }
}
