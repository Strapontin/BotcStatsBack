using BotcRoles.AuthorizationHandlers;
using BotcRoles.Enums;
using BotcRoles.Helper;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;

namespace BotcRoles.Misc
{
    public enum UpdateHistoryAction
    {
        Create, Update, Delete
    }

    public enum UpdateHistoryType
    {
        Player, Role, Edition, Game
    }

    public class ObjectUpdateHistory
    {
        public ObjectUpdateHistory(
            Player? newPlayer = null,
            Player? oldPlayer = null,
            Role? newRole = null,
            Role? oldRole = null,
            Edition? newEdition = null,
            Edition? oldEdition = null,
            Game? newGame = null,
            Game? oldGame = null)
        {
            NewPlayer = newPlayer;
            OldPlayer = oldPlayer;
            NewRole = newRole;
            OldRole = oldRole;
            NewEdition = newEdition;
            OldEdition = oldEdition;
            NewGame = newGame;
            OldGame = oldGame;
        }

        public Player? NewPlayer { get; set; }
        public Player? OldPlayer { get; set; }
        public Role? NewRole { get; set; }
        public Role? OldRole { get; set; }
        public Edition? NewEdition { get; set; }
        public Edition? OldEdition { get; set; }
        public Game? NewGame { get; set; }
        public Game? OldGame { get; set; }
    }

    public static class UpdateHistory
    {
        public static void AddUpdateHistory(ModelContext db,
            UpdateHistoryAction updateHistoryAction,
            UpdateHistoryType updateHistoryType,
            IAuthorizationHandler authorizationHandler,
            IHeaderDictionary header,
            ObjectUpdateHistory objectUpdateHistory
            )
        {
            var bearer = header["Authorization"][0];
            var username = ((IsStorytellerAuthorizationHandler)authorizationHandler).GetDiscordUsernameByBearer(bearer);

            string? textToUpdate = null;
            var objectType = "";
            var objectName = "";
            var isHidden = false;

            switch (updateHistoryType)
            {
                case UpdateHistoryType.Player:
                    objectType = "du joueur";
                    objectName = PlayerHelper.GetPlayerFullName(objectUpdateHistory.NewPlayer);
                    isHidden = objectUpdateHistory.NewPlayer.IsHidden;
                    break;
                case UpdateHistoryType.Role:
                    objectType = "du rôle";
                    objectName = objectUpdateHistory.NewRole.Name;
                    isHidden = objectUpdateHistory.NewRole.IsHidden;
                    break;
                case UpdateHistoryType.Edition:
                    objectType = "du module";
                    objectName = objectUpdateHistory.NewEdition.Name;
                    isHidden = objectUpdateHistory.NewEdition.IsHidden;
                    break;
                case UpdateHistoryType.Game:
                    objectType = "de la partie du";
                    objectName = objectUpdateHistory.NewGame.DatePlayed.ToString("dd/MM/yyyy") + " contée par " + PlayerHelper.GetPlayerFullName(objectUpdateHistory.NewGame.Storyteller);
                    isHidden = objectUpdateHistory.NewGame.IsHidden;
                    break;
            }

            switch (updateHistoryAction)
            {
                case UpdateHistoryAction.Create:
                    textToUpdate = $"[{username}] - Ajout {objectType} '{objectName}'.";
                    break;
                case UpdateHistoryAction.Delete:
                    textToUpdate = $"[{username}] - Suppression {objectType} '{objectName}'{(isHidden ? " isHidden" : "")}.";
                    break;
                case UpdateHistoryAction.Update:
                    textToUpdate = GetUpdateHistoryDifferences(updateHistoryType, objectUpdateHistory, $"[{username}] - Modification(s) {objectType} '{objectName}' :");
                    break;
            }

            if (textToUpdate != null)
            {
                Models.UpdateHistory? updateHistory = new(textToUpdate)
                {
                    Date = DateTime.Now,
                    PlayerId = objectUpdateHistory.NewPlayer?.PlayerId,
                    RoleId = objectUpdateHistory.NewRole?.RoleId,
                    EditionId = objectUpdateHistory.NewEdition?.EditionId,
                    GameId = objectUpdateHistory.NewGame?.GameId,
                };

                db.Add(updateHistory);
                db.SaveChanges();
            }
        }

