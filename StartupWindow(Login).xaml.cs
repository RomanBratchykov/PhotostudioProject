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
    /// Interaction logic for StartupWindow_Login_.xaml
    /// </summary>
    public partial class StartupWindow_Login_ : Window
    {
        public StartupWindow_Login_()
        {
            InitializeComponent();
            ClientContentLogin.Content = new StartupWindowClient();
            this.Resources.MergedDictionaries.Clear();
            foreach (var dict in Application.Current.Resources.MergedDictionaries)
            {
                this.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
