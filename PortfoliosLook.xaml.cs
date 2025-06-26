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
using System.Windows.Navigation;

using System.IO;

namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for PortfoliosLook.xaml
    /// </summary>
    public partial class PortfoliosLook : UserControl
    {
        private Clients? currentClient { get; set; }
        public string email { get; set; }

        public PortfoliosLook(string email)
        {
            InitializeComponent();
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
            this.email = email;
            using (var db = new PhotoStudioDbContext())
            {
                currentClient = db.Clients.FirstOrDefault(c => c.EmailOfClient == email);
                if (currentClient == null)
                {
                    MessageBox.Show("Клієнт не знайдений.");
                    return;
                }
            }
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var images = Directory.GetFiles(folderPath, "*.jpg");
            PhotoGallery.ItemsSource = images;
        }

        private void GetBackToClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new MainWindowControlClient(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = clientControl;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
