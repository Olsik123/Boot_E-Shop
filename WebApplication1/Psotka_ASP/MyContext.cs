using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class MyContext : DbContext
{
    public MyContext()
    {
    }

    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAdmin> TbAdmins { get; set; }

    public virtual DbSet<TbBoot> TbBoots { get; set; }

    public virtual DbSet<TbBootCategory> TbBootCategories { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbColor> TbColors { get; set; }

    public virtual DbSet<TbCustomer> TbCustomers { get; set; }

    public virtual DbSet<TbOrder> TbOrders { get; set; }

    public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; }

    public virtual DbSet<TbPhoto> TbPhotos { get; set; }

    public virtual DbSet<TbVariation> TbVariations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=mysqlstudenti.litv.sssvt.cz;user=psotkaoldrich;password=123456;database=4b2_psotkaoldrich_db2", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.25-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_czech_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<TbAdmin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.ToTable("tbAdmins");

            entity.Property(e => e.AdminId)
                .HasColumnType("int(11)")
                .HasColumnName("AdminID");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(200);
        });

        modelBuilder.Entity<TbBoot>(entity =>
        {
            entity.HasKey(e => e.BootId).HasName("PRIMARY");

            entity.ToTable("tbBoots");

            entity.Property(e => e.BootId)
                .HasColumnType("int(11)")
                .HasColumnName("BootID");
            entity.Property(e => e.Code).HasColumnType("int(11)");
            entity.Property(e => e.LongDescription).HasColumnType("mediumtext");
            entity.Property(e => e.Material).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasColumnType("text");
        });

        modelBuilder.Entity<TbBootCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tbBootCategories");

            entity.HasIndex(e => e.BootId, "BootID");

            entity.HasIndex(e => e.CategoryId, "CategoryID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.BootId)
                .HasColumnType("int(11)")
                .HasColumnName("BootID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("CategoryID");

            entity.HasOne(d => d.Boot).WithMany(p => p.TbBootCategories)
                .HasForeignKey(d => d.BootId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbBootCategories_ibfk_1");

            entity.HasOne(d => d.Category).WithMany(p => p.TbBootCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbBootCategories_ibfk_2");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("tbCategories");

            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("CategoryID");
            entity.Property(e => e.Left).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.Right).HasColumnType("int(11)");
        });

        modelBuilder.Entity<TbColor>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PRIMARY");

            entity.ToTable("tbColors");

            entity.Property(e => e.ColorId)
                .HasColumnType("int(11)")
                .HasColumnName("ColorID");
            entity.Property(e => e.Color).HasMaxLength(30);
            entity.Property(e => e.Hexadecimal).HasMaxLength(7);
        });

        modelBuilder.Entity<TbCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("tbCustomers");

            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("CustomerID");
            entity.Property(e => e.Adress).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Phone).HasColumnType("int(9)");
            entity.Property(e => e.PostalCode).HasColumnType("int(5)");
            entity.Property(e => e.Surname).HasMaxLength(30);
        });

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("tbOrders");

            entity.HasIndex(e => e.CustomerId, "CustomerID");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("OrderID");
            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("CustomerID");
            entity.Property(e => e.Discount).HasPrecision(11);
            entity.Property(e => e.Dph)
                .HasPrecision(11)
                .HasColumnName("DPH");
            entity.Property(e => e.Price).HasPrecision(11);

            entity.HasOne(d => d.Customer).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbOrders_ibfk_1");
        });

        modelBuilder.Entity<TbOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Odid).HasName("PRIMARY");

            entity.ToTable("tbOrderDetail");

            entity.HasIndex(e => e.OrderId, "OrderID");

            entity.HasIndex(e => e.VariationId, "VariationID");

            entity.Property(e => e.Odid)
                .HasColumnType("int(11)")
                .HasColumnName("ODID");
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.Discount).HasPrecision(11);
            entity.Property(e => e.Dph)
                .HasPrecision(11)
                .HasColumnName("DPH");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("OrderID");
            entity.Property(e => e.Photo).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(11);
            entity.Property(e => e.Quantity).HasColumnType("int(11)");
            entity.Property(e => e.Size).HasPrecision(11);
            entity.Property(e => e.VariationId)
                .HasColumnType("int(11)")
                .HasColumnName("VariationID");

            entity.HasOne(d => d.Order).WithMany(p => p.TbOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbOrderDetail_ibfk_2");

            entity.HasOne(d => d.Variation).WithMany(p => p.TbOrderDetails)
                .HasForeignKey(d => d.VariationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tbOrderDetail_ibfk_3");
        });

        modelBuilder.Entity<TbPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PRIMARY");

            entity.ToTable("tbPhotos");

            entity.HasIndex(e => e.BootId, "BootID");

            entity.Property(e => e.PhotoId)
                .HasColumnType("int(11)")
                .HasColumnName("PhotoID");
            entity.Property(e => e.BootId)
                .HasColumnType("int(11)")
                .HasColumnName("BootID");
            entity.Property(e => e.Path).HasMaxLength(1000);

            entity.HasOne(d => d.Boot).WithMany(p => p.TbPhotos)
                .HasForeignKey(d => d.BootId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbPhotos_ibfk_1");
        });

        modelBuilder.Entity<TbVariation>(entity =>
        {
            entity.HasKey(e => e.VariationId).HasName("PRIMARY");

            entity.ToTable("tbVariations");

            entity.HasIndex(e => e.BootId, "BootID");

            entity.HasIndex(e => e.ColorId, "ColorID");

            entity.Property(e => e.VariationId)
                .HasColumnType("int(11)")
                .HasColumnName("VariationID");
            entity.Property(e => e.BootId)
                .HasColumnType("int(11)")
                .HasColumnName("BootID");
            entity.Property(e => e.ColorId)
                .HasColumnType("int(11)")
                .HasColumnName("ColorID");
            entity.Property(e => e.Discount).HasPrecision(11);
            entity.Property(e => e.Dph)
                .HasPrecision(11)
                .HasColumnName("DPH");
            entity.Property(e => e.InStock).HasColumnType("int(10)");
            entity.Property(e => e.Price).HasPrecision(11);
            entity.Property(e => e.Size).HasPrecision(11);

            entity.HasOne(d => d.Boot).WithMany(p => p.TbVariations)
                .HasForeignKey(d => d.BootId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbVariations_ibfk_1");

            entity.HasOne(d => d.Color).WithMany(p => p.TbVariations)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tbVariations_ibfk_3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
