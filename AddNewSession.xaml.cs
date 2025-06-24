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
    /// Interaction logic for AddNewSession.xaml
    /// </summary>
    public partial class AddNewSession : Window
    {
        private List<Photographer>? allPhotographers;
        private Clients? currentClient;
        public AddNewSession(string email)
        {
            InitializeComponent();
            using (var db = new PhotoStudioDbContext())
            {
                currentClient = db.Clients.FirstOrDefault(c => c.EmailOfClient == email);
                var locations = db.Locations.ToList();
                if (!locations.Any())
                {
                    MessageBox.Show("Немає локацій для відображення.");
                    return;
                }
                ChooseLocationComboBox.ItemsSource = locations;
                ChooseLocationComboBox.DisplayMemberPath = "Address";
                ChooseLocationComboBox.SelectedValuePath = "IdOfLocation";

                allPhotographers = db.Photographers.ToList();
                if (!allPhotographers.Any())
                {
                    MessageBox.Show("Немає фотографів для відображення.");
                    return;
                }

                var services = db.TypeOfServices.ToList();
                if (!services.Any())
                {
                    MessageBox.Show("Немає послуг для відображення.");
                    return;
                }
                ChooseTypeOfSession.ItemsSource = services;
            }

            ChooseLocationComboBox.SelectionChanged += ChoosePhotographerComboBox_SelectionChanged;
        }

        private void CreateNewSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseLocationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть локацію.");
                return;
            }
            if (ChoosePhotographerComboBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть фотографа.");
                return;
            }
            if (ChooseTypeOfSession.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть тип послуги.");
                return;
            }
            var selectedLocation = ChooseLocationComboBox.SelectedItem as Location;
            var selectedPhotographer = ChoosePhotographerComboBox.SelectedItem as Photographer;
            var selectedService = ChooseTypeOfSession.SelectedItem as TypeOfService;
            if (selectedLocation == null || selectedPhotographer == null || selectedService == null)
            {
                MessageBox.Show("Будь ласка, оберіть всі необхідні параметри.");
                return;
            }
            var idOfClient = currentClient.IdClient;
            var price = selectedService.Price;
            if (CosmetologistCheckBox.IsChecked == true)
            {
                price += 1000;
            }
            if (CosmetologistCheckBox.IsChecked == true)
            {
                price += 1000;
            }
            using (var db = new PhotoStudioDbContext())
            {
                var newSession = new PhotoSessions
                {
                    Location = selectedLocation.Address,
                    IdClient = idOfClient,
                    Price = price,
                    StatusOfSession = "Очікується",
                    IdPhotographer = selectedPhotographer.IdPhotographer,
                    TypeOfService = selectedService.IdService,
                    DateOfSession = SessionDatePicker.SelectedDate ?? DateTime.Now,
                };
                db.PhotoSessions.Add(newSession);
                db.SaveChanges();
            }
            this.DialogResult = true;
        }

        private void BackToClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ChoosePhotographerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedLocation = ChooseLocationComboBox.SelectedItem as Location;
            if (selectedLocation != null)
            {
                var filteredPhotographers = allPhotographers
                    .Where(p => p.IdOfLocation == selectedLocation.IdOfLocation)
                    .ToList();

                ChoosePhotographerComboBox.ItemsSource = filteredPhotographers;
                ChoosePhotographerComboBox.DisplayMemberPath = "NameOfPhotographer";
                ChoosePhotographerComboBox.SelectedValuePath = "IdPhotographer";
            }
            else
            {
                ChoosePhotographerComboBox.ItemsSource = null;
            }
        }
    }
}
