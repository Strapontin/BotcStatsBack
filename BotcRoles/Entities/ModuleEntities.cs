using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BotcRoles.Entities
{
    public class ModuleEntities
    {
        public ModuleEntities(Models.ModelContext db, Models.Module module)
        {
            this.Id = module.ModuleId;
            this.Name = module.Name;
            this.Roles = module.RolesModule?.Select(rm => new RoleEntities(db, rm.Role)).ToList();


            this.TimesPlayed = db.Modules
                .Include(m => m.Games)
                .First(m => m.ModuleId == this.Id)
                .Games.Count;

            this.TimesGoodWon = db.Modules
                .Include(m => m.Games)
                .First(m => m.ModuleId == this.Id)
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
