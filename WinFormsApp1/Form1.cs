using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly FoodPantryDbContext _dbContext;
        public Form1()
        {
            InitializeComponent();
            _dbContext = new FoodPantryDbContext();
            LoadItems();
            comboBoxUnit.Items.Add("grams");
            comboBoxUnit.Items.Add("kilograms");
            comboBoxUnit.Items.Add("pounds");
            comboBoxUnit.Items.Add("ounces");
            comboBoxUnit.Items.Add("milliliters");
            comboBoxUnit.Items.Add("liters");
            comboBoxUnit.SelectedIndex = 0; // set the default selected item
        }

        private void LoadItems()
        {
            var itemService = new ItemService(_dbContext);
            var items = itemService.GetItems();
            dataGridView1.DataSource = items.Select(i => new {
                i.ItemID,
                i.ItemName,
                i.ItemDescription,
                CategoryName = i.Category.CategoryName,
                CategoryID = i.Category.CategoryID,
                i.Quantity,
                i.Unit,
                //Expiration = DateTime.ParseExact(i.Expiration, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                i.Expiration
            }).ToList();
            dataGridView1.Columns["ItemID"].Visible = false;
            dataGridView1.Columns["CategoryID"].Visible = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemNameTextBox.Text) || string.IsNullOrWhiteSpace(descriptionRichTextBox.Text) || string.IsNullOrWhiteSpace(categoryTextBox.Text) || string.IsNullOrWhiteSpace(quantityTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }
            int quantity;
            if (!int.TryParse(quantityTextBox.Text, out quantity))
            {
                MessageBox.Show("Quantity must be a valid integer.");
                return;
            }

            var itemService = new ItemService(_dbContext);

            var item = new Item
            {
                ItemName = itemNameTextBox.Text,
                ItemDescription = descriptionRichTextBox.Text,
                CategoryID = GetCategoryID(categoryTextBox.Text),
                Category = new Category { CategoryName = categoryTextBox.Text },
                Quantity = quantity,
                Unit = comboBoxUnit.SelectedItem.ToString(),
                Expiration = dateTimePicker1.Value
            };
            try
            {
                itemService.AddItem(item);
                LoadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the item: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null)
                {
                    MessageBox.Show("Please select an item to update.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(itemNameTextBox.Text))
                {
                    MessageBox.Show("Item name is required.");
                    return;
                }

                if (!int.TryParse(quantityTextBox.Text, out int quantity))
                {
                    MessageBox.Show("Invalid quantity. Please enter a valid number.");
                    return;
                }

                using (var dbContext = new FoodPantryDbContext())
                {
                    var itemService = new ItemService(dbContext);
                    int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                    int itemId = (int)selectedRow.Cells["ItemID"].Value;
                    var item = dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);
                    item.ItemName = itemNameTextBox.Text;
                    item.ItemDescription = descriptionRichTextBox.Text;
                    item.CategoryID = GetCategoryID(categoryTextBox.Text);
                    item.Quantity = quantity;
                    item.Unit = comboBoxUnit.SelectedItem.ToString();
                    dbContext.SaveChanges();
                    LoadItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the item. " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
            int itemId;

            // Validate that the selected row contains a valid item ID
            try
            {
                itemId = (int)selectedRow.Cells["ItemID"].Value;
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show("Invalid item ID. Please try again.");
                return;
            }

            try
            {
                using (var dbContext = new FoodPantryDbContext())
                {
                    var itemService = new ItemService(dbContext);
                    var item = dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);

                    // Check if the item exists in the database
                    if (item == null)
                    {
                        MessageBox.Show("Item not found in the database.");
                        return;
                    }

                    dbContext.Items.Remove(item);
                    dbContext.SaveChanges();
                    LoadItems();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while trying to delete the item from the database.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("You are about to delete all of the items and categories in the database. Are you sure you want to proceed?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (var ctx = new FoodPantryDbContext())
                    {
                        ctx.Database.ExecuteSqlRaw("EXEC sp_resetPantryData");
                        ctx.SaveChanges();
                    }
                    LoadItems();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while deleting the data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int GetCategoryID(string categoryName)
        {
            // Validate the input
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            // Get the category from the database
            Category category = null;
            try
            {
                category = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while accessing the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            // If the category exists, return the ID
            if (category != null)
            {
                return category.CategoryID;
            }

            // If the category does not exist, add it to the database and return the new ID
            Category newCategory = new Category { CategoryName = categoryName };
            try
            {
                _dbContext.Categories.Add(newCategory);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("An error occurred while updating the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            return newCategory.CategoryID;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int selectedRowIndex = e.RowIndex;
                if (selectedRowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                    itemNameTextBox.Text = selectedRow.Cells["ItemName"].Value?.ToString() ?? string.Empty;
                    descriptionRichTextBox.Text = selectedRow.Cells["ItemDescription"].Value?.ToString() ?? string.Empty;
                    categoryTextBox.Text = selectedRow.Cells["CategoryName"].Value?.ToString() ?? string.Empty;

                    // validate the value in Quantity column before parsing it to integer
                    if (int.TryParse(selectedRow.Cells["Quantity"].Value?.ToString(), out int quantity))
                    {
                        quantityTextBox.Text = quantity.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Invalid value for Quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        quantityTextBox.Text = string.Empty;
                    }

                    comboBoxUnit.SelectedItem = selectedRow.Cells["Unit"].Value?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int selectedRowIndex = e.RowIndex;
                if (selectedRowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                    itemNameTextBox.Text = selectedRow.Cells["ItemName"].Value.ToString();
                    descriptionRichTextBox.Text = selectedRow.Cells["ItemDescription"].Value.ToString();
                    categoryTextBox.Text = selectedRow.Cells["CategoryName"].Value.ToString();

                    if (!int.TryParse(selectedRow.Cells["Quantity"].Value.ToString(), out int quantity))
                    {
                        throw new Exception("Invalid quantity value");
                    }
                    quantityTextBox.Text = quantity.ToString();

                    var unit = selectedRow.Cells["Unit"].Value.ToString();
                    if (!comboBoxUnit.Items.Contains(unit))
                    {
                        throw new Exception("Invalid unit value");
                    }
                    comboBoxUnit.SelectedItem = unit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}