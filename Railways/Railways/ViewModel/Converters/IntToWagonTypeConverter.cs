using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Railways.ViewModel.Converters
{
    public class IntToWagonTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (int.Parse(value.ToString()))
            {
                case 0: return "Плацкарт";
                case 1: return "Купе";
                case 2: return "Люкс";
            }
            return "Плацкарт";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String)
            {
                switch ((String)value)
                {
                    case "Плацкарт": return 0;
                    case "Купе": return 1;
                    case "Люкс": return 2;
                }
            }
            return 0;
        }

    }
}
