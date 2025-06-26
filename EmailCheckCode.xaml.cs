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
            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                this.Resources.MergedDictionaries.Add(dict);
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
