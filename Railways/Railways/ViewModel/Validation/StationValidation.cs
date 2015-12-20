using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class StationValidation:  ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!Regex.IsMatch(value.ToString(), @"^[А-Яа-я0-9 -]+$") && !String.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Неверный формат названия станции!"); 
            }
            return new ValidationResult(true, null);
        }
    }
}
