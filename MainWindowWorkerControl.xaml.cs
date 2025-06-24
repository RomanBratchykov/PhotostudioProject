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
    /// Interaction logic for MainWindowWorkerControl.xaml
    /// </summary>
    public partial class MainWindowWorkerControl : UserControl
    {
        private string email { get; set; } = string.Empty;
        private Photographer? currentPhotographer { get; set; }
        public MainWindowWorkerControl(string email)
        {
            this.email = email;
            InitializeComponent();
            using (var db = new PhotoStudioDbContext())
            {

                currentPhotographer = db.Photographers.FirstOrDefault(p => p.EmailOfPhotographer == email);
                if (currentPhotographer == null)
                {
                    MessageBox.Show("Фотограф не знайдений.");
                    return;
                }
                var photographersSession = db.PhotoSessions.ToList().Where(r => r.IdPhotographer == currentPhotographer.IdPhotographer && r.StatusOfSession != "Відмінена" && r.StatusOfSession != "Готова");
                OrdersFowWorker.ItemsSource = photographersSession;
            }
            RefreshSessions();
            CheckUpName.Text = "Вітаємо, " + currentPhotographer.NameOfPhotographer + "!";
        }


        private void ExitButtonWorker_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void ViewProfileWorker_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetBackToLogin_Click(object sender, RoutedEventArgs e)
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

        private void PortfolioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CompletedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var completedTasks = new CompletedTasksWorker(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = completedTasks;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }

        private void ChangeMainPhoto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChoosePhotosPortfolio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersFowWorker_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrdersFowWorker.SelectedItem is PhotoSessions selectedSession)
            {
                var result = MessageBox.Show(
                "Cкинути фото?",
                "Підтвердження скасування",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                 );

                if (result == MessageBoxResult.Yes)
                {
                    selectedSession.StatusOfSession = "Готова";
                    MessageBox.Show("Фото відправлені.");
                    using (var db = new PhotoStudioDbContext())
                    {
                        db.PhotoSessions.Update(selectedSession);
                        db.SaveChanges();
                    }
                    RefreshSessions();
                }
                else
                {
                    MessageBox.Show("Скасування відмінено.");
                }
            }
        }
        public void RefreshSessions()
        {
            using (var db = new PhotoStudioDbContext())
            {
                if (currentPhotographer != null)
                {
                    var photographersSession = db.PhotoSessions
                        .Where(r => r.IdPhotographer == currentPhotographer.IdPhotographer && r.StatusOfSession != "Відмінена")
                        .ToList();
                    OrdersFowWorker.ItemsSource = photographersSession;
                }
            }
        }
    }
}