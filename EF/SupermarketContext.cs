using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupermarketEF.Models;

namespace SupermarketEF.EF;

public partial class SupermarketContext : DbContext 
{
    public SupermarketContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductInReceipt> ProductInReceipts { get; set; }
    public DbSet<ProductInSupermarket> ProductInSupermarkets { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Supermarket> Supermarkets { get; set; }
    public DbSet<Worker> Workers { get; set; }       
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
             .AddJsonFile($"appsettings.json", true, true).Build();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Department");
            entity.Property(e => e.ProductType).HasMaxLength(30);
            entity.HasOne(d => d.Supermarket).WithMany(p => p.Departments)
                .HasForeignKey(d => d.SupermarketId)
                .OnDelete(DeleteBehavior.Cascade); 
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Product");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.ProductType).HasMaxLength(30);
        });
        modelBuilder.Entity<ProductInReceipt>(entity =>
        {
            entity.HasKey(e => new { e.ReceiptId, e.ProductId });
            entity.ToTable("ProductInReceipt");
            entity.HasOne(d => d.Product).WithMany(p => p.ProductInReceipts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);             
            entity.HasOne(d => d.Receipt).WithMany(p => p.ProductInReceipts)
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);                
        });
        modelBuilder.Entity<ProductInSupermarket>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Supermarket });
            entity.ToTable("ProductInSupermarket");
            entity.HasOne(d => d.Product).WithMany(p => p.ProductInSupermarkets)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.SupermarketNavigation).WithMany(p => p.ProductInSupermarkets)
                .HasForeignKey(d => d.Supermarket)
                .OnDelete(DeleteBehavior.Cascade);               
        });
        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Receipt");
            entity.HasOne(d => d.Worker).WithMany(p => p.Receipts).HasForeignKey(d => d.WorkerId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Supermarket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Supermarket");
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Worker");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Position).HasMaxLength(30);
            entity.HasMany(d => d.Departments).WithMany(p => p.Workers).UsingEntity(p => p.ToTable("WorkerDepartment"));
        });
    }
}
