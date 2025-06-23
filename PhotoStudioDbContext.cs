using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace PhotostudioProject
{
    class PhotoStudioDbContext : DbContext
    {
        public string CurrentUser { get; set; } = string.Empty;
        public DbSet<Clients> Clients { get; set; }
        public DbSet<TypeOfService> TypeOfServices { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Photographer> Photographers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<FoldersOfPhotosUser> FoldersOfPhotosUsers { get; set; }
        public DbSet<FoldersOfPhotosPhotographer> FoldersOfPhotosPhotographers { get; set; }
        public DbSet<PhotoSessions> PhotoSessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Port=3306;Database=photostudio;User=root;Password=mypassword;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clients>(entity =>
            {
                entity.ToTable("clients");
                entity.HasKey(e => e.IdClient);
                entity.Property(e => e.IdClient).HasColumnName("id_client");
                entity.Property(e => e.NameOfClient).HasColumnName("name_of_client");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.EmailOfClient).HasColumnName("email");
                entity.Property(e => e.PasswordClient).HasColumnName("password_client");
            });

            modelBuilder.Entity<TypeOfService>(entity =>
            {
                entity.ToTable("type_of_service");
                entity.HasKey(e => e.IdService);
                entity.Property(e => e.IdService).HasColumnName("id_service");
                entity.Property(e => e.NameOfService).HasColumnName("name_of_service");
                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.ToTable("administrators");
                entity.HasKey(e => e.IdAdmin);
                entity.Property(e => e.IdAdmin).HasColumnName("id_admin");
                entity.Property(e => e.NameOfAdmin).HasColumnName("name_of_admin");
                entity.Property(e => e.IdOfLocation).HasColumnName("id_of_location");
                entity.Property(e => e.EmailOfAdmin).HasColumnName("email_of_admin");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.PasswordAdmin).HasColumnName("password_admin");
            });

            modelBuilder.Entity<Photographer>(entity =>
            {
                entity.ToTable("photographer");
                entity.HasKey(e => e.IdPhotographer);
                entity.Property(e => e.IdPhotographer).HasColumnName("id_photographer");
                entity.Property(e => e.NameOfWorker).HasColumnName("name_of_worker");
                entity.Property(e => e.IdOfLocation).HasColumnName("id_of_location");
                entity.Property(e => e.IdOfAdmin).HasColumnName("id_of_admin");
                entity.Property(e => e.EmailOfWorker).HasColumnName("email_of_worker");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.PasswordPhotographer).HasColumnName("password_photographer");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");
                entity.HasKey(e => e.IdOfLocation);
                entity.Property(e => e.IdOfLocation).HasColumnName("id_of_location");
                entity.Property(e => e.Address).HasColumnName("address");
            });

            modelBuilder.Entity<FoldersOfPhotosUser>(entity =>
            {
                entity.ToTable("folders_of_photos_user");
                entity.HasKey(e => e.IdFolder);
                entity.Property(e => e.IdFolder).HasColumnName("id_folder");
                entity.Property(e => e.IdUs).HasColumnName("id_us");
                entity.Property(e => e.FolderPath).HasColumnName("folder_path");
            });

            modelBuilder.Entity<FoldersOfPhotosPhotographer>(entity =>
            {
                entity.ToTable("folders_of_photos_photographer");
                entity.HasKey(e => e.IdFolder);
                entity.Property(e => e.IdFolder).HasColumnName("id_folder");
                entity.Property(e => e.IdPh).HasColumnName("id_ph");
                entity.Property(e => e.FolderPath).HasColumnName("folder_path");
            });

            modelBuilder.Entity<PhotoSessions>(entity =>
            {
                entity.ToTable("photo_sessions");
                entity.HasKey(e => e.IdPs);
                entity.Property(e => e.IdPs).HasColumnName("id_ps");
                entity.Property(e => e.IdClient).HasColumnName("id_client");
                entity.Property(e => e.IdPhotographer).HasColumnName("id_photographer");
                entity.Property(e => e.DateOfSession).HasColumnName("date_of_session");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.StatusOfSession).HasColumnName("status_of_session");
                entity.Property(e => e.TypeOfService).HasColumnName("type_of_service");
            });
        }
    }
}
