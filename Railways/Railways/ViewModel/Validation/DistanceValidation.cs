using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class DistanceValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(true, null);

            int distance;
            if (Int32.TryParse(value.ToString(), out distance))
            {
                if (distance >= 0 && distance < Int32.MaxValue)
                    return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "Неверное значение расстояния!");

        }
    }
}
