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
    /// Interaction logic for CompletedTasksWorker.xaml
    /// </summary>
    public partial class CompletedTasksWorker : UserControl
    {
        public CompletedTasksWorker()
        {
            InitializeComponent();
        }

        private void GetBackToPhotographer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var photographerControl = new MainWindowWorkerControl();
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Content = photographerControl;
            ((MainWindow)Application.Current.MainWindow).MainWindowContent.Visibility = Visibility.Visible;
        }
    }
}
