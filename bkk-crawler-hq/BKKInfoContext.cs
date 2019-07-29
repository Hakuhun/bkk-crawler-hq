using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bkk_crawler_hq
{
    public partial class bkkinfoContext : DbContext
    {
        public bkkinfoContext()
        {
        }

        public bkkinfoContext(DbContextOptions<bkkinfoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bkkinfo> Bkkinfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("DataSource=bkkinfo.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Bkkinfo>(entity =>
            {
                entity.ToTable("bkkinfo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.End)
                    .HasColumnName("end")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.RouteType).HasColumnName("routeType");

                entity.Property(e => e.Start)
                    .HasColumnName("start")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Title).HasColumnName("title");
            });
        }
    }
}
