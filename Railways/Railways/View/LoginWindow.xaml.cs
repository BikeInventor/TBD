﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
          //  AdminFunctions.RegisterEmployee("Иванов И.И.", "123456", "0");
            InitializeComponent();
        }

        private void TryLogin(object sender, RoutedEventArgs e)
        {
            LoginWindowController.Login(idTextBox.Text, passwordBox.Password);
        }
    }
}
