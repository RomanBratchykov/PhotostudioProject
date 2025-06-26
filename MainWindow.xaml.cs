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
        public string currentEmail { get; set; } = string.Empty;

        public MainWindow(string currentUser, string email)
            {
            InitializeComponent();
            currentEmail = email;
            this.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Clear();

            // Завантажуємо тему з Settings
            var themePath = Properties.Settings.Default.CurrentTheme;

            try
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri(themePath, UriKind.RelativeOrAbsolute)
                };

                // Додаємо в Application глобально
                Application.Current.Resources.MergedDictionaries.Add(themeDict);

                // Додаємо в локальні ресурси вікна (якщо потрібно)
                this.Resources.MergedDictionaries.Add(themeDict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося завантажити тему: {ex.Message}");
            }
            if (currentUser == "client")
            {
                
                var clientControl = new MainWindowControlClient(email);
                MainWindowContent.Content = clientControl;

            }
            else if (currentUser == "photographer")
            {
                var workerControl = new MainWindowWorkerControl(email);
                MainWindowContent.Content = workerControl;
                this.Title = "Фотостудія Emerald(Фотограф)";
            }
            else if (currentUser == "admin")
            {
                var adminControl = new MainWindowAdminControl(email);
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