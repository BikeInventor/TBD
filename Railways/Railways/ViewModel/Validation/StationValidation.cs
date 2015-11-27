using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Railways.ViewModel.Validation
{
    public class StationValidation:  ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            //string stationName;
            int stationInt;
            if (Int32.TryParse(value.ToString(), out stationInt))
            {
                return new ValidationResult(false, "Название станции не может содержать числа"); 
            }
            return new ValidationResult(true, null);
            
            //throw new NotImplementedException();
        }
    }
}
