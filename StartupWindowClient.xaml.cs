using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for StartupWindowClient.xaml
    /// </summary>
    public partial class StartupWindowClient : UserControl
    {
        private MediaPlayer _player = new MediaPlayer();
        private async void PlaySoundForTwoSeconds()
        {
            _player.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds/soft-piano.mp3")));
            _player.Play();

            await Task.Delay(3000);
            _player.Stop();
        }
        public StartupWindowClient()
        {
            InitializeComponent();
            PlaySoundForTwoSeconds();
          
            // Очищаємо словники
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
        }
        private void RegistrationTextStartup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registrationWindow = new RegistrationControlClient();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = registrationWindow;
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Visibility = Visibility.Visible;
        }

        private void EnterAsWorker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var workerWindow = new StartupWindowWorker();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = workerWindow;
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Visibility = Visibility.Visible;
        }

        private void LoginButtonStartUpWindow_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBoxLogin.Text) || string.IsNullOrWhiteSpace(PasswordBoxLogin.Password))
            {
                NullErrorText.Visibility = Visibility.Visible;
                return;
            }


            string? email = EmailTextBoxLogin.Text.Trim();
            string? password = PasswordBoxLogin.Password.Trim();
            using (var db = new PhotoStudioDbContext())
            {
                var client = db.Clients
                               .FirstOrDefault(c => c.EmailOfClient == email && c.PasswordClient == password);
                if (client != null)
                {
                    MessageBox.Show($"Вітаємо, клієнте {client.NameOfClient}!");
                    var window = new MainWindow("client", client.EmailOfClient);
                    window.Show();
                    ((StartupWindow_Login_)Application.Current.MainWindow).Close();
                    Application.Current.MainWindow = window;
                    ErrorTextBlockLogin.Visibility = Visibility.Collapsed;
                    NullErrorText.Visibility = Visibility.Collapsed;
                    return;
                }
                ErrorTextBlockLogin.Visibility = Visibility.Visible;
                return;
            }
        }
    }
}
