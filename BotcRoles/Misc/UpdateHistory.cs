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
        Player, Role, Edition, Game, GameDraft
    }

    public class ObjectUpdateHistory
    {
        public ObjectUpdateHistory(
            object newObject,
            object oldObject = null)
        {
            NewObject = newObject;
            OldObject = oldObject;
        }

        public object NewObject { get; set; }
        public object OldObject { get; set; }
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

            long? id = null;
            string? textToUpdate = null;
            var objectType = "";
            var objectName = "";
            var isHidden = false;

            switch (updateHistoryType)
            {
                case UpdateHistoryType.Player:
                    Player player = (Player)objectUpdateHistory.NewObject;
                    id = player.PlayerId;
                    objectType = "du joueur";
                    objectName = player.GetFullName();
                    isHidden = player.IsHidden;
                    break;
                case UpdateHistoryType.Role:
                    Role role = (Role)objectUpdateHistory.NewObject;
                    id = role.RoleId;
                    objectType = "du rôle";
                    objectName = role.Name;
                    isHidden = role.IsHidden;
                    break;
                case UpdateHistoryType.Edition:
                    Edition edition = (Edition)objectUpdateHistory.NewObject;
                    id = edition.EditionId;
                    objectType = "du module";
                    objectName = edition.Name;
                    isHidden = edition.IsHidden;
                    break;
                case UpdateHistoryType.Game:
                    Game game = (Game)objectUpdateHistory.NewObject;
                    id = game.GameId;
                    objectType = "de la partie du";
                    objectName = game.DatePlayed.ToString("dd/MM/yyyy") + " contée par " + game.Storyteller.GetFullName();
                    isHidden = game.IsHidden;
                    break;
                case UpdateHistoryType.GameDraft:
                    GameDraft gameDraft = (GameDraft)objectUpdateHistory.NewObject;
                    id = gameDraft.GameDraftId;
                    objectType = "du rappel de partie du";
                    objectName = gameDraft.DatePlayed.ToString("dd/MM/yyyy") + " contée par " + gameDraft.Storyteller.GetFullName();
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
                    PlayerId = updateHistoryType == UpdateHistoryType.Player ? id : null,
                    RoleId = updateHistoryType == UpdateHistoryType.Role ? id : null,
                    EditionId = updateHistoryType == UpdateHistoryType.Edition ? id : null,
                    GameId = updateHistoryType == UpdateHistoryType.Game ? id : null,
                    GameDraftId = updateHistoryType == UpdateHistoryType.GameDraft ? id : null,
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
                    var oldPlayer = (Player)objectUpdateHistory.OldObject;
                    var newPlayer = (Player)objectUpdateHistory.NewObject;

                    result += GetTextIfDifference(oldPlayer.Name, newPlayer.Name, "\nNom changé de '{0}' en '{1}'");
                    result += GetTextIfDifference(oldPlayer.Pseudo, newPlayer.Pseudo, "\nPseudo changé de '{0}' en '{1}'");
                    break;

                case UpdateHistoryType.Role:
                    var oldRole = (Role)objectUpdateHistory.OldObject;
                    var newRole = (Role)objectUpdateHistory.NewObject;

                    result += GetTextIfDifference(oldRole.Name, newRole.Name, "\nNom changé de '{0}' en '{1}'");
                    result += GetTextIfDifference(oldRole.CharacterType, newRole.CharacterType, "\nNom changé de '{0}' en '{1}'");
                    break;

                case UpdateHistoryType.Edition:
                    var oldEdition = (Edition)objectUpdateHistory.OldObject;
                    var newEdition = (Edition)objectUpdateHistory.NewObject;

                    result += GetTextIfDifference(oldEdition.Name, newEdition.Name, "\nNom changé de '{0}' en '{1}'");

                    var rolesDeleted = new List<RoleEdition>(oldEdition.RolesEdition);
                    rolesDeleted.RemoveAll(r => newEdition.RolesEdition.Any(o => o.Equals(r)));
                    if (rolesDeleted.Any())
                    {
                        result += $"\nRôles supprimé(s) : \n\t{string.Join("\n\t", rolesDeleted.Select(r => r.Role.Name))}";
                    }
                    var rolesAdded = new List<RoleEdition>(newEdition.RolesEdition);
                    rolesAdded.RemoveAll(r => oldEdition.RolesEdition.Any(o => o.Equals(r)));
                    if (rolesAdded.Any())
                    {
                        result += $"\nRôles ajouté(s) : \n\t{string.Join("\n\t", rolesAdded.Select(r => r.Role.Name))}";
                    }
                    break;

                case UpdateHistoryType.Game:
                    var oldGame = (Game)objectUpdateHistory.OldObject;
                    var newGame = (Game)objectUpdateHistory.NewObject;

                    result += GetTextIfDifference(oldGame.Edition.Name, newGame.Edition.Name, "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGame.Storyteller.GetFullName(), newGame.Storyteller.GetFullName(), "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGame.DatePlayed.ToString("dd/MM/yyyy"), newGame.DatePlayed.ToString("dd/MM/yyyy"), "\nDate changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGame.Notes, newGame.Notes, "\nNotes changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGame.WinningAlignment, newGame.WinningAlignment, "\nAlignement gagnant changé de '{0}' vers '{1}'");

                    var prgDeleted = new List<PlayerRoleGame>(oldGame.PlayerRoleGames);
                    prgDeleted.RemoveAll(prgA => newGame.PlayerRoleGames.Any(prg => prg.Equals(prgA)));
                    if (prgDeleted.Any())
                    {
                        result += $"\nJoueur/rôle supprimé(s) : \n\t{string.Join("\n\t", prgDeleted.Select(prgA => $"{prgA.Player.GetFullName()}/{prgA.Role.Name} ({prgA.FinalAlignment.ToText()})"))}";
                    }
                    var prgAdded = new List<PlayerRoleGame>(newGame.PlayerRoleGames);
                    prgAdded.RemoveAll(prgA => oldGame.PlayerRoleGames.Any(prg => prg.Equals(prgA)));
                    if (prgAdded.Any())
                    {
                        result += $"\nJoueur/rôle ajouté(s) : \n\t{string.Join("\n\t", prgAdded.Select(prgA => $"{prgA.Player.GetFullName()}/{prgA.Role.Name} ({prgA.FinalAlignment.ToText()})"))}";
                    }

                    var demonBluffDeleted = new List<DemonBluff>(oldGame.DemonBluffs);
                    demonBluffDeleted.RemoveAll(prgA => newGame.DemonBluffs.Any(db => db.Equals(prgA)));
                    if (demonBluffDeleted.Any())
                    {
                        result += $"\nDemon bluff supprimé(s) : \n\t{string.Join("\n\t", demonBluffDeleted.Select(db => db.Role.Name))}";
                    }
                    var demonBluffAdded = new List<DemonBluff>(newGame.DemonBluffs);
                    demonBluffAdded.RemoveAll(prgA => oldGame.DemonBluffs.Any(db => db.Equals(prgA)));
                    if (demonBluffAdded.Any())
                    {
                        result += $"\nDemon bluff ajouté(s) : \n\t{string.Join("\n\t", demonBluffAdded.Select(db => db.Role.Name))}";
                    }
                    break;

                case UpdateHistoryType.GameDraft:
                    var oldGameDraft = (GameDraft)objectUpdateHistory.OldObject;
                    var newGameDraft = (GameDraft)objectUpdateHistory.NewObject;

                    result += GetTextIfDifference(oldGameDraft.Edition.Name, newGameDraft.Edition.Name, "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGameDraft.Storyteller.GetFullName(), newGameDraft.Storyteller.GetFullName(), "\nModule changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGameDraft.DatePlayed.ToString("dd/MM/yyyy"), newGameDraft.DatePlayed.ToString("dd/MM/yyyy"), "\nDate changé de '{0}' vers '{1}'");
                    result += GetTextIfDifference(oldGameDraft.Notes, newGameDraft.Notes, "\nNotes changé de '{0}' vers '{1}'");
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
                return string.Format(textToFormat, oldObject.ToText(), newObject.ToText());
            }
            return "";
        }

        private static string GetTextIfDifference(Alignment oldObject, Alignment newObject, string textToFormat)
        {
            if (oldObject != newObject)
            {
                return string.Format(textToFormat, oldObject.ToText(), newObject.ToText());
            }
            return "";
        }
    }
}
