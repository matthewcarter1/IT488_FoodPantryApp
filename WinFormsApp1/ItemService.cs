using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

                // Add a tracking record with the existing quantity
                var tracking = new Tracking
                {
                    TrackingDate = DateTime.Now,
                    Quantity = existingItem.Quantity,
                    Item = existingItem
                };
                _dbContext.Tracking.Add(tracking);
            }
            // If the item does not exist, add it to the database
            else
            {
                _dbContext.Items.Add(item);

                // Add a tracking record with the new item's quantity
                var tracking = new Tracking
                {
                    TrackingDate = DateTime.Now,
                    Quantity = item.Quantity,
                    Item = item
                };
                _dbContext.Tracking.Add(tracking);
            }

            // Save the changes to the database
            _dbContext.SaveChanges();
        }



        // Method to modify an existing item in the database
        public void ModifyItem(int itemID, string itemName, string itemDescription, string categoryName, decimal quantity, string unit, DateTime expiration)
        {
            // Find the item in the database
            var existingItem = _dbContext.Items.Include(i => i.Category).FirstOrDefault(i => i.ItemID == itemID);

            // If the item is not found, throw an exception
            if (existingItem == null)
            {
                throw new Exception("Item not found.");
            }

            // Update the properties of the existing item
            existingItem.ItemName = itemName;
            existingItem.ItemDescription = itemDescription;
            existingItem.Quantity = quantity;
            existingItem.Unit = unit;
            existingItem.Expiration = expiration;

            // Check if the category already exists in the database
            var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

            // If the category does not exist, add it to the database
            if (existingCategory == null)
            {
                existingCategory = new Category { CategoryName = categoryName };
                _dbContext.Categories.Add(existingCategory);
                _dbContext.SaveChanges();
            }

            // Update the category property of the existing item to use the existing category ID
            existingItem.Category = existingCategory;

            // Create a new tracking entry
            var tracking = new Tracking
            {
                TrackingDate = DateTime.Now,
                Quantity = quantity,
                Item = existingItem
            };
            _dbContext.Tracking.Add(tracking);

            // Save the changes to the database
            _dbContext.SaveChanges();
        }


        public void RemoveItem(int itemId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var item = _dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);

                    // Check if the item exists in the database
                    if (item == null)
                    {
                        throw new Exception("Item not found in the database.");
                    }

                    // Remove all tracking entries associated with the item
                    var trackingEntries = _dbContext.Tracking.Where(t => t.ItemID == itemId);
                    _dbContext.Tracking.RemoveRange(trackingEntries);

                    // Remove the item
                    _dbContext.Items.Remove(item);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while trying to delete the item from the database.", ex);
                }
            }
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
                        Expiration = i.Expiration,
                        Category = new Category { CategoryName = i.Category.CategoryName }, 
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