using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Railways.ViewModel.Converters
{
    public class IntToUserRoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (int.Parse(value.ToString()))
            {
                case 0: return "Кассир";
                case 1: return "Администратор";
                case 2: return "Гл. администратор";
            }
            return "Кассир";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                switch ((String)value)
                {
                    case "Кассир": return 0;
                    case "Администратор": return 1;
                    case "Гл. администратор": return 2;
                }
            }
            return 0;
        }
    }
}
