using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace WinFormsApp1
{
    public class FoodPantryDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-IU3R9KA\\SQLEXPRESS;Initial Catalog=FoodPantryDB;Integrated Security=False;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-IU3R9KA\\SQLEXPRESS;Initial Catalog=FoodPantryDB;User Id=FoodPantry;Password=PantryFood;TrustServerCertificate=True;");

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
public class Item
{
    public int ItemID { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public int CategoryID { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }

    public Category Category { get; set; }

    public Item()
    {
        ItemName = "";
        ItemDescription = "";
        Unit = "";
        Category = new Category();
    }
}

public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }

    public ICollection<Item> Items { get; set; }
    public Category()
    {
        CategoryName = string.Empty;
    }
}
