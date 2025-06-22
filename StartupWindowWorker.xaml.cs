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
    /// Interaction logic for StartupWindowWorker.xaml
    /// </summary>
    public partial class StartupWindowWorker : UserControl
    {
        public StartupWindowWorker()
        {
            InitializeComponent();
        }

        private void EnterAsClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new StartupWindowClient();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }

        private void LoginButtonStartUpWindowWorker_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBoxLoginWorker.Text) || string.IsNullOrWhiteSpace(PasswordBoxLoginWorker.Password))
            {
                NullErrorTextWorker.Visibility = Visibility.Visible;
                return;
            }
            else
            {

                var window = new MainWindow("photographer");
                window.Show();
                ((StartupWindow_Login_)Application.Current.MainWindow).Close();
                NullErrorTextWorker.Visibility = Visibility.Collapsed;
                return;
            }
        }
    }
}
