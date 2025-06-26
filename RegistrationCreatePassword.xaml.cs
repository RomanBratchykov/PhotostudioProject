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
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;


namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for RegistrationCreatePassword.xaml
    /// </summary>
    public partial class RegistrationCreatePassword : UserControl
    {
        private readonly RegistrationState _state;
        public RegistrationCreatePassword(RegistrationState state)
        {
            InitializeComponent();
            _state = state;
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

        private void GetBackToCode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new EmailCheckCode(_state);
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }

        private void AcceptPassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBoxRegistration.Password) ||
                string.IsNullOrWhiteSpace(PasswordBoxRegistrationCompare.Password))
            {
                NullErrorTextPasswords.Visibility = Visibility.Visible;
            }
            else if (PasswordBoxRegistration.Password != PasswordBoxRegistrationCompare.Password)
            {
                ErrorTextBlockPasswordsNotSame.Visibility = Visibility.Visible;
            }
            else
            {
                string password = PasswordBoxRegistration.Password.Trim();
                var client = new Clients
                {
                    EmailOfClient = _state.Email,
                    NameOfClient = _state.Name,
                    PhoneNumberClient = _state.Phone,
                    PasswordClient = password
                };
                using (var db = new PhotoStudioDbContext())
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                }
                MessageBox.Show("Реєстрація успішна! Тепер ви можете увійти до системи.");
                var clientControl = new StartupWindowClient();
                ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
                ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Visibility = Visibility.Visible;
            }
        }
    }
}
