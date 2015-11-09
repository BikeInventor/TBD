using System.Windows;
using Railways.Logic;

namespace Railways
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TryLogin(object sender, RoutedEventArgs e)
        {
            LoginWindowController.Login(idTextBox.Text, passwordBox.Password);

            if (LoginWindowController.IsLoggedIn) 
                this.Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoginWindowController.IsLoggedIn)
            {
                var scheduleWindow = new ScheduleWindow();
                scheduleWindow.Show();
            }
        }
    }
}
