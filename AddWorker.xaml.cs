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
        public AddWorker()
        {
            InitializeComponent();
        }

        private void GetBackToAdminPageButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void DeleteWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void ReturnToAdminPageAddWor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
