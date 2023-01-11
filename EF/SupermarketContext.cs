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
    public DbSet<Person> Persons { get; set; }
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
            entity.Property(e => e.Price).HasDefaultValue(10);
            entity.ToTable(p => p.HasCheckConstraint("Price", "Price > 0"));
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
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Person")
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Person>("Person");
            entity.ToTable("Person")
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Worker>("Worker");
            entity.Property(e => e.Name).HasMaxLength(30);
        });
        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.Position).HasMaxLength(30);
            entity.HasMany(d => d.Departments).WithMany(p => p.Workers).UsingEntity(p => p.ToTable("WorkerDepartment"));
        });
        modelBuilder.Entity<Supermarket>().HasData(
            new Supermarket { Id = 1, Name = "supermarket1", Adress = "adress1"},
            new Supermarket { Id = 2, Name = "supermarket2", Adress = "adress2"},
            new Supermarket { Id = 3, Name = "supermarket3", Adress = "adress3"}
        );
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, ProductType = "type1", SupermarketId = 1},
            new Department { Id = 2, ProductType = "type1", SupermarketId = 2},
            new Department { Id = 3, ProductType = "type1", SupermarketId = 3}
        );
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "product1", Price = 100, ProductType = "type1" },
            new Product { Id = 2, Name = "product2", Price = 200, ProductType = "type2" },
            new Product { Id = 3, Name = "product3", Price = 300, ProductType = "type3" }
        );
        modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, Name = "name1", Birthday = DateTime.Now },
            new Person { Id = 2, Name = "name2", Birthday = DateTime.Now },
            new Person { Id = 3, Name = "name3", Birthday = DateTime.Now }
        );
        modelBuilder.Entity<Worker>().HasData(
            new Worker { 
                Id  = 4, 
                Name = "name4", 
                Birthday = DateTime.Now, 
                Position = "position4",
                DateOfEmployment = DateTime.Now
            },
            new Worker { 
                Id  = 5, 
                Name = "name5", 
                Birthday = DateTime.Now, 
                Position = "position5",
                DateOfEmployment = DateTime.Now
            },
            new Worker { 
                Id  = 6, 
                Name = "name6", 
                Birthday = DateTime.Now, 
                Position = "position6",
                DateOfEmployment = DateTime.Now
            }
        );
        modelBuilder.Entity<ProductInSupermarket>().HasData(
            new ProductInSupermarket { 
                Id = 1, 
                Supermarket = 1, 
                ProductId = 1, 
                Amount = 10, 
                ExpirationDate = DateTime.Now
            },
            new ProductInSupermarket { 
                Id = 2, 
                Supermarket = 2, 
                ProductId = 2, 
                Amount = 20, 
                ExpirationDate = DateTime.Now
            },
            new ProductInSupermarket { 
                Id = 3, 
                Supermarket = 3, 
                ProductId = 3, 
                Amount = 30, 
                ExpirationDate = DateTime.Now
            }
        );
        modelBuilder.Entity<Receipt>().HasData(
            new Receipt { Id = 1, Date = DateTime.Now, WorkerId = 1, Price = 100},
            new Receipt { Id = 2, Date = DateTime.Now, WorkerId = 2, Price = 200},
            new Receipt { Id = 3, Date = DateTime.Now, WorkerId = 3, Price = 300}
        );
        modelBuilder.Entity<ProductInReceipt>().HasData(
            new ProductInReceipt { ReceiptId = 1, ProductId = 1, Amount = 10},
            new ProductInReceipt { ReceiptId = 2, ProductId = 2, Amount = 20},
            new ProductInReceipt { ReceiptId = 3, ProductId = 3, Amount = 30}
        );
    }
}
