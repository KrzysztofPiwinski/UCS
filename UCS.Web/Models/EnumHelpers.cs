using UCS.Db.Entities;

namespace UCS.Web.Models
{
    public static class EnumHelpers
    {
        public static string GetDescription(this PermissionEnum permission)
        {
            switch (permission)
            {
                case PermissionEnum.USERS: return "Zarządzanie pracownikami";
                case PermissionEnum.STUDENTS: return "Zarządzanie studentami";
                case PermissionEnum.CATEGORIES: return "Zarządzanie kategoriami wiadomości e-mail";
                case PermissionEnum.SEND_EMAIL: return "Wysyłanie wiadomości e-mail";
                case PermissionEnum.VIEW_ALL_EMAIL: return "Wyświetlanie wszystkich wiadomości e-mail";
                case PermissionEnum.CHAT: return "Wysyłanie wiadomości chat";
                default: return string.Empty;
            }
        }
    }
}