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
    /// Interaction logic for RegistrationControlClient.xaml
    /// </summary>
    public partial class RegistrationControlClient : UserControl
    {
        public RegistrationControlClient()
        {
            InitializeComponent();
        }

        private void ReturnToMainPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new StartupWindowClient();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }

        private void RegistrationButton1_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
