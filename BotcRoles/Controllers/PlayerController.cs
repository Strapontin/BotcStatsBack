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

        [HttpGet]
        [Route("")]
        public IEnumerable<Player> Get()
        {
            var players = db.Players;
            return players;
        }

        [HttpPost]
        [Route("{playerName}")]
        public IActionResult Post(string playerName)
        {
            try
            {
                if (db.Players.Any(p => p.Name == playerName))
                {
                    return BadRequest($"Le nom de joueur '{playerName}' existe déjà.");
                }

                db.Add(new Player(playerName));
                db.SaveChanges();

                return Created("Player", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}