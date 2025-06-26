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
    /// Interaction logic for CompletedTasksWorker.xaml
    /// </summary>
    public partial class CompletedTasksWorker : UserControl
    {
        private Photographer? currentPhotographer { get; set; }
        public string email { get; set; }
        public CompletedTasksWorker(string email)
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
                var completedTasks = db.PhotoSessions
                    .Where(t => t.IdPhotographer == currentPhotographer.IdPhotographer && t.StatusOfSession == "Готова")
                    .ToList();
                CompletedSessionsWorker.ItemsSource = completedTasks.Where(r => r.StatusOfSession == "Готова");
            }
        }

        private void GetBackToPhotographer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var photographerControl = new MainWindowWorkerControl(email);
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = photographerControl;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }
    }
    
}
