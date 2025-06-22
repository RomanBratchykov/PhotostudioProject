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
    /// Interaction logic for EmailCheckCode.xaml
    /// </summary>
    public partial class EmailCheckCode : UserControl
    {
        public EmailCheckCode()
        {
            InitializeComponent();
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
                NullErrorTextCheck.Visibility = Visibility.Collapsed;
                ErrorTextBlockLoginCheck.Visibility = Visibility.Collapsed;
                var createPassword = new RegistrationCreatePassword();
                ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = createPassword;
            }
        }
    }
}
