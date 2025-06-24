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
    /// Interaction logic for MainWindowControlClient.xaml
    /// </summary>
    public partial class MainWindowControlClient : UserControl
    {
        private Clients? currentClient { get; set; }
        public MainWindowControlClient(string email)
        {
            InitializeComponent();
            using (var db = new PhotoStudioDbContext())
            {
                currentClient = db.Clients.FirstOrDefault(c => c.EmailOfClient == email);
                if (currentClient == null)
                {
                    MessageBox.Show("Клієнт не знайдений.");
                    return;
                }
            }
        }

        private void ViewProfileClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProfileClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButtonClient_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HelpButtonClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteToSupportButtonPH_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteToSupport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetHelpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetBackTologin_Click(object sender, RoutedEventArgs e)
        {
            var loginWin = new StartupWindow_Login_();
            loginWin.Show();

            foreach (Window win in Application.Current.Windows)
            {
                if (win != loginWin)
                    win.Close();
            }

            Application.Current.MainWindow = loginWin;
        }
    }
}
