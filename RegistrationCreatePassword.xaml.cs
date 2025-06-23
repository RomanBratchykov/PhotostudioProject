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
    /// Interaction logic for RegistrationCreatePassword.xaml
    /// </summary>
    public partial class RegistrationCreatePassword : UserControl
    {
        public RegistrationCreatePassword()
        {
            InitializeComponent();
        }

        private void GetBackToCode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new EmailCheckCode();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }

        private void AcceptPassword_Click(object sender, RoutedEventArgs e)
        {
            //    if (string.IsNullOrWhiteSpace(PasswordBoxRegistration.Password) ||
            //        string.IsNullOrWhiteSpace(PasswordBoxRegistrationCompare.Password))
            //    {
            //        NullErrorTextPasswords.Visibility = Visibility.Visible;
            //    }
            //    else if (PasswordBoxRegistration.Password != PasswordBoxRegistrationCompare.Password)
            //    {
            //        ErrorTextBlockPasswordsNotSame.Visibility = Visibility.Visible;
            //    }
            //    else
            //    {

            //        var mainWindow = new MainWindow("client");
            //        mainWindow.Show();
            //        ((StartupWindow_Login_)Application.Current.MainWindow).Close();
            //        Application.Current.MainWindow = mainWindow;
            //    }
        }
    }
}
