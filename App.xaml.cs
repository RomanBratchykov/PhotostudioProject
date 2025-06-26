using System.Configuration;
using System.Data;
using System.Windows;
using PhotostudioProject.Properties;

namespace PhotostudioProject
{
    /// <summary>  
    /// Interaction logic for App.xaml  
    /// </summary>  
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Access the settings correctly  
            string savedTheme = Settings.Default.CurrentTheme;
            if (!string.IsNullOrEmpty(savedTheme))
            {
                var uri = new Uri(savedTheme, UriKind.Relative);
                var theme = new ResourceDictionary { Source = uri };

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(theme);
            }
        }
    }

}
