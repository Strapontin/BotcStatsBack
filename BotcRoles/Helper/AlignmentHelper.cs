using BotcRoles.Enums;

namespace BotcRoles.Helper
{
    public static class AlignmentHelper
    {
        public static string GetAlignmentName(Alignment alignment)
        {
            return alignment == Alignment.Good ? "Gentil" :
                alignment == Alignment.Evil ? "Maléfique" :
                "";
        }
    }
}



