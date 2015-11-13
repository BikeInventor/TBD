using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Railways.DebugTools
{
    /// <summary>
    /// Логика взаимодействия для EntriesAdjustionWindow.xaml
    /// </summary>
    public partial class EntriesAdjustionWindow : Window
    {
        public EntriesAdjustionWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Railways.DebugTools.RailwayDataDataSet railwayDataDataSet = ((Railways.DebugTools.RailwayDataDataSet)(this.FindResource("railwayDataDataSet")));
            // Загрузить данные в таблицу Client. Можно изменить этот код как требуется.
            Railways.DebugTools.RailwayDataDataSetTableAdapters.ClientTableAdapter railwayDataDataSetClientTableAdapter = new Railways.DebugTools.RailwayDataDataSetTableAdapters.ClientTableAdapter();
            railwayDataDataSetClientTableAdapter.Fill(railwayDataDataSet.Client);
            System.Windows.Data.CollectionViewSource clientViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientViewSource")));
            clientViewSource.View.MoveCurrentToFirst();
            // Загрузить данные в таблицу Seat. Можно изменить этот код как требуется.
            Railways.DebugTools.RailwayDataDataSetTableAdapters.SeatTableAdapter railwayDataDataSetSeatTableAdapter = new Railways.DebugTools.RailwayDataDataSetTableAdapters.SeatTableAdapter();
            railwayDataDataSetSeatTableAdapter.Fill(railwayDataDataSet.Seat);
            System.Windows.Data.CollectionViewSource seatViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("seatViewSource")));
            seatViewSource.View.MoveCurrentToFirst();
        }
    }
}
