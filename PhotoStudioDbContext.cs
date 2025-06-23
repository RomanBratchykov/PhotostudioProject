using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotostudioProject
{
    class PhotoStudioDbContext : DbContext
    {
        public string CurrentUser { get; set; } = string.Empty;
        public DbSet<TypeOfService> TypeOfServices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Port=3306;Database=photostudio;User=root;Password=mypassword;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfService>().ToTable("type_of_service").HasKey(t => t.id_service);
        }
    }
}
