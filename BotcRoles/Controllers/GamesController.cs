using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ModelContext _db;

        public GamesController(ILogger<GamesController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GameEntities>> GetGames()
        {
            var games = _db.Games
                .Include(g => g.Edition)
                .Include(g => g.StoryTeller)
                .Include(g => g.PlayerRoleGames)
                .OrderByDescending(g => g.DatePlayed)
                .Select(g => new GameEntities(_db, g))
                .ToList();

            return games;
        }

        [HttpGet]
        [Route("{gameId}")]
        public ActionResult<GameEntities> GetGameById(long gameId)
        {
            var game = _db.Games
                .Where(g => g.GameId == gameId)
                .Include(g => g.Edition)
                .Include(g => g.StoryTeller)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .Select(g => new GameEntities(_db, g))
                .FirstOrDefault();

            return game == null ? NotFound() : game;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateGame([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["editionId"].ToString(), out long editionId))
                {
                    return BadRequest($"Une partie doit avoir un module.");
                }
                var edition = _db.Editions.FirstOrDefault(e => e.EditionId == editionId);
                if (edition == null)
                {
                    return BadRequest($"Le module avec l'id '{editionId}' n'a pas été trouvé");
                }

                if (!long.TryParse(data["storyTellerId"].ToString(), out long storyTellerId))
                {
                    return BadRequest($"Une partie doit avoir un conteur.");
                }
                var storyTeller = _db.Players.FirstOrDefault(e => e.PlayerId == storyTellerId);
                if (storyTeller == null)
                {
                    return BadRequest($"Le module avec l'id '{storyTellerId}' n'a pas été trouvé");
                }

                if (!DateTime.TryParse(data["datePlayed"].ToString(), out DateTime datePlayed))
                {
                    return BadRequest($"La date n'est pas correcte.");
                }

                var notes = data["notes"].ToString();

                if (!int.TryParse(data["winningAlignment"]?.ToString(), out int alignmentInt) || !Enum.IsDefined(typeof(Alignment), alignmentInt))
                {
                    return BadRequest($"Une erreur a été rencontrée avec le paramètre 'winningAlignment'.");
                }
                Alignment winningAlignment = (Alignment)alignmentInt;


                // Try to convert to Role object from database to ensure it exists
                Dictionary<long, long>? playersIdRolesId = data["playersIdRolesId"]?.ToObject<Dictionary<long, long>>();
                if (playersIdRolesId == null)
                {
                    return BadRequest($"Aucun joueur-rôle trouvé.");
                }

                List<PlayerRoleGame> playersRoles = new();
                foreach (var playerIdRoleId in playersIdRolesId)
                {
                    Player? playerDb = _db.Players.FirstOrDefault(p => p.PlayerId == playerIdRoleId.Key);
                    if (playerDb == null)
                    {
                        return BadRequest($"Le joueur avec l'id '{playerIdRoleId.Key}' n'a pas été trouvé.");
                    }

                    Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == playerIdRoleId.Value);
                    if (roleDb == null)
                    {
                        return BadRequest($"Le rôle avec l'id '{playerIdRoleId.Value}' n'a pas été trouvé.");
                    }

                    playersRoles.Add(new PlayerRoleGame(playerDb, roleDb, null));
                }

                DateTime dateCreated = DateTime.Now;

                Game game = new (edition, storyTeller, dateCreated, datePlayed, notes, winningAlignment);
                _db.Add(game);
                _db.SaveChanges();

                // Get game db
                var gameDb = _db.Games.First(g => g.DateCreated == dateCreated);

                playersRoles = playersRoles.Select(pr => new PlayerRoleGame(pr.Player, pr.Role, gameDb)).ToList();
                _db.AddRange(playersRoles);
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        //[HttpDelete]
        //[Route("{gameId}")]
        //public IActionResult Delete(long gameId)
        //{
        //    try
        //    {
        //        _db.Games.Remove(_db.Games.First(g => g.GameId == gameId));
        //        _db.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpPost]
        //[Route("{gameId}")]
        //public IActionResult AddPlayerInGame(long gameId, [FromQuery] long? playerId)
        //{
        //    try
        //    {
        //        if (playerId == null)
        //        {
        //            return BadRequest($"Le paramètre playerId n'est pas spécifié");
        //        }

        //        var game = _db.Games
        //            .Where(g => g.GameId == gameId)
        //            .Include(g => g.PlayerRoleGames)
        //            .FirstOrDefault();

        //        var player = _db.Players.Find(playerId);

        //        if (game == null)
        //        {
        //            return BadRequest($"La partie avec l'id '{gameId}' n'a pas été trouvé.");
        //        }

        //        if (player == null)
        //        {
        //            return BadRequest($"Le joueur avec l'id '{playerId}' n'a pas été trouvé.");
        //        }

        //        if (game.PlayerRoleGames.Any(prg => prg.PlayerId == playerId))
        //        {
        //            return BadRequest($"Le joueur est déjà dans la partie.");
        //        }

        //        _db.Add(new PlayerRoleGame(player, game));
        //        _db.SaveChanges();

        //        return Created("", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpPut]
        //[Route("{gameId}/players/{playerId}")]
        //public IActionResult ChangePlayerRoleAndAlignmentInGame(long gameId, long playerId, [FromQuery] long roleId, [FromQuery] Alignment finalAlignment)
        //{
        //    try
        //    {
        //        var playerRole = _db.PlayerRoleGames.Where(pr => pr.PlayerId == playerId &&
        //                                                    pr.GameId == gameId)
        //            .Include(pr => pr.Game.Edition.RolesEdition)
        //            .FirstOrDefault();

        //        if (playerRole == null)
        //        {
        //            return BadRequest($"Le PlayerRole n'a pas été trouvé. L'utilisateur a-t-il bien été ajouté à la partie ?");
        //        }

        //        if (!playerRole.Game.Edition.RolesEdition.Any(rm => rm.RoleId == roleId))
        //        {
        //            return BadRequest($"Le rôle que vous essayez d'assigner n'appartient pas aux roles assign�sEditiondule de cette game.");
        //        }

        //        playerRole.RoleId = roleId;
        //        playerRole.FinalAlignment = finalAlignment;
        //        _db.SaveChanges();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpGet]
        //[Route("{gameId}/players")]
        //public IEnumerable<Player>? GetPlayers(long gameId)
        //{
        //    try
        //    {
        //        var game = _db.Games.Find(gameId);

        //        var result = game?.PlayerRoleGames.Select(prg => prg.Player);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return null;
        //    }
        //}

        //[HttpGet]
        //[Route("{gameId}/players/{playerId}")]
        //public IEnumerable<PlayerRoleGame>? GetPlayerRoleFromGame(long gameId, long playerId)
        //{
        //    try
        //    {
        //        var game = _db.Games.Find(gameId);
        //        var player = _db.Players.Find(playerId);


        //        var result = player?.PlayerRoleGames.Where(prg => prg.GameId == gameId);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return null;
        //    }
        //}
    }
}