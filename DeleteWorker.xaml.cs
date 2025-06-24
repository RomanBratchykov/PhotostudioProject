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
using System.Windows.Shapes;

namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for DeleteWorker.xaml
    /// </summary>
    public partial class DeleteWorker : Window
    {
        private Administrators? currentAdmin { get; set; }
        private string email { get; set; } 
        public DeleteWorker(string email)
        {
            InitializeComponent();
            this.email = email;
            using (var db = new PhotoStudioDbContext())
            {
                currentAdmin = db.Administrators.FirstOrDefault(a => a.EmailOfAdmin == email);
                if (currentAdmin == null)
                {
                    MessageBox.Show("Адміністратор не знайдений.");
                    return;
                }
            }
            LoadPhotographers(currentAdmin);
        }
        private void GetBackToAdminPageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DeleteWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteWorkerComboBox.SelectedValue is int selectedId)
            {
                DeletePhotographerById(selectedId);
                LoadPhotographers(currentAdmin); 
            }
            else
            {
                MessageBox.Show("Оберіть фотографа для видалення.");
            }
        }
        private void LoadPhotographers( Administrators currentAdmin)
        {
            using (var db = new PhotoStudioDbContext())
            {
                var photographers = db.Photographers.ToList().Where(r => r.IdOfLocation == currentAdmin.IdOfLocation);
                DeleteWorkerComboBox.ItemsSource = photographers;
                DeleteWorkerComboBox.DisplayMemberPath = "NameOfWorker";
                DeleteWorkerComboBox.SelectedValuePath = "IdPhotographer";
            }
        }
        private void DeletePhotographerById(int photographerId)
        {
            using (var db = new PhotoStudioDbContext())
            {
                var photographer = db.Photographers.FirstOrDefault(p => p.IdPhotographer == photographerId);
                if (photographer != null)
                {
                    db.Photographers.Remove(photographer);
                    db.SaveChanges();
                    MessageBox.Show("Фотограф успішно видалений.");
                }
                else
                {
                    MessageBox.Show("Фотограф не знайдений.");
                }
            }
        }
    }
}
