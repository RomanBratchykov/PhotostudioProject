using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Photos");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Вікно вибору файлу
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Зображення (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false,
                Title = "Оберіть фото"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;
                    string fileName = Path.GetFileName(selectedFile);
                    string destinationPath = Path.Combine(folderPath, fileName);

                    // Якщо такий файл вже є — додай суфікс
                    int counter = 1;
                    while (File.Exists(destinationPath))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                        string ext = Path.GetExtension(fileName);
                        destinationPath = Path.Combine(folderPath, $"{nameWithoutExt}_{counter}{ext}");
                        counter++;
                    }

                    File.Copy(selectedFile, destinationPath);
                    MessageBox.Show("Фото успішно додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при додаванні фото: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CompletedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var completedTasks = new CompletedTasksWorker(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = completedTasks;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
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
                        .Where(r => r.IdPhotographer == currentPhotographer.IdPhotographer && r.StatusOfSession != "Відмінена" && r.StatusOfSession != "Готова")
                        .ToList();
                    OrdersFowWorker.ItemsSource = photographersSession;
                }
            }
        }
    }
}