﻿// <auto-generated />
using System;
using CloudDrive.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CloudDrive.EntityFramework.Migrations.MainDatabaseMigrations
{
    [DbContext(typeof(MainDatabaseContext))]
    partial class MainDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CloudDrive.Domain.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("CloudDrive.Domain.FileOperationsLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OperationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("FileOperationsLogs");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserDirectory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentDirectoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RelativePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentDirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("UserDirectories");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DirectoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("FileVersion")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelativePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("CloudDrive.Domain.FileOperationsLogs", b =>
                {
                    b.HasOne("CloudDrive.Domain.UserFile", "File")
                        .WithMany("FileOperationsLogs")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserDirectory", b =>
                {
                    b.HasOne("CloudDrive.Domain.UserDirectory", "ParentDirectory")
                        .WithMany("ChildDirectories")
                        .HasForeignKey("ParentDirectoryId");

                    b.HasOne("CloudDrive.Domain.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentDirectory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserFile", b =>
                {
                    b.HasOne("CloudDrive.Domain.UserDirectory", "Directory")
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId");

                    b.HasOne("CloudDrive.Domain.AppUser", "User")
                        .WithMany("UserFiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudDrive.Domain.AppUser", b =>
                {
                    b.Navigation("UserFiles");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserDirectory", b =>
                {
                    b.Navigation("ChildDirectories");

                    b.Navigation("Files");
                });

            modelBuilder.Entity("CloudDrive.Domain.UserFile", b =>
                {
                    b.Navigation("FileOperationsLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
