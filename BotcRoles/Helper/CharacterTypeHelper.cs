using BotcRoles.Enums;

namespace BotcRoles.Helper
{
    public static class CharacterTypeHelper
    {
        public static string ToText(this CharacterType characterType)
        {
            return characterType == CharacterType.Townsfolk ? "Villageois" :
                characterType == CharacterType.Outsider ? "Etranger" :
                characterType == CharacterType.Minion ? "Sbire" :
                characterType == CharacterType.Demon ? "Démon" :
                characterType == CharacterType.Traveller ? "Voyageur" :
                characterType == CharacterType.Fabled ? "Légendaire" :
                "";
        }
    }
}



