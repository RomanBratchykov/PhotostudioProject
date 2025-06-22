using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

namespace PhotostudioProject
{
    public class PhotoStudioDbContext : DbContext
    {
        public string CurrentUser { get; set; } = string.Empty;
        //    public DbSet<TypeOfService> TypeOfServices { get; set; }  

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  
        //    {  
        //        var connectionString = "Server=localhost;Port=3306;Database=photostudio;User=root;Password=mypassword;";  
        //        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));  
        //    }  
        //    protected override void OnModelCreating(ModelBuilder modelBuilder)  
        //    {  
        //        modelBuilder.Entity<TypeOfService>().ToTable("type_of_service").HasKey(t => t.id_service);  
        //    }  
        //}  

        //public class TypeOfService  
        //{  
        //    public int id_service { get; set; }  
        //    public string? name_of_service { get; set; }  

        //    public int? price { get; set; }  
        //}  
    }
    public partial class MainWindow : Window
        {
        public string CurrentUser { get; set; } = string.Empty;
        public MainWindow(string currentUser)
            {
            InitializeComponent();
            CurrentUser = currentUser;
            if (currentUser == "client")
            {
                var clientControl = new MainWindowControlClient();
                MainWindowContent.Content = clientControl;

            }
            else if (currentUser == "photographer")
            {
                var workerControl = new MainWindowWorkerControl();
                MainWindowContent.Content = workerControl;
            }
            //else if (currentUser == "admin")
            //{
            //    var workerControl = new StartupWindowWorker();
            //    ClientContentLogin.Content = workerControl;
            //}
            else
            {
                throw new ArgumentException("Invalid user type");
            }
        }

        }
 }