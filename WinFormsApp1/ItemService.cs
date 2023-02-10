using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace WinFormsApp1
{
    public class ItemService
    {
        private readonly FoodPantryDbContext _dbContext;

        public ItemService(FoodPantryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddItem(Item item)
        {
            var existingItem = _dbContext.Items.FirstOrDefault(i => i.ItemName == item.ItemName);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _dbContext.Items.Add(item);
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<Item> GetItems()
        {
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
                    Category = new Category { CategoryName = i.Category.CategoryName }}).ToList();
        }
    }
}