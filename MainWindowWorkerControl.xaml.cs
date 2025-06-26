using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
        private MediaPlayer _player = new MediaPlayer();
        private async void PlaySoundForTwoSeconds()
        {
            _player.Open(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds/soft-piano.mp3")));
            _player.Play();

            await Task.Delay(3000);
            _player.Stop();
        }

        private string email { get; set; } = string.Empty;
        private Photographer? currentPhotographer { get; set; }
        public MainWindowWorkerControl(string email)
        {
            
            this.email = email;
            InitializeComponent();
            PlaySoundForTwoSeconds();
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
                Multiselect = false,
                Title = "Оберіть файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;
                    string fileName = Path.GetFileName(selectedFile);
                    string destinationPath = Path.Combine(folderPath, fileName);

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

        private void SendEmailWithAttachments(string toEmail, List<string> filePaths)
        {
            const string fromEmail = "photostudioemerald@gmail.com";
            const string fromPassword = "mqoz ecow tqfj dcmr";
            const string smtpHost = "smtp.gmail.com";
            const int smtpPort = 587;

            var fromAddress = new MailAddress(fromEmail, "Emerald Studio");
            var toAddress = new MailAddress(toEmail);

            using var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                Timeout = 20000
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "Ваші фото з фотостудії",
                Body = "Дякуємо! У вкладенні — ваші фото."
            };

            foreach (var path in filePaths)
            {
                var attachment = new Attachment(path);
                message.Attachments.Add(attachment);
            }

            smtp.Send(message);
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
                var idClient = selectedSession.IdClient;
                var clientEmail = string.Empty;
                using (var db = new PhotoStudioDbContext())
                {
                    var client = db.Clients.FirstOrDefault(c => c.IdClient == idClient);
                    if (client != null)
                    {
                        clientEmail = client.EmailOfClient;
                    }
                    else
                    {
                        MessageBox.Show("Клієнт не знайдений.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                    var result = MessageBox.Show(
                        "Cкинути фото?",
                        "Підтвердження скасування",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Multiselect = true,
                        Title = "Оберіть файли"
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            var selectedFiles = openFileDialog.FileNames.ToList();
                            SendEmailWithAttachments(clientEmail, selectedFiles);

                            MessageBox.Show("Фото відправлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                            selectedSession.StatusOfSession = "Готова";

                            using (var db = new PhotoStudioDbContext())
                            {
                                db.PhotoSessions.Update(selectedSession);
                                db.SaveChanges();
                            }

                            RefreshSessions();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Помилка при відправленні: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Відправку відмінено.");
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