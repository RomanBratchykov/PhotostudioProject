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
    /// Interaction logic for ViewAllOrdersAdmin.xaml
    /// </summary>
    public partial class ViewAllOrdersAdmin : UserControl
    {
        private string email { get; set; } = string.Empty;

        private string? currentAdmin { get; set; } = string.Empty;
        public ViewAllOrdersAdmin(string email)
        {
            this.email = email;
            InitializeComponent();
            this.Resources.MergedDictionaries.Clear();
            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                this.Resources.MergedDictionaries.Add(dict);
            }
            RefreshSessions();
            using (var db = new PhotoStudioDbContext())
            {
                var admin = db.Administrators.FirstOrDefault(a => a.EmailOfAdmin == email);
                if (admin == null)
                {
                    MessageBox.Show("Адміністратор не знайдений.");
                    return;
                }
                var photographers = db.Photographers
                    .Where(p => p.IdOfAdmin == admin.IdAdmin)
                    .ToList();
                ChoosePhotographerComboBox.ItemsSource = photographers;
            }
        }

        private void GetBackToPhotographer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var photographerControl = new MainWindowAdminControl(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = photographerControl;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }

        private void ChoosePhotographerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChoosePhotographerComboBox.SelectedItem is Photographer selected)
            {
                int id = selected.IdPhotographer;
                RefreshSessions(id);
            }
        }
        private void RefreshSessions(int idPhotographer)
        {
            using (var db = new PhotoStudioDbContext())
            {
                var admin = db.Administrators.FirstOrDefault(a => a.EmailOfAdmin == email);
                if (admin == null)
                {
                    MessageBox.Show("Адміністратор не знайдений.");
                    return;
                }
                var sessions = db.PhotoSessions
                    .Where(s => s.IdPhotographer == idPhotographer)
                    .ToList();
                SessionsForAdmin.ItemsSource = sessions;
            }
        }
        private void RefreshSessions()
        {
            using (var db = new PhotoStudioDbContext())
            {
                var admin = db.Administrators.FirstOrDefault(a => a.EmailOfAdmin == email);
                if (admin == null)
                {
                    MessageBox.Show("Адміністратор не знайдений.");
                    return;
                }
                var photographerIds = db.Photographers
                    .Where(p => p.IdOfAdmin == admin.IdAdmin)
                    .Select(p => p.IdPhotographer)
                    .ToList();
                var sessions = db.PhotoSessions
                    .Where(s => photographerIds.Contains(s.IdPhotographer))
                    .ToList();
                SessionsForAdmin.ItemsSource = sessions;
            }
        }
    }
}
