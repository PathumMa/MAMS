using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MAMS.Validators // Use your project namespace
{
    public class SriLankanPhoneAttribute : ValidationAttribute
    {
        public SriLankanPhoneAttribute()
        {
            ErrorMessage = "Please enter a valid Sri Lankan mobile number (e.g., 0771234567)";
        }

        public override bool IsValid(object value)
        {
            var phone = value as string;

            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Accepts valid Sri Lankan mobile numbers (starting with 070, 071, 072, 074, 075, 076, 077, 078)
            return Regex.IsMatch(phone, @"^07[01245678]\d{7}$");
        }
    }
}