        private static string GetUpdateHistoryDifferences(UpdateHistoryType updateHistoryType, ObjectUpdateHistory objectUpdateHistory, string baseMsg)
        {
            var result = baseMsg;

            switch (updateHistoryType)
            {
                case UpdateHistoryType.Player:
                    result += GetTextIfDifference(objectUpdateHistory.OldPlayer.Name, objectUpdateHistory.NewPlayer.Name, "\nNom changé de '{0}' en '{1}'");
                    result += GetTextIfDifference(objectUpdateHistory.OldPlayer.Pseudo, objectUpdateHistory.NewPlayer.Pseudo, "\nPseudo changé de '{0}' en '{1}'");
                    break;

                case UpdateHistoryType.Role:
                    result += GetTextIfDifference(objectUpdateHistory.OldRole.Name, objectUpdateHistory.NewRole.Name, "\nNom changé de '{0}' en '{1}'");
                    result += GetTextIfDifference(objectUpdateHistory.OldRole.CharacterType, objectUpdateHistory.NewRole.CharacterType, "\nNom changé de '{0}' en '{1}'");
                    break;

                case UpdateHistoryType.Edition:
                    result += GetTextIfDifference(objectUpdateHistory.OldEdition.Name, objectUpdateHistory.NewEdition.Name, "\nNom changé de '{0}' en '{1}'");

                    var rolesDeleted = new List<RoleEdition>(objectUpdateHistory.OldEdition.RolesEdition);
                    rolesDeleted.RemoveAll(r => objectUpdateHistory.NewEdition.RolesEdition.Any(o => o.RoleId == r.RoleId));
                    if (rolesDeleted.Any())
                    {
                        result += $"\nRôles supprimé(s) : {string.Join(", ", rolesDeleted.Select(r => r.Role.Name))}";
                    }
                    var rolesAdded = new List<RoleEdition>(objectUpdateHistory.NewEdition.RolesEdition);
                    rolesAdded.RemoveAll(r => objectUpdateHistory.OldEdition.RolesEdition.Any(o => o.RoleId == r.RoleId));
                    if (rolesAdded.Any())
                    {
                        result += $"\nRôles ajouté(s) : {string.Join(", ", rolesAdded.Select(r => r.Role.Name))}";
                    }
                    break;

                case UpdateHistoryType.Game:
                    result += GetTextIfDifference(objectUpdateHistory.OldGame.Edition.Name, objectUpdateHistory.NewGame.Edition.Name, 
                        "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(PlayerHelper.GetPlayerFullName(objectUpdateHistory.OldGame.Storyteller), PlayerHelper.GetPlayerFullName(objectUpdateHistory.NewGame.Storyteller),
                        "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(objectUpdateHistory.OldGame.DatePlayed.ToString("dd/MM/yyyy"), objectUpdateHistory.NewGame.DatePlayed.ToString("dd/MM/yyyy"),
                        "\nDate changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(objectUpdateHistory.OldGame.Notes, objectUpdateHistory.NewGame.Notes,
                        "\nNotes changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(objectUpdateHistory.OldGame.WinningAlignment, objectUpdateHistory.NewGame.WinningAlignment,
                        "\nAlignement gagnant changé de '{0}' vers '{1}'");

                    var prgDeleted = new List<PlayerRoleGame>(objectUpdateHistory.OldGame.PlayerRoleGames);
                    prgDeleted.RemoveAll(prgA => objectUpdateHistory.NewGame.PlayerRoleGames.Any(prg => prg.PlayerId == prgA.PlayerId && prg.RoleId == prgA.RoleId));
                    if (prgDeleted.Any())
                    {
                        result += $"\nJoueur/rôle supprimé(s) : {string.Join(", ", prgDeleted.Select(prgA => $"{PlayerHelper.GetPlayerFullName(prgA.Player)}/{prgA.Role.Name}"))}";
                    }
                    var prgAdded = new List<PlayerRoleGame>(objectUpdateHistory.NewGame.PlayerRoleGames);
                    prgAdded.RemoveAll(prgA => objectUpdateHistory.OldGame.PlayerRoleGames.Any(prg => prg.PlayerId == prgA.PlayerId && prg.RoleId == prgA.RoleId));
                    if (prgAdded.Any())
                    {
                        result += $"\nJoueur/rôle ajouté(s) : {string.Join(", ", prgAdded.Select(prgA => $"{PlayerHelper.GetPlayerFullName(prgA.Player)}/{prgA.Role.Name}"))}";
                    }

                    var demonBluffDeleted = new List<DemonBluff>(objectUpdateHistory.OldGame.DemonBluffs);
                    demonBluffDeleted.RemoveAll(prgA => objectUpdateHistory.NewGame.DemonBluffs.Any(db => db.RoleId == prgA.RoleId));
                    if (demonBluffDeleted.Any())
                    {
                        result += $"\nDemon bluff supprimé(s) : {string.Join(", ", demonBluffDeleted.Select(db => db.Role.Name))}";
                    }
                    var demonBluffAdded = new List<DemonBluff>(objectUpdateHistory.NewGame.DemonBluffs);
                    demonBluffAdded.RemoveAll(prgA => objectUpdateHistory.OldGame.DemonBluffs.Any(db => db.RoleId == prgA.RoleId));
                    if (demonBluffAdded.Any())
                    {
                        result += $"\nDemon bluff ajouté(s) : {string.Join(", ", demonBluffAdded.Select(db => db.Role.Name))}";
                    }
                    break;
            }

            return result;
        }

        private static string GetTextIfDifference(string oldObject, string newObject, string textToFormat)
        {
            if (oldObject != newObject)
            {
                return string.Format(textToFormat, oldObject, newObject);
            }
            return "";
        }

        private static string GetTextIfDifference(CharacterType oldObject, CharacterType newObject, string textToFormat)
        {
            if (oldObject != newObject)
            {
                return string.Format(textToFormat, CharacterTypeHelper.GetCharacterTypeName(oldObject), CharacterTypeHelper.GetCharacterTypeName(newObject));
            }
            return "";
        }

        private static string GetTextIfDifference(Alignment oldObject, Alignment newObject, string textToFormat)
        {
            if (oldObject != newObject)
            {
                return string.Format(textToFormat, AlignmentHelper.GetAlignmentName(oldObject), AlignmentHelper.GetAlignmentName(newObject));
            }
            return "";
        }
    }
}
