using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BotcRoles.Entities
{
    public class UpdateHistoryEntities
    {
        public UpdateHistoryEntities(Models.UpdateHistory updateHistory)
        {
            if (updateHistory == null)
                return;

            this.Id = updateHistory.UpdateHistoryId;

            this.PlayerId = updateHistory.PlayerId;
            this.RoleId = updateHistory.RoleId;
            this.EditionId = updateHistory.EditionId;
            this.GameId = updateHistory.GameId;
            this.GameDraftId = updateHistory.GameDraftId;

            this.Date = updateHistory.Date;
            this.Text = updateHistory.UpdateDetails;
        }

        public long Id { get; set; }

        public long? PlayerId { get; set; }
        public long? RoleId { get; set; }
        public long? EditionId { get; set; }
        public long? GameId { get; set; }
        public long? GameDraftId { get; set; }

        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
