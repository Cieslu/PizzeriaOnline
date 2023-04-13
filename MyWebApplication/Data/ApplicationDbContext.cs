#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.Models;

namespace MyWebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<MeatVegetable> MeatsVegetables { get; set; }
        public DbSet<Meat> Meats { get; set; }
        public DbSet<Vegetable> Vegetables { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Sauce> Sauces { get; set; }
        public DbSet<Dough> Doughs { get; set; }
        public DbSet<Cheese> Cheeses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<TempOrderInBasket> TempOrders { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Size>().HasData(new Size[]
            {
                new Size {Id = 1, Name = "mała", Price = 20.00},
                new Size {Id = 2, Name = "średnia", Price = 25.00},
                new Size {Id = 3, Name = "duża", Price = 30.00},
                new Size {Id = 4, Name = "mega", Price = 35.00}
            });

            modelBuilder.Entity<Dough>().HasData(new Dough[]
            {
                new Dough {Id = 1, Name = "cienkie", Price = 3.00},
                new Dough {Id = 2, Name = "grube", Price = 6.00},
                new Dough {Id = 3, Name = "ser w brzegach", Price = 9.00}
            });

            modelBuilder.Entity<Cheese>().HasData(new Cheese[]
            {
                new Cheese {Id = 1, Name = "mozzarella", Price = 0.00},
                new Cheese {Id = 2, Name = "podwójna mozzarella", Price = 4.00}
            });

            modelBuilder.Entity<Sauce>().HasData(new Sauce[]
            {
                new Sauce {Id = 1, Name = "sos pomidorowy", Price = 2.00},
                new Sauce {Id = 2, Name = "sos kremowy", Price = 2.00},
                new Sauce {Id = 3, Name = "sos pikantny pomidorowy", Price = 2.00},
                new Sauce {Id = 4, Name = "sos czosnkowy", Price = 2.00}
            });

            modelBuilder.Entity<Meat>().HasData(new Meat[]
            {
                new Meat {Id = 1, Name = "pepperoni", Price = 5.20},
                new Meat {Id = 2, Name = "szynka", Price = 5.20},
                new Meat {Id = 3, Name = "wołowina", Price = 5.20},
                new Meat {Id = 4, Name = "wieprzowina", Price = 5.20},
                new Meat {Id = 5, Name = "kurczak", Price = 5.20},
                new Meat {Id = 6, Name = "boczek", Price = 5.20}
            });

            modelBuilder.Entity<Vegetable>().HasData(new Vegetable[]
            {
                new Vegetable {Id = 1, Name = "kukurydza", Price = 5.00},
                new Vegetable {Id = 2, Name = "oliwki", Price = 5.00},
                new Vegetable {Id = 3, Name = "papryczki jalapenos", Price = 5.00},
                new Vegetable {Id = 4, Name = "pieczarki", Price = 5.00},
                new Vegetable {Id = 5, Name = "papryka", Price = 5.00},
                new Vegetable {Id = 6, Name = "cebula", Price = 5.00},
                new Vegetable {Id = 7, Name = "pomidory", Price = 5.00},
                new Vegetable {Id = 8, Name = "rukola", Price = 5.00}
            });

           /* modelBuilder.Entity<Pizza>().HasData(new Pizza[]
            {
                new Pizza {Id = 1, Name = "Margherita", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 1},
                new Pizza {Id = 2, Name = "Pepperoni", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 2},
                new Pizza {Id = 3, Name = "Capricciosa", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 1},
                new Pizza {Id = 4, Name = "Vegetariana", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 1},
                new Pizza {Id = 5, Name = "Pescatora", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 1},
                new Pizza {Id = 6, Name = "Al Pollo", SizeId = 1, DoughId = 1, SauceId = 3, CheeseId = 1},
                new Pizza {Id = 7, Name = "Diablo", SizeId = 1, DoughId = 1, SauceId = 3, CheeseId = 2},
                new Pizza {Id = 8, Name = "Vesuvio", SizeId = 1, DoughId = 1, SauceId = 4, CheeseId = 2},
                new Pizza {Id = 9, Name = "Chorizo", SizeId = 1, DoughId = 1, SauceId = 4, CheeseId = 1},
                new Pizza {Id = 10, Name = "Prosciutto", SizeId = 1, DoughId = 1, SauceId = 2, CheeseId = 1},
                new Pizza {Id = 11, Name = "Salami", SizeId = 1, DoughId = 1, SauceId = 2, CheeseId = 2},
                new Pizza {Id = 12, Name = "Carbonara", SizeId = 1, DoughId = 1, SauceId = 2, CheeseId = 1},
                new Pizza {Id = 13, Name = "Classico", SizeId = 1, DoughId = 1, SauceId = 3, CheeseId = 1},
                new Pizza {Id = 14, Name = "Bacon", SizeId = 1, DoughId = 1, SauceId = 1, CheeseId = 1},
                new Pizza {Id = 15, Name = "Farmerska", SizeId = 1, DoughId = 1, SauceId= 4, CheeseId = 1}
            });

            modelBuilder.Entity<MeatVegetable>().HasData(new MeatVegetable[]
            {
                new MeatVegetable {Id = 1, PizzaId = 1},
                new MeatVegetable {Id = 2, PizzaId = 2, MeatId = 1},
                new MeatVegetable {Id = 3, PizzaId = 3, MeatId = 2, VegetableId = 4},
                new MeatVegetable {Id = 4, PizzaId = 4, VegetableId = 1},
                new MeatVegetable {Id = 5, PizzaId = 4, VegetableId = 7},
                new MeatVegetable {Id = 6, PizzaId = 4, VegetableId = 5},
                new MeatVegetable {Id = 7, PizzaId = 4, VegetableId = 6},
                new MeatVegetable {Id = 8, PizzaId = 5, MeatId = 5 ,VegetableId = 4},
                new MeatVegetable {Id = 9, PizzaId = 5, VegetableId = 2},
                new MeatVegetable {Id = 10, PizzaId = 6, MeatId = 5,VegetableId = 6},
                new MeatVegetable {Id = 11, PizzaId = 7, VegetableId = 2},
                new MeatVegetable {Id = 12, PizzaId = 7, VegetableId = 3},
                new MeatVegetable {Id = 13, PizzaId = 8, MeatId = 2},
                new MeatVegetable {Id = 14, PizzaId = 9, MeatId = 4, VegetableId = 8},
                new MeatVegetable {Id = 15, PizzaId = 9, MeatId = 3},
                new MeatVegetable {Id = 16, PizzaId = 10, MeatId = 2, VegetableId = 4},
                new MeatVegetable {Id = 17, PizzaId = 11, MeatId = 6, VegetableId = 6},
                new MeatVegetable {Id = 19, PizzaId = 12, MeatId = 6, VegetableId = 4},
                new MeatVegetable {Id = 20, PizzaId = 12, VegetableId = 1},
                new MeatVegetable {Id = 21, PizzaId = 13, MeatId = 2, VegetableId = 5},
                new MeatVegetable {Id = 22, PizzaId = 13, MeatId = 1, VegetableId = 4},
                new MeatVegetable {Id = 23, PizzaId = 13, VegetableId = 2},
                new MeatVegetable {Id = 24, PizzaId = 13, VegetableId = 1},
                new MeatVegetable {Id = 25, PizzaId = 14, MeatId = 6, VegetableId = 7},
                new MeatVegetable {Id = 26, PizzaId = 14, VegetableId = 2},
                new MeatVegetable {Id = 27, PizzaId = 14, VegetableId = 6},
                new MeatVegetable {Id = 28, PizzaId = 15, MeatId = 5, VegetableId = 4},
                new MeatVegetable {Id = 29, PizzaId = 15, VegetableId = 5},
                new MeatVegetable {Id = 30, PizzaId = 15, VegetableId = 6},
           });*/

            modelBuilder.Entity<Status>().HasData(new Status[]
            {
                new Status {Id = 1, Name = "Przyjęte do realizacji"},
                new Status {Id = 2, Name = "W trakcie realizacji"},
                new Status {Id = 3, Name = "Zamówienie wysłane"},
            });
        }
    }
}