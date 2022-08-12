using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private ModelContext db;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
            db = new ModelContext();
        }

        [HttpGet(Name = "GetPlayers")]
        public IEnumerable<Player> Get()
        {
            var players = db.Players;

            return players;
        }

        [HttpPost(Name = "PostPlayer")]
        public void Post(string playerName)
        {
            db.Add(new Player { Name = playerName });
            db.SaveChanges();
        }
    }
}