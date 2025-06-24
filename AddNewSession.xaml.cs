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
using System.Net.Mail;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.IO;
using System.Net;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.Globalization;
namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for AddNewSession.xaml
    /// </summary>
    public partial class AddNewSession : Window
    {
        private List<Photographer>? allPhotographers;
        private Clients? currentClient;
        private string email { get; set; } = string.Empty;
        public AddNewSession(string email)
        {
            InitializeComponent();
            this.email = email;
            using (var db = new PhotoStudioDbContext())
            {
                currentClient = db.Clients.FirstOrDefault(c => c.EmailOfClient == email);
                var locations = db.Locations.ToList(); 
                allPhotographers = db.Photographers.ToList();
                ChooseLocationComboBox.SelectionChanged += ChoosePhotographerComboBox_SelectionChanged;
                if (!locations.Any())
                {
                    MessageBox.Show("Немає локацій для відображення.");
                    return;
                }
                ChooseLocationComboBox.ItemsSource = locations;
                ChooseLocationComboBox.DisplayMemberPath = "Address";
                ChooseLocationComboBox.SelectedValuePath = "IdOfLocation";

                
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

           
        }

        [Obsolete]
        private void CreateNewSessionButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (ChooseLocationComboBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть локацію.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ChoosePhotographerComboBox.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть фотографа.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ChooseTypeOfSession.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть тип послуги.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SessionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Будь ласка, оберіть дату фотосесії.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (currentClient == null || string.IsNullOrWhiteSpace(currentClient.NameOfClient) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Недійсні дані клієнта або email.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var selectedLocation = ChooseLocationComboBox.SelectedItem as Location;
                var selectedPhotographer = ChoosePhotographerComboBox.SelectedItem as Photographer;
                var selectedService = ChooseTypeOfSession.SelectedItem as TypeOfService;

                var idOfClient = currentClient.IdClient;
                var nameOfClient = currentClient.NameOfClient;
                var nameOfPhotographer = selectedPhotographer.NameOfPhotographer;
                var nameOfService = selectedService.NameOfService;
                var price = selectedService.Price;

                const int cosmetologistPrice = 1000;
                const int stylistPrice = 1000;
                string additional = string.Empty;
                if (CosmetologistCheckBox.IsChecked == true)
                {
                    price += cosmetologistPrice;
                    additional += " візажист ";
                }
                if (StylistCheckBox.IsChecked == true)
                {
                    price += stylistPrice;
                    additional += " стиліст ";
                }

                using (var db = new PhotoStudioDbContext())
                {
                    var newSession = new PhotoSessions
                    {
                        NameOfClient = nameOfClient,
                        Location = selectedLocation.Address,
                        IdClient = idOfClient,
                        Price = price,
                        StatusOfSession = "Очікується",
                        IdPhotographer = selectedPhotographer.IdPhotographer,
                        TypeOfService = selectedService.IdService,
                        DateOfSession = SessionDatePicker.SelectedDate.Value,
                        NameOfPhotographer = nameOfPhotographer,
                        NameOfType = nameOfService
                    };
                    db.PhotoSessions.Add(newSession);
                    db.SaveChanges();
                }

                // Generate PDF with QuestPDF
                string pdfPath = Path.Combine(Path.GetTempPath(), $"Session_{Guid.NewGuid()}.pdf");

                QuestPDF.Settings.License = LicenseType.Community;
                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(14).FontFamily("Arial"));

                        page.Header()
                            .Text("Замовлення фотосесії")
                            .SemiBold().FontSize(20);

                        page.Content()
                            .Column(col =>
                            {
                                col.Spacing(10);
                                col.Item().Text($"Дата створення: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}");
                                col.Item().Text($"Фотограф: {nameOfPhotographer}");
                                col.Item().Text($"Замовник: {nameOfClient}");
                                col.Item().Text($"Тип послуги: {nameOfService}");
                                col.Item().Text($"Ціна: {price.ToString("C2", new CultureInfo("uk-UA"))}");
                                col.Item().Text($"Локація: {selectedLocation.Address}");
                                col.Item().Text($"Дата замовлення: {SessionDatePicker.SelectedDate.Value:dd/MM/yyyy}");
                                col.Item().Text($"Додаткові послуги: {(string.IsNullOrEmpty(additional) ? "Відсутні" : additional)}");
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text("Emerald Studio").FontSize(10);
                    });
                }).GeneratePdf(pdfPath);

                SendEmailWithAttachment(email, pdfPath);
                File.Delete(pdfPath);

                MessageBox.Show("Фотосесію успішно створено, PDF надіслано на пошту.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmailWithAttachment(string toEmail, string attachmentPath)
        {
            const string fromEmail = "photostudioemerald@gmail.com";
            const string fromEmailPassword = "mqoz ecow tqfj dcmr";
            const string smtpHost = "smtp.gmail.com";
            const int smtpPort = 587;

            try
            {
                var fromAddress = new MailAddress(fromEmail, "Emerald Studio");
                var toAddress = new MailAddress(toEmail);
                const string subject = "Ваше замовлення";
                const string body = "Дякуємо за замовлення! У вкладенні — PDF із деталями.";

                using var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromEmail, fromEmailPassword),
                    Timeout = 20000
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                using (var attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);
                    smtp.Send(message);
                }
            }
            catch (SmtpException ex)
            {
                throw new Exception($"Помилка надсилання email: {ex.Message}", ex);
            }
            catch (FormatException)
            {
                throw new Exception("Недійсний формат email адреси.");
            }
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
