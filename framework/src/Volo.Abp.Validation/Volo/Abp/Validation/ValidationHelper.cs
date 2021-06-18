using System.Text.RegularExpressions;

namespace Volo.Abp.Validation
{
    public class ValidationHelper
    {
        // Taken from W3C as an alternative to the RFC5322 specification: https://html.spec.whatwg.org/#valid-e-mail-address
        // The RFC5322 regex can be found here: https://emailregex.com/
        public static string EmailRegEx { get; set; } = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        public static bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            return Regex.IsMatch(email, EmailRegEx, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}