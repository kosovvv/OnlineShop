using System.Text.RegularExpressions;

namespace OnlineShop.Common.Entities
{
    public static class UserConstants
    {
        public const string PasswordRegex = "(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$";
    }
}
