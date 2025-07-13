using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MAMS.Services
{
    public class SriLankanNICAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var nic = value as string;

            if (string.IsNullOrWhiteSpace(nic))
                return false;

            // Old format: 9 digits + V or X (e.g., 931234567V)
            if (Regex.IsMatch(nic, @"^\d{9}[vVxX]$"))
                return true;

            // New format: 12 digits (e.g., 200012345678)
            if (Regex.IsMatch(nic, @"^\d{12}$"))
                return true;

            return false;
        }
    }
}
