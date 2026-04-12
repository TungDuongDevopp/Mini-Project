
using System.Text.RegularExpressions;
using System.Net.Mail;
namespace UI.Helper;

internal static class Validator
{

    public static bool IsValidEmail(string email)
        
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            var addr = new MailAddress(email.Trim());
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }
    public static bool IsValidPhone(string phone) { 
     return Regex.IsMatch(phone, @"^(0|\+84)\d{9}$");
    }

    public static bool IsValidMoney(decimal money)
    {
        return money >= 0;
    }
    
}
