using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.DataAccessLayer
{
    public class CurrentMisparIshi
    {
        private static HashSet<string> ADMINS = new HashSet<string> {
            "6073570", // Gal
            "5235136", // Omer
            "8159165", // Sean
        }; 

        public static string GetCurrentMisparIshi()
        {
            var userNameSplit = System.Web.HttpContext.Current.Request
                .LogonUserIdentity.Name.Split('\\');
            if (userNameSplit.Length == 0 || userNameSplit[1].Length < 2)
            {
                return "";
            }
            return userNameSplit[1].Substring(1);
        }

        public static bool IsAdmin()
        {
            return ADMINS.Contains(GetCurrentMisparIshi());
        }

        public static bool IsCurrentUser(string misparIshi)
        {
            return GetCurrentMisparIshi().Equals(misparIshi);
        }

        public static bool IsCurrentUserOrAdmin(string misparIshi)
        {
            return IsAdmin() || IsCurrentUser(misparIshi);
        }
    }
}