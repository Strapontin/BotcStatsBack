using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BotcRoles.Entities
{
    public class EditionEntities
    {
        public EditionEntities(Models.Edition edition, List<Models.Game> gamesWithThisEdition = null)
        {
            this.Id = edition.EditionId;
            this.Name = edition.Name;
            this.Roles = edition.RolesEdition?.Select(rm => new RoleEntities(rm.Role)).ToList();

            if (gamesWithThisEdition == null) { return; }

            this.TimesPlayed = gamesWithThisEdition.Count;
            this.TimesGoodWon = gamesWithThisEdition.Count(g => g.WinningAlignment == Enums.Alignment.Good);
            this.TimesEvilWon = gamesWithThisEdition.Count(g => g.WinningAlignment == Enums.Alignment.Evil);
        }


        public long Id { get; set; }
        public string Name { get; set; }
        public List<RoleEntities> Roles { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesGoodWon { get; set; }
        public int TimesEvilWon { get; set; }
    }
}
