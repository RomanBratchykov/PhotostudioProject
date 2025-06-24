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
using System.Windows.Shapes;

namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for StartupWindowClient.xaml
    /// </summary>
    public partial class StartupWindowClient : UserControl
    {
        public StartupWindowClient()
        {
            InitializeComponent();
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
