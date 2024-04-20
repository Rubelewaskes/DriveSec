using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DriveSec.Models;

public partial class DriveSecContext : DbContext
{
    public DriveSecContext()
    {
    }

    public DriveSecContext(DbContextOptions<DriveSecContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChangeHistory> ChangeHistories { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Folder> Folders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersFolder> UsersFolders { get; set; }

    public virtual DbSet<UsersMac> UsersMacs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5438;Database=dbDS;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChangeHistory>(entity =>
        {
            entity.HasKey(e => e.ChangeHistoryId).HasName("change_history_pkey");

            entity.ToTable("change_history");

            entity.Property(e => e.ChangeHistoryId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("change_history_id");
            entity.Property(e => e.ChangeDescription).HasColumnName("change_description");
            entity.Property(e => e.DateChange)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_change");
            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.File).WithMany(p => p.ChangeHistories)
                .HasForeignKey(d => d.FileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("change_history_file_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ChangeHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("change_history_user_id_fkey");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.FileId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("file_id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FolderId).HasColumnName("folder_id");
            entity.Property(e => e.UploaderId).HasColumnName("uploader_id");
            entity.Property(e => e.VirusAvailiability).HasColumnName("virus_availiability");
            entity.Property(e => e.VirusDescription).HasColumnName("virus_description");

            entity.HasOne(d => d.Folder).WithMany(p => p.Files)
                .HasForeignKey(d => d.FolderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("files_folder_id_fkey");

            entity.HasOne(d => d.Uploader).WithMany(p => p.Files)
                .HasForeignKey(d => d.UploaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("files_uploader_id_fkey");
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.FolderId).HasName("folders_pkey");

            entity.ToTable("folders");

            entity.Property(e => e.FolderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("folder_id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date");
            entity.Property(e => e.FolderDescription).HasColumnName("folder_description");
            entity.Property(e => e.FolderName)
                .HasMaxLength(255)
                .HasColumnName("folder_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<UsersFolder>(entity =>
        {
            entity.HasKey(e => e.UsersFolderId).HasName("users_folder_pkey");

            entity.ToTable("users_folder");

            entity.Property(e => e.UsersFolderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("users_folder_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.FolderId).HasColumnName("folder_id");
            entity.Property(e => e.Role)
                .HasMaxLength(32)
                .HasColumnName("role");

            entity.HasOne(d => d.Folder).WithMany(p => p.UsersFolders)
                .HasForeignKey(d => d.FolderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_folder_folder_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersFolders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_folder_user_id_fkey");
        });

        modelBuilder.Entity<UsersMac>(entity =>
        {
            entity.HasKey(e => e.UserMacId).HasName("users_mac_pkey");

            entity.ToTable("users_mac");

            entity.Property(e => e.UserMacId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_mac_id");
            entity.Property(e => e.Mac)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("mac");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UsersMacs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_mac_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
