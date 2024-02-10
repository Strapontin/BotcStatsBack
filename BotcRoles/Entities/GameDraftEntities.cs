using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class GameDraftEntities
    {
        public GameDraftEntities(Models.GameDraft gameDraft)
        {
            this.Id = gameDraft.GameDraftId;

            if (gameDraft.Edition != null)
            {
                this.Edition = new EditionEntities(gameDraft.Edition, null);
            }

            this.Storyteller = new PlayerEntities(gameDraft.Storyteller);
            this.DatePlayed = gameDraft.DatePlayed;
            this.Notes = gameDraft.Notes;
        }

        public long Id { get; set; }
        public EditionEntities Edition { get; set; }
        public PlayerEntities Storyteller { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Notes { get; set; }
    }
}
