﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotostudioProject;

#nullable disable

namespace PhotostudioProject.Migrations
{
    [DbContext(typeof(PhotoStudioDbContext))]
    partial class PhotoStudioDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PhotostudioProject.TypeOfService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("TypeName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TypeOfServices");
                });
#pragma warning restore 612, 618
        }
    }
}
