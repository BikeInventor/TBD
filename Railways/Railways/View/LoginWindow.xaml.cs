using System.Windows;
using Railways.Logic;
using Railways.Model;
using MahApps.Metro.Controls;
namespace Railways
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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

        private void DoTestActions(object sender, RoutedEventArgs e)
        {
            Model.Test.ModelTestingClass.AddEmployee();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

    }
}
