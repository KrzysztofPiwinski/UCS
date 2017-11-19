using System;
using UCS.Db.Entities;

namespace UCS.Web.Models
{
    public class EnumHelpers
    {
        public static string GetPermissionDescription(string value)
        {
            int p = (int)Enum.Parse(typeof(PermissionEnum), value);

            switch (p)
            {
                case 0:
                    return "Zarządzanie pracownikami";
                case 1:
                    return "Zarządzanie studentami";
                case 2:
                    return "Zarządzanie kategoriami wiadomości e-mail";
                case 3:
                    return "Wysyłanie wiadomości e-mail";
                default:
                    return "";
            }
        }

    }
}