using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage;

namespace WinFormsApp1
{
    // Class that defines the FoodPantryDB database context
    public class FoodPantryDbContext : DbContext
    {
        // Override method that sets up the connection to the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set up the connection string using SQL Server
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-IU3R9KA\\SQLEXPRESS;Initial Catalog=FoodPantryDB;User Id=FoodPantry;Password=PantryFood;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Data Source=LANDO\\SQLEXPRESS;Initial Catalog=FoodPantryDB;User Id=FoodPantry;Password=PantryFood;TrustServerCertificate=True;");
        }

        // DbSet properties that represent the tables in the database
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }

    // Class that represents the Item table in the database
    public class Item
    {
        // Properties of the Item class
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int CategoryID { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        // Navigation property for the relationship with the Category table
        public Category Category { get; set; }

        // Default constructor
        public Item()
        {
            ItemName = "";
            ItemDescription = "";
            Unit = "";
            Category = new Category();
        }
    }

    // Class that represents the Category table in the database
    public class Category
    {
        // Properties of the Category class
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        // Navigation property for the relationship with the Item table
        public ICollection<Item> Items { get; set; }

        // Default constructor
        public Category()
        {
            CategoryName = string.Empty;
        }
    }
}
