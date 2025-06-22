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
    /// Interaction logic for MainWindowAdminControl.xaml
    /// </summary>
    public partial class MainWindowAdminControl : UserControl
    {
        public MainWindowAdminControl()
        {
            InitializeComponent();
        }

        private void ViewProfileAdmin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateRaport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButtonAdmin_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
