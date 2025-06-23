using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

namespace PhotostudioProject
{

    public partial class MainWindow : Window
        {
        public string CurrentUser { get; set; } = string.Empty;
        public MainWindow(string currentUser)
            {
            InitializeComponent();
            CurrentUser = currentUser;
            if (currentUser == "client")
            {
                var clientControl = new MainWindowControlClient();
                MainWindowContent.Content = clientControl;

            }
            else if (currentUser == "photographer")
            {
                var workerControl = new MainWindowWorkerControl();
                MainWindowContent.Content = workerControl;
                this.Title = "Фотостудія Emerald(Фотограф)";
            }
            else if (currentUser == "admin")
            {
                var adminControl = new MainWindowAdminControl();
                MainWindowContent.Content = adminControl;
                this.Title = "Фотостудія Emerald(Адмін)";
            }
            else
            {
                throw new ArgumentException("Invalid user type");
            }
        }

        }
 }