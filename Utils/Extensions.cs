using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TripickServer.Utils
{
    public static class Extensions
    {
        #region User

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string ToCleanUsername(this string s)
        {
            string cleanString = Regex.Replace(Regex.Replace(s.Trim(), "[ -]+|_+", "_"), "^_*|_*$|[^a-zA-ZÀ-ÿ0-9_]*", "");
            return cleanString.Substring(0, Math.Min(cleanString.Length, 20));
        }

        #endregion
    }
}
