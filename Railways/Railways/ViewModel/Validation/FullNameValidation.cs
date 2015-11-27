using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class FullNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (!Regex.IsMatch(value.ToString(), @"^[а-яА-Я\s]") && !String.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "Неверный формат ФИО!\n");
            }
            return new ValidationResult(true, null);
        }
    }
}
