using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class IdValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(true, null);

            int id;
            if (Int32.TryParse(value.ToString(), out id))
            {
                if (id > 0 && id < Int32.MaxValue)
                    return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "Неверное значение id!\nДопускается только числовое значение.");

        }
    }
}
