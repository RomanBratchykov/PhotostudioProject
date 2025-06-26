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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;





namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for MainWindowControlClient.xaml
    /// </summary>
    public partial class MainWindowControlClient : UserControl
    {
        private Clients? currentClient { get; set; }
        private CancellationTokenSource? _loadingTokenSource;

        private MediaPlayer _player = new MediaPlayer();
        private async void PlaySoundForTwoSeconds()
        {
            _player.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds/soft-piano.mp3")));
            _player.Play();

            await Task.Delay(3000);
            _player.Stop();
        }
        private string email { get; set; } = string.Empty;
        public MainWindowControlClient(string email)
        {
            InitializeComponent();
            PlaySoundForTwoSeconds();
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
            using (var db = new PhotoStudioDbContext())
            {
                var sessions = db.PhotoSessions
                    .Where(p => p.IdClient == currentClient.IdClient && p.StatusOfSession != "Відмінена" && p.StatusOfSession != "Готова")
                    .ToList();

                ClientSessions.ItemsSource = sessions;
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
            var newSession = new AddNewSession(email);
            bool? result = newSession.ShowDialog();
            if (result == true)
            {
                RefreshSessions();
            }
            else
            {
                MessageBox.Show("Помилка при створенні нового сеансу.");
            }
        }
        public void RefreshSessions()
        {
            using (var db = new PhotoStudioDbContext())
            {
                var sessions = db.PhotoSessions
                    .Where(p => p.IdClient == currentClient.IdClient && p.StatusOfSession != "Відмінена")
                    .ToList();

                ClientSessions.ItemsSource = sessions;
            }
        }


        private void LookPhotosMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

  

        private void ClientSessions_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (ClientSessions.SelectedItem is PhotoSessions selectedSession)
            {
                var result = MessageBox.Show(
                "Ви впевнені, що хочете скасувати замовлення?",
                "Підтвердження скасування",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                 );

                if (result == MessageBoxResult.Yes)
                {
                    selectedSession.StatusOfSession = "Відмінена";
                    MessageBox.Show("Сесія скасована.");
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
    }
}
