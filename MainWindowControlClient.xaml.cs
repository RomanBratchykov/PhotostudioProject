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
    /// Interaction logic for MainWindowControlClient.xaml
    /// </summary>
    public partial class MainWindowControlClient : UserControl
    {
        public MainWindowControlClient()
        {
            InitializeComponent();
        }

        private void ViewProfileClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProfileClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButtonClient_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void HelpButtonClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteToSupportButtonPH_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteToSupport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetHelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
