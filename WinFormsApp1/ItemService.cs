using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace WinFormsApp1
{
    // Class definition for the ItemService
    public class ItemService
    {
        // Private field to store the database context
        private readonly FoodPantryDbContext _dbContext;

        // Constructor that takes in the database context and stores it in the private field
        public ItemService(FoodPantryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to add a new item to the database
        public void AddItem(Item item)
        {
            // Check if the category already exists in the database
            var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == item.Category.CategoryName);

            // If the category does not exist, add it to the database
            if (existingCategory == null)
            {
                existingCategory = new Category { CategoryName = item.Category.CategoryName };
                _dbContext.Categories.Add(existingCategory);
                _dbContext.SaveChanges();
            }

            // Update the category property of the item to use the existing category ID
            item.Category = existingCategory;

            // Check if the item already exists in the database
            var existingItem = _dbContext.Items.FirstOrDefault(i => i.ItemName == item.ItemName);

            // If the item already exists, update the quantity
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            // If the item does not exist, add it to the database
            else
            {
                _dbContext.Items.Add(item);
            }

            // Save the changes to the database
            _dbContext.SaveChanges();
        }

        // Method to retrieve all items from the database
        public IEnumerable<Item> GetItems()
        {
            try
            {
                // Return a list of items including the category information
                return _dbContext.Items
                    .Include(i => i.Category)
                    .Select(i => new Item
                    {
                        ItemID = i.ItemID,
                        ItemName = i.ItemName,
                        ItemDescription = i.ItemDescription,
                        CategoryID = i.CategoryID,
                        Quantity = i.Quantity,
                        Unit = i.Unit,
                        Category = new Category { CategoryName = i.Category.CategoryName }
                    }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred while retrieving items from the database: " + ex.Message);
                return null;
            }
        }
    }
}
