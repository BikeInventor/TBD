using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class TimeSpanValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!Regex.IsMatch(value.ToString(), @"^\d+(:[0-5]\d){1,2}$") && !String.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Неправильный формат времени!");
            }
            return new ValidationResult(true, null);
        }
    }
}
