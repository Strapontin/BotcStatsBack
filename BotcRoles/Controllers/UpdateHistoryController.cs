using BotcRoles.Entities;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [Authorize(Policy = "IsStoryteller")]
    [ApiController]
    [Route("[controller]")]
    public class UpdateHistoryController : ControllerBase
    {
        private readonly ILogger<UpdateHistoryController> _logger;
        private readonly ModelContext _db;

        public UpdateHistoryController(ILogger<UpdateHistoryController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<UpdateHistoryEntities>> GetAllHistory()
        {
            var updateHistory = _db.UpdateHistories
                .Select(uh => new UpdateHistoryEntities(uh))
                .ToList();

            return updateHistory;
        }
    }
}
