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
using ZstdSharp.Unsafe;

namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for EmailCheckCode.xaml
    /// </summary>
    public partial class EmailCheckCode : UserControl
    {
        private readonly RegistrationState _state;
        public EmailCheckCode(RegistrationState state)
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

        private void GetBackToRegistration_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new RegistrationControlClient();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }

        private void AcceptCodeEmail_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckCodeFromEmailBox.Text))
            {
                NullErrorTextCheck.Visibility = Visibility.Visible;
            }
            else
            {
                if (CheckCodeFromEmailBox.Text == _state.Code)
                {
                    var passwordStep = new RegistrationCreatePassword(_state);
                    ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = passwordStep;
                }
                else
                {
                    ErrorTextBlockLoginCheck.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
