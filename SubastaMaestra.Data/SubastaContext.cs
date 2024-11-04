using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;

    namespace SubastaMaestra.Data
    {
        public class SubastaContext : DbContext
        {
            public SubastaContext(DbContextOptions<SubastaContext> options) : base(options)
            {
            }
            // for composite key : para clave compuesta
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Bid>()
                      .HasKey(m => new { m.ProductId, m.BidderId });

                // Configurar las relaciones de Producto con Usuario
                modelBuilder.Entity<Sale>()
                    .HasOne (s=>s.Buyer)
                    .WithMany ()
                    .HasForeignKey (s => s.BuyerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                modelBuilder.Entity<Sale>()
                    .HasOne(s => s.Product)
                    .WithMany()
                    .HasForeignKey(s => s.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Seller)
                    .WithMany()  // Si no hay una colección de productos en Usuario, deja WithMany() vacío
                    .HasForeignKey(p => p.SellerId)
                    .OnDelete(DeleteBehavior.Restrict)  // Evita el borrado en cascada
                    .IsRequired();

                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Buyer)
                    .WithMany()  // Si no hay una colección de productos ganados en Usuario, deja WithMany() vacío
                    .HasForeignKey(p => p.BuyerId)
                    .OnDelete(DeleteBehavior.Restrict) // Evita el borrado en cascada
                    .IsRequired(false);

                modelBuilder.Entity<Bid>() // relaciones pujas
                    .HasOne(p => p.Product)
                    .WithMany()  // Si no hay una colección de productos en Usuario, deja WithMany() vacío
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Bid>()
                    .HasOne(p => p.Bidder)
                    .WithMany()  // Si no hay una colección de productos en Usuario, deja WithMany() vacío
                    .HasForeignKey(p => p.BidderId)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Auction>() // realcion subasta
                    .HasMany(p => p.Products)
                    .WithOne(p => p.Auction)
                    .HasForeignKey(p => p.AuctionId)
                    .IsRequired(); // requiere asignar una subasta al producto

                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(p => p.CategoryId);

                modelBuilder.Entity<User>()
                    .HasOne(p => p.Rol)
                    .WithMany()
                    .HasForeignKey(p => p.RolId)
                    .IsRequired();

                modelBuilder.Entity<Notification>()
                    .HasOne(n=>n.Product)
                    .WithMany()
                    .HasForeignKey(p => p.ProductoId)
                    .IsRequired();

                modelBuilder.Entity<Notification>()
                    .HasOne(n => n.User)
                    .WithMany(u=>u.Notifications)
                    .HasForeignKey(p => p.UserId)
                    .IsRequired();

                // datos unicos
                modelBuilder.Entity<ProductCategory>()
                    .HasIndex(u => u.Name)
                    .IsUnique();
                modelBuilder.Entity<User>()
                    .HasIndex(u => new { u.Email, u.DocumentNumber})
                    .IsUnique();

                modelBuilder.Entity<Sale>()
                    .HasIndex(s => s.ProductId)
                    .IsUnique();

                // de enums a strings

                modelBuilder.Entity<Product>()
                    .Property(p => p.Condition)
                    .HasConversion(new EnumToStringConverter<ProductConditions>());
                modelBuilder.Entity<Product>()
                    .Property(p => p.DeliveryCondition)
                    .HasConversion(new EnumToStringConverter<DeliveryModes>());
                modelBuilder.Entity<Product>()
                    .Property(p => p.CurrentState)
                    .HasConversion(new EnumToStringConverter<ProductState>());

                modelBuilder.Entity<Bid>()
                    .Property(p => p.PaymentMethods)
                    .HasConversion(new EnumToStringConverter<PaymentMethods>());
                modelBuilder.Entity<Sale>()
                   .Property(s => s.PaymentMethod)
                   .HasConversion(new EnumToStringConverter<PaymentMethods>());

                modelBuilder.Entity<Auction>()
                    .Property(p => p.CurrentState)
                    .HasConversion(new EnumToStringConverter<AuctionState>()); // cmbiar

                modelBuilder.Entity<User>()
                    .Property(p => p.PersonType)
                    .HasConversion(new EnumToStringConverter<PersonType>()); // per1.PersonType==PersonType.Juridica
                modelBuilder.Entity<User>()
                    .Property(p => p.DocumentType)
                    .HasConversion(new EnumToStringConverter<DocumType>());
                modelBuilder.Entity<User>()
                    .Property(p => p.State)
                    .HasConversion(new EnumToStringConverter<State>());

            }
            public DbSet<ProductCategory> ProductCategories { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Auction> Auctions { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Bid> Bids { get; set; }
            public DbSet<UserRol> Roles { get; set; }
            public DbSet<Sale> Sales { get; set; }
            public DbSet<Notification> Notifications { get; set; }

        }
    }


