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

        private string email { get; set; } = string.Empty;
        public MainWindowControlClient(string email)
        {
            InitializeComponent();
            this.email = email;
            using (var db = new PhotoStudioDbContext())
            {
                currentClient = db.Clients.FirstOrDefault(c => c.EmailOfClient == email);
                if (currentClient == null)
                {
                    MessageBox.Show("Клієнт не знайдений.");
                    return;
                }
            }
            CheckUpNameClient.Text = "Вітаємо, " + currentClient.NameOfClient + "!";
        }

        private void ViewProfileClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProfileClient_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new PhotoStudioDbContext())
            {
                if (currentClient != null)
                {
                    db.Clients.Remove(currentClient);
                    db.SaveChanges();
                    MessageBox.Show("Ваш профіль успішно видалено.");
                    var loginWin = new StartupWindow_Login_();
                    loginWin.Show();
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win != loginWin)
                            win.Close();
                    }
                    Application.Current.MainWindow = loginWin;
                }
                else
                {
                    MessageBox.Show("Помилка: Клієнт не знайдений.");
                }
            }
        }

        private void ExitButtonClient_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        private void LookPortfolioButton_Click(object sender, RoutedEventArgs e)
        {
            var portfoliosLook = new PortfoliosLook(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = portfoliosLook;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpButtonClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetHelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
