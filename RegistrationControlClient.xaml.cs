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
using System.IO;
using System.Net;
using System.Net.Mail;


namespace PhotostudioProject
{
    /// <summary>
    /// Interaction logic for RegistrationControlClient.xaml
    /// </summary>
    public partial class RegistrationControlClient : UserControl
    {
        public RegistrationControlClient()
        {
            InitializeComponent();
            var registrationClient = new Clients();
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
        }

        private void ReturnToMainPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clientControl = new StartupWindowClient();
            ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = clientControl;
        }
        private string GenerateVerificationCode(int length = 6)
        {
            Random rnd = new Random();
            return rnd.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
        }
        private void SendVerificationEmail(string recipientEmail, string code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("photostudioemerald@gmail.com");
                mail.To.Add(recipientEmail);
                mail.Subject = "Код підтвердження";
                mail.Body = $"Ваш код підтвердження: {code}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("photostudioemerald@gmail.com", "mqoz ecow tqfj dcmr");
                smtp.EnableSsl = true;

                smtp.Send(mail);
                MessageBox.Show("Код надіслано на пошту");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка надсилання: {ex.Message}");
            }
        }
        private void RegistrationButton1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBoxReg.Text) ||
        string.IsNullOrWhiteSpace(NameTextBoxReg.Text) ||
        string.IsNullOrWhiteSpace(PhoneTextBoxReg.Text))
            {
                NullErrorTextReg.Visibility = Visibility.Visible;
                return;
            }

            string email = EmailTextBoxReg.Text;
            string name = NameTextBoxReg.Text;
            string phone = PhoneTextBoxReg.Text;
            string code = GenerateVerificationCode();

            try
            {
                SendVerificationEmail(email, code);

                var state = new RegistrationState
                {
                    Email = email,
                    Name = name,
                    Phone = phone,
                    Code = code
                };

                var checkEmail = new EmailCheckCode(state);
                ((StartupWindow_Login_)Application.Current.MainWindow).ClientContentLogin.Content = checkEmail;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка реєстрації: {ex.Message}");
                return;
            }
        }
    }
}
