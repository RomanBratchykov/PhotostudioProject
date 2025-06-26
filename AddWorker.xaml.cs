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
            this.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Clear();

            // Завантажуємо тему з Settings
            var themePath = Properties.Settings.Default.CurrentTheme;

            try
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri(themePath, UriKind.RelativeOrAbsolute)
                };

                // Додаємо в Application глобально
                Application.Current.Resources.MergedDictionaries.Add(themeDict);

                // Додаємо в локальні ресурси вікна (якщо потрібно)
                this.Resources.MergedDictionaries.Add(themeDict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося завантажити тему: {ex.Message}");
            }
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
            this.DialogResult = true;
            string emailStart = newPhotographer.EmailOfPhotographer.Length >= 5
            ? newPhotographer.EmailOfPhotographer.Substring(0, 5)
            : newPhotographer.EmailOfPhotographer;
            this.Close();
        }

        private void ReturnToAdminPageAddWor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
