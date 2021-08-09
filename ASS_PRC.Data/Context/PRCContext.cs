using System;
using ASS_PRC.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASS_PRC.Data.Context
{
    public partial class PRCContext : DbContext
    {
        public PRCContext()
        {
        }

        public PRCContext(DbContextOptions<PRCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryMedia> CategoryMedia { get; set; }
        public virtual DbSet<CategoryPlaylist> CategoryPlaylist { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Playlist> Playlist { get; set; }
        public virtual DbSet<PlaylistDetail> PlaylistDetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:prckhanhnd.database.windows.net,1433;Database=PRC;User ID=khanhnd;Password=0939761499Kt;Trusted_Connection=False;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirebaseProvider)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirebaseUid)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<CategoryMedia>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryMedia)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryMedia_Category");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.CategoryMedia)
                    .HasForeignKey(x => x.MediaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryMedia_Media");
            });

            modelBuilder.Entity<CategoryPlaylist>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryPlaylist)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryPlaylist_Category");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.CategoryPlaylist)
                    .HasForeignKey(x => x.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryPlaylist_Playlist");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(700)
                    .IsUnicode(false);

                entity.Property(e => e.MusicName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Singer).HasMaxLength(50);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Media)
                    .HasForeignKey(x => x.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_Account");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.PlaylistName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Playlist)
                    .HasForeignKey(x => x.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Playlist_Account");
            });

            modelBuilder.Entity<PlaylistDetail>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.PlaylistDetail)
                    .HasForeignKey(x => x.MediaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistDetail_Media");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistDetail)
                    .HasForeignKey(x => x.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlaylistDetail_Playlist");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
