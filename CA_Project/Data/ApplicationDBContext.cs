using System;
using Microsoft.EntityFrameworkCore;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CA_Project.Data
{
    public class ApplicationDBContext : DbContext
    {
        public static string connectionString = "Server=localhost;Database=CAProject2;User Id=practiceuser;Password=password";
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        

        protected override void OnModelCreating(ModelBuilder model)
        {
           /* model.Entity<CartProduct>().HasNoKey();*/
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
