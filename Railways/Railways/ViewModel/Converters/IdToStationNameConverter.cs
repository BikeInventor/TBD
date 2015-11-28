using Railways.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Railways.ViewModel.Converters
{
    public class IdToStationNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stationId = int.Parse(value.ToString());

            return ContextKeeper.Stations.Where(s => s.Id == stationId).Select(s => s.StationName).First();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stationName = value.ToString();

            return ContextKeeper.Stations.Where(s => s.StationName == stationName).Select(s => s.Id).First();
        }
    }
}