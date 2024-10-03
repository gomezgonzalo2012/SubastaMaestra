using Microsoft.EntityFrameworkCore;
using SubastaMaestra.Data.SubastaMaestra.Data;
using SubastaMaestra.Entities.Core;
using SubastaMaestra.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Seeders
{
    public class DbSeeder 
    {
        private readonly SubastaContext context;
         public DbSeeder(SubastaContext context)
        {
            this.context = context;
        }

        public  async Task SeedAsync()
        {
            // Ensure the database is created
            //context.Database.EnsureCreated();

            // Check if the database has been seeded
            if (context.Products.Any() || context.Bids.Any() || context.Auctions.Any()|| context.Users.Any()
                || context.Roles.Any())
            {
                Console.WriteLine("Database already seeded.");
                return;
            }

            using var transaction = context.Database.BeginTransaction();
            
            try
            {
                context.Database.Migrate();
                await CheckCategoriesAsync();
                await CheckRolesAsync();
                await CheckUsersAsync();
                await CheckAuctionAsync();
                await CheckProductAsync();
                await CheckBidAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Rollback transaction if any error occurs
                transaction.Rollback();
                Console.WriteLine($"Error during seeding: {ex.Message}");
            }

        }
        private async Task CheckCategoriesAsync()
        {
           if (!context.ProductCategories.Any())
            {
                // seed Categories
            var categories = new List<ProductCategory>()
                {
                    new ProductCategory{ Name="Muebles"},
                    new ProductCategory{ Name="Electrodomésticos"},
                    new ProductCategory{ Name="Automotores"},
                    new ProductCategory{ Name="Motocicetas"},
                    new ProductCategory{ Name="Antiguedades"},
                };
                await context.ProductCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

            }
            
        }
        private async Task CheckRolesAsync()
        {
            if (!context.Roles.Any())
            {
                var roles = new List<UserRol>()
                {
                    new UserRol{Name="Admin"},
                    new UserRol{Name="User"}
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
        }

        private async Task CheckUsersAsync()
        {
            var roles = await context.Roles.ToListAsync();
            if (!context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User{
                             Name="George",
                             DocumentType=DocumType.DNI,
                             DocumentNumber= "12345678",
                             Email="admin@gmail.com",
                             Password="Password",
                             PersonType= PersonType.Fisica,
                             PhoneNumber="12345678",
                             RolId = roles.Single(c => c.Name == "Admin").Id, // obtiene de roles
                             State = State.Enable
                    },
                    new User{
                             Name="Subasta Maestra",
                             DocumentType=DocumType.CUIT,
                             DocumentNumber= "000-3232323",
                             Email="subata@gmail.com",
                             Password="Password",
                             PersonType= PersonType.Juridica,
                             PhoneNumber="12345678",
                             RolId = roles.Single(c => c.Name == "User").Id, // obtiene de roles
                             State = State.Enable
                    },
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

        }
        private async Task CheckAuctionAsync()
        {
            if (!context.Auctions.Any())
            {
                var auctions = new List<Auction>()
                {
                    new Auction
                    {
                        Title= "Motores de los 90",
                        StartDate = DateTime.Now,
                        CurrentState = AuctionState.Active,
                        FinishDate = DateTime.Now.AddDays(30),
                    },
                    new Auction
                    {
                        Title= "Rareza del siglo XVIII",
                        StartDate = DateTime.Now,
                        CurrentState = AuctionState.Active,
                        FinishDate = DateTime.Now.AddDays(15),
                    },
                    new Auction
                    {
                        Title= "Todo en Electrodomestico",
                        StartDate = DateTime.Now.AddDays(-3),
                        CurrentState = AuctionState.Active,
                        FinishDate = DateTime.Now.AddDays(-1),
                    }
                };

                await context.Auctions.AddRangeAsync(auctions);
                await context.SaveChangesAsync();

            }
           
        }

        private async Task CheckProductAsync()
        {
            if (!context.Products.Any())
            {
                
                // seed products
                var products = new List<Product>()
                {
                    new Product
                    {
                        Name="Opel Calibra",
                        Description = "Fabricado entre 1989 y 1997 con gran éxito, captó a la perfección el concepto de deportivo de clase media.",
                        ImgUrl = "insertar imagen",
                        CategoryId= context.ProductCategories.Single(c=>c.Name.Equals("Automotores")).Id,
                        AuctionId= context.Auctions.Single(a => a.Title=="Motores de los 90").Id,
                        Condition= ProductConditions.New,
                        InitialPrice= 16000,
                        DeliveryCondition= DeliveryModes.Domicilio,
                        CreatedAt= DateTime.Now,
                        CurrentState = ProductState.Pending,
                        FinalPrice= 0,
                        NumberOfOffers= 0,
                        SellerId= context.Users.Single(u=> u.Email.Equals("subata@gmail.com")).Id
                    },
                    new Product
                    {
                        Name="Renault Clio Williams",
                        Description = "El Clio fue, en los 90, uno de los utilitarios que mejor supo transmitir el espíritu deportivo.",
                        ImgUrl = "insertar imagen",
                        CategoryId= context.ProductCategories.Single(c=>c.Name.Equals("Automotores")).Id,
                        AuctionId= context.Auctions.Single(a => a.Title=="Motores de los 90").Id,
                        Condition= ProductConditions.New,
                        InitialPrice= 12000,
                        DeliveryCondition= DeliveryModes.Domicilio,
                        CreatedAt= DateTime.Now,
                        CurrentState = ProductState.Pending,
                        FinalPrice= 0,
                        NumberOfOffers= 0,
                        SellerId= context.Users.Single(u=> u.Email.Equals("subata@gmail.com")).Id
                    },
                    new Product
                    {
                        Name="Lavarropas Drean 2000",
                        Description = "Perfectas condiciones, 3600 recoliciones por minuto, hasta 5 kilogramos de peso",
                        ImgUrl = "insertar imagen",
                        CategoryId= context.ProductCategories.Single(c=>c.Name.Equals("Electrodomésticos")).Id,
                        AuctionId=  context.Auctions.Single(a => a.Title.Equals( "Todo en Electrodomestico")).Id,
                        Condition= ProductConditions.Used,
                        InitialPrice= 1000,
                        DeliveryCondition= DeliveryModes.RetiroLocal,
                        CreatedAt= DateTime.Now,
                        CurrentState = ProductState.Pending,
                        FinalPrice= 0,
                        NumberOfOffers= 0,
                        SellerId=  context.Users.Single(u=> u.Email.Equals("subata@gmail.com")).Id
                    },
                    new Product
                    {
                        Name="Aire acondicionado Philips",
                        Description = "Perfectas condiciones, 3600 frigorias, coneccion wifi",
                        ImgUrl = "insertar imagen",
                        CategoryId= context.ProductCategories.Single(c=>c.Name.Equals("Electrodomésticos")).Id,
                        AuctionId=  context.Auctions.Single(a => a.Title.Equals( "Todo en Electrodomestico")).Id,
                        Condition= ProductConditions.Used,
                        InitialPrice= 1200,
                        DeliveryCondition= DeliveryModes.Domicilio,
                        CreatedAt= DateTime.Now,
                        CurrentState = ProductState.Pending,
                        FinalPrice= 0,
                        NumberOfOffers= 0,
                        SellerId= context.Users.Single(u=> u.Email.Equals("subata@gmail.com")).Id
                    },

                };
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
        private async Task CheckBidAsync()
        {
            if (!context.Bids.Any())
            {
                var bids = new List<Bid> (){
                    new Bid {
                        BidderId= context.Users.Single(u => u.Email.Equals("subata@gmail.com")).Id,
                        OfferDate= DateTime.Now,
                        PaymentMethods = PaymentMethods.Efectivo,
                        ProductId= context.Products.Single(p=> p.Name.Equals("Opel Calibra")).Id,
                        Price= 30000
                        
                    },
                    new Bid {
                        BidderId= context.Users.Single(u => u.Email.Equals("subata@gmail.com")).Id,
                        OfferDate= DateTime.Now,
                        PaymentMethods = PaymentMethods.Efectivo,
                        ProductId= context.Products.Single(p=> p.Name.Equals("Renault Clio Williams")).Id,
                        Price= 30000

                    },
                    new Bid {
                        BidderId= context.Users.Single(u => u.Email.Equals("subata@gmail.com")).Id,
                        OfferDate= DateTime.Now,
                        PaymentMethods = PaymentMethods.Efectivo,
                        ProductId= context.Products.Single(p=> p.Name.Equals("Aire acondicionado Philips")).Id,
                        Price= 30000

                    }

                };
                await context.AddRangeAsync(bids);
                await context.SaveChangesAsync();
            }
        }
    }
}
