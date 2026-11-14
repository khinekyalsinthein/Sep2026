using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sep2026.ZodiacHoroscope.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Horoscope> Horoscopes { get; set; }

    public virtual DbSet<ZodiacSign> ZodiacSigns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ZodiacHoroscopeDB;User ID=sa;Password=sasa@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Horoscope>(entity =>
        {
            entity.HasKey(e => e.HoroscopeId).HasName("PK__Horoscop__FD847190D5A7974A");

            entity.Property(e => e.Career).HasMaxLength(1000);
            entity.Property(e => e.Finance).HasMaxLength(1000);
            entity.Property(e => e.Health).HasMaxLength(1000);
            entity.Property(e => e.Love).HasMaxLength(1000);
            entity.Property(e => e.LuckyColor).HasMaxLength(50);
            entity.Property(e => e.Overall).HasMaxLength(1000);

            entity.HasOne(d => d.Zodiac).WithMany(p => p.Horoscopes)
                .HasForeignKey(d => d.ZodiacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Horoscopes_ZodiacSigns");
        });

        modelBuilder.Entity<ZodiacSign>(entity =>
        {
            entity.HasKey(e => e.ZodiacId).HasName("PK__ZodiacSi__A7EE3D9EB5A9BF01");

            entity.Property(e => e.DateRange).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Element).HasMaxLength(20);
            entity.Property(e => e.Symbol).HasMaxLength(10);
            entity.Property(e => e.ZodiacName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
