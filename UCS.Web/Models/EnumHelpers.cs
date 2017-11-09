using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    return "Zmiana hasła";
                case 1:
                    return "Zarządzanie pracownikami";
                default:
                    return "";
            }
        }

    }
}