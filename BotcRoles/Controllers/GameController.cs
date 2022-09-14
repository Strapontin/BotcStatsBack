using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;
        private readonly ModelContext _db;

        public GameController(ILogger<GameController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Game> Get()
        {
            var roles = _db.Games
                .Include(g => g.Module)
                .Include(g => g.StoryTeller)
                .ToList();

            return roles;
        }

        [HttpGet]
        [Route("{gameId}")]
        public Game? Get(long gameId)
        {
            var game = _db.Games
                .Where(g => g.GameId == gameId)
                .Include(g => g.Module)
                .Include(g => g.StoryTeller)
                .FirstOrDefault();

            return game;
        }

        [HttpPost]
        [Route("{moduleId}/{storyTellerId}")]
        public IActionResult Post(long moduleId, long storyTellerId)
        {
            try
            {
                var module = _db.Modules.Find(moduleId);
                var storyTeller = _db.Players.Find(storyTellerId);

                if (module == null)
                {
                    return BadRequest($"Le module avec l'id '{module}' n'a pas été trouvé.");
                }

                if (storyTeller == null)
                {
                    return BadRequest($"Le joueur avec l'id '{storyTeller}' n'a pas été trouvé.");
                }

                _db.Add(new Game(module, storyTeller));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{gameId}")]
        public IActionResult Delete(long gameId)
        {
            try
            {
                _db.Games.Remove(_db.Games.First(g => g.GameId == gameId));
                _db.SaveChanges();
                return Ok();
            }
            catch { return BadRequest(); }
        }
    }
}