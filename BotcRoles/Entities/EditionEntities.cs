using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BotcRoles.Entities
{
    public class EditionEntities
    {
        public EditionEntities(Models.ModelContext db, Models.Edition edition)
        {
            this.Id = edition.EditionId;
            this.Name = edition.Name;
            this.Roles = edition.RolesEdition?.Select(rm => new RoleEntities(db, rm.Role)).ToList();


            this.TimesPlayed = db.Editions
                .Include(m => m.Games)
                .First(m => m.EditionId == this.Id)
                .Games.Count;

            this.TimesGoodWon = db.Editions
                .Include(m => m.Games)
                .First(m => m.EditionId == this.Id)
                .Games.Where(g => g.WinningAlignment == Enums.Alignment.Good)
                .Count();
        }


        public long Id { get; set; }
        public string Name { get; set; }
        public List<RoleEntities> Roles { get; set; }

        public int TimesPlayed { get; set; }
        public int TimesGoodWon { get; set; }
    }
}
