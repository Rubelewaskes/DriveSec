﻿// <auto-generated />
using System;
using DriveSec.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DriveSec.Migrations
{
    [DbContext(typeof(DriveSecContext))]
    partial class DriveSecContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DriveSec.Models.ChangeHistory", b =>
                {
                    b.Property<int>("ChangeHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("change_history_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("ChangeHistoryId"));

                    b.Property<string>("ChangeDescription")
                        .HasColumnType("text")
                        .HasColumnName("change_description");

                    b.Property<DateTime>("DateChange")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_change");

                    b.Property<int>("FileId")
                        .HasColumnType("integer")
                        .HasColumnName("file_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("ChangeHistoryId")
                        .HasName("change_history_pkey");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("change_history", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("file_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("FileId"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("file_name");

                    b.Property<int>("FolderId")
                        .HasColumnType("integer")
                        .HasColumnName("folder_id");

                    b.Property<int>("UploaderId")
                        .HasColumnType("integer")
                        .HasColumnName("uploader_id");

                    b.Property<bool>("VirusAvailiability")
                        .HasColumnType("boolean")
                        .HasColumnName("virus_availiability");

                    b.Property<string>("VirusDescrition")
                        .HasColumnType("text")
                        .HasColumnName("virus_descrition");

                    b.HasKey("FileId")
                        .HasName("files_pkey");

                    b.HasIndex("FolderId");

                    b.HasIndex("UploaderId");

                    b.ToTable("files", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.Folder", b =>
                {
                    b.Property<int>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("folder_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("FolderId"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_date");

                    b.Property<string>("FolderDescription")
                        .HasColumnType("text")
                        .HasColumnName("folder_description");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("folder_name");

                    b.HasKey("FolderId")
                        .HasName("folders_pkey");

                    b.ToTable("folders", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("UserId"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.HasKey("UserId")
                        .HasName("users_pkey");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.UsersFolder", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<int>("FolderId")
                        .HasColumnType("integer")
                        .HasColumnName("folder_id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("role");

                    b.HasKey("UserId", "FolderId")
                        .HasName("users_folder_pk");

                    b.HasIndex("FolderId");

                    b.ToTable("users_folder", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.UsersMac", b =>
                {
                    b.Property<int>("UserMacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_mac_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("UserMacId"));

                    b.Property<string>("Mac")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("character(12)")
                        .HasColumnName("mac")
                        .IsFixedLength();

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("UserMacId")
                        .HasName("users_mac_pkey");

                    b.HasIndex("UserId");

                    b.ToTable("users_mac", (string)null);
                });

            modelBuilder.Entity("DriveSec.Models.ChangeHistory", b =>
                {
                    b.HasOne("DriveSec.Models.File", "File")
                        .WithMany("ChangeHistories")
                        .HasForeignKey("FileId")
                        .IsRequired()
                        .HasConstraintName("change_history_file_id_fkey");

                    b.HasOne("DriveSec.Models.User", "User")
                        .WithMany("ChangeHistories")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("change_history_user_id_fkey");

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DriveSec.Models.File", b =>
                {
                    b.HasOne("DriveSec.Models.Folder", "Folder")
                        .WithMany("Files")
                        .HasForeignKey("FolderId")
                        .IsRequired()
                        .HasConstraintName("files_folder_id_fkey");

                    b.HasOne("DriveSec.Models.User", "Uploader")
                        .WithMany("Files")
                        .HasForeignKey("UploaderId")
                        .IsRequired()
                        .HasConstraintName("files_uploader_id_fkey");

                    b.Navigation("Folder");

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("DriveSec.Models.UsersFolder", b =>
                {
                    b.HasOne("DriveSec.Models.Folder", "Folder")
                        .WithMany("UsersFolders")
                        .HasForeignKey("FolderId")
                        .IsRequired()
                        .HasConstraintName("users_folder_folder_id_fkey");

                    b.HasOne("DriveSec.Models.User", "User")
                        .WithMany("UsersFolders")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("users_folder_user_id_fkey");

                    b.Navigation("Folder");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DriveSec.Models.UsersMac", b =>
                {
                    b.HasOne("DriveSec.Models.User", "User")
                        .WithMany("UsersMacs")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("users_mac_user_id_fkey");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DriveSec.Models.File", b =>
                {
                    b.Navigation("ChangeHistories");
                });

            modelBuilder.Entity("DriveSec.Models.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("UsersFolders");
                });

            modelBuilder.Entity("DriveSec.Models.User", b =>
                {
                    b.Navigation("ChangeHistories");

                    b.Navigation("Files");

                    b.Navigation("UsersFolders");

                    b.Navigation("UsersMacs");
                });
#pragma warning restore 612, 618
        }
    }
}
