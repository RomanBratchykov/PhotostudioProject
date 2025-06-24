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
    /// Interaction logic for AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        private Administrators? currentAdmin { get; set; }
        public AddWorker(string email)
        {
            InitializeComponent();
            using (var db = new PhotoStudioDbContext())
            {
                currentAdmin = db.Administrators.FirstOrDefault(a => a.EmailOfAdmin == email);
                if (currentAdmin == null)
                {
                    MessageBox.Show("Адміністратор не знайдений.");
                    return;
                }
            }
        }

        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {   
            string name = NameTextBoxAddWor.Text;
            string email = EmailTextBoxAddWor.Text;
            string phoneNumber = PhoneTextBoxAddWor.Text;
            string password = PasswordBoxLoginAddWor.Password;
            int idOfLocation = currentAdmin.IdOfLocation;
            int idOfAdmin = currentAdmin.IdAdmin;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(password))
            {
                NullErrorTextAddWor.Visibility = Visibility.Visible;
                return;
            }
            
            var newPhotographer = new Photographer
            {
                NameOfPhotographer = name,
                EmailOfPhotographer = email,
                PhoneNumberPhotographer = phoneNumber,
                PasswordPhotographer = password,
                IdOfLocation = idOfLocation,
                IdOfAdmin = idOfAdmin
            };
            using (var db = new PhotoStudioDbContext())
            {
                db.Photographers.Add(newPhotographer);
                db.SaveChanges();
            }
            MessageBox.Show("Фотограф успішно доданий.");
        }

        private void ReturnToAdminPageAddWor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
