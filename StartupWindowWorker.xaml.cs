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
            this.Resources.MergedDictionaries.Clear();
            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                this.Resources.MergedDictionaries.Add(dict);
            }
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
            string? email = EmailTextBoxLoginWorker.Text.Trim();
            string? password = PasswordBoxLoginWorker.Password.Trim();
            using (var db = new PhotoStudioDbContext())
            {
                var admin = db.Administrators
                              .FirstOrDefault(a => a.EmailOfAdmin == email && a.PasswordAdmin == password);

                if (admin != null)
                {
                    MessageBox.Show($"Вітаємо, адміне {admin.NameOfAdmin}!");
                    var window = new MainWindow("admin", admin.EmailOfAdmin);
                    window.Show();
                    ((StartupWindow_Login_)Application.Current.MainWindow).Close();
                    Application.Current.MainWindow = window;
                    NullErrorTextWorker.Visibility = Visibility.Collapsed;
                    return;
                }

                var worker = db.Photographers
                               .FirstOrDefault(p => p.EmailOfPhotographer == email && p.PasswordPhotographer == password);

                if (worker != null)
                {
                    MessageBox.Show($"Вітаємо, {worker.NameOfPhotographer}!");
                    var window = new MainWindow("photographer", worker.EmailOfPhotographer);
                    window.Show();
                    ((StartupWindow_Login_)Application.Current.MainWindow).Close();
                    Application.Current.MainWindow = window;
                    NullErrorTextWorker.Visibility = Visibility.Collapsed;
                    return;
                }

                ErrorTextBlockLoginWorker.Visibility = Visibility.Visible;
            }
        }
    }
}
