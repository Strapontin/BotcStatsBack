using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly ModelContext _db;

        public PlayerController(ILogger<PlayerController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Player> Get()
        {
            var players = _db.Players;
            return players;
        }

        [HttpPost]
        [Route("{playerName}")]
        public IActionResult Post(string playerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerName))
                {
                    return BadRequest($"Le nom du joueur est vide.");
                }

                if (_db.Players.Any(p => p.Name == playerName))
                {
                    return BadRequest($"Un joueur avec le nom '{playerName}' existe déjà.");
                }

                _db.Add(new Player(playerName));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}