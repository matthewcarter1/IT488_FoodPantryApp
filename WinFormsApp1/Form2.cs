using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;

namespace IT488_FoodPantryApp
{
    public partial class Form2 : Form
    {
        private FoodPantryDbContext _dbContext;
        private BindingSource shoppingListBindingSource = new BindingSource();

        public Form2()
        {
            InitializeComponent();

            shoppingList.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn itemColumn = new DataGridViewTextBoxColumn();
            itemColumn.Name = "Item";
            itemColumn.DataPropertyName = "Ingredient";
            shoppingList.Columns.Add(itemColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.Name = "Quantity";
            quantityColumn.DataPropertyName = "Quantity";
            shoppingList.Columns.Add(quantityColumn);

            DataGridViewTextBoxColumn unitColumn = new DataGridViewTextBoxColumn();
            unitColumn.Name = "Unit";
            unitColumn.DataPropertyName = "Unit";
            shoppingList.Columns.Add(unitColumn);

            shoppingList.DataSource = shoppingListBindingSource;

            comboBox1.Items.Add("grams");
            comboBox1.Items.Add("kilograms");
            comboBox1.Items.Add("pounds");
            comboBox1.Items.Add("ounces");
            comboBox1.Items.Add("milliliters");
            comboBox1.Items.Add("liters");
        }

        public class ShoppingListItem
        {
            public string Ingredient { get; set; }
            public double Quantity { get; set; }
            public string Unit { get; set; }
        }

        private List<ShoppingListItem> shoppingListTemp = new List<ShoppingListItem>();

        private void AddRecipe_Click(object sender, EventArgs e)
        {
            _dbContext = new FoodPantryDbContext();
            // Get the ingredient and quantity values from the text boxes
            string ingredient = ingredientTextBox.Text;
            int quantity = Convert.ToInt32(qtyTextBox.Text);
            string unit = comboBox1.SelectedItem.ToString();

            // Check if the ingredient is already in the pantry
            var pantryItem = _dbContext.Items.FirstOrDefault(i => i.ItemName.ToLower() == ingredient.ToLower());

            if (pantryItem != null)
            {
                // Check if enough is available in the pantry
                if (pantryItem.Quantity >= quantity)
                {
                    // Prompt the user to confirm if the item can be removed from the pantry
                    var confirmResult = MessageBox.Show($"The ingredient {ingredient} is already in the pantry. Can it be removed?", "Confirm Remove from Pantry", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Subtract the ingredients from the pantry
                        pantryItem.Quantity -= quantity;
                        _dbContext.SaveChanges();

                        // Refresh the dataGridView1 on Form1
                        var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        form1?.LoadItems();
                    }
                }
                else
                {
                    // Prompt the user to confirm if the available items in the pantry can be used
                    var confirmResult = MessageBox.Show($"The ingredient {ingredient} is already in the pantry, but only {pantryItem.Quantity} {unit} is available. Can it be used?", "Confirm Use from Pantry", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Subtract the ingredients from the pantry
                        quantity -= pantryItem.Quantity;
                        _dbContext.Remove(pantryItem);
                        _dbContext.SaveChanges();

                        // Refresh the dataGridView1 on Form1
                        var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        form1?.LoadItems();
                    }

                    // Check if the item already exists in the shopping list
                    var existingItem = shoppingListTemp.FirstOrDefault(i => i.Ingredient.ToLower() == ingredient.ToLower());

                    if (existingItem != null)
                    {
                        // Update the quantity
                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        // Add the remaining quantity to the shopping list
                        var newItem = new ShoppingListItem { Ingredient = ingredient, Quantity = quantity, Unit = unit };
                        shoppingListTemp.Add(newItem);
                    }
                    MessageBox.Show($"Added {quantity} {unit} of {ingredient} to the shopping list.");
                }
            }
            else
            {
                // Check if the item already exists in the shopping list
                var existingItem = shoppingListTemp.FirstOrDefault(i => i.Ingredient.ToLower() == ingredient.ToLower());

                if (existingItem != null)
                {
                    // Update the quantity
                    existingItem.Quantity += quantity;
                }
                else
                {
                    // Add the item to the shopping list
                    var newItem = new ShoppingListItem { Ingredient = ingredient, Quantity = quantity, Unit = unit };
                    shoppingListTemp.Add(newItem);
                }
                MessageBox.Show($"Added {quantity} {unit} of {ingredient} to the shopping list.");
            }

            // Update the binding source
            shoppingListBindingSource.DataSource = shoppingListTemp;
            shoppingListBindingSource.ResetBindings(false);

            // Clear the text boxes
            ingredientTextBox.Text = "";
            qtyTextBox.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void ClearRecipe_Click(object sender, EventArgs e)
        {
            // Clear the text boxes
            ingredientTextBox.Text = "";
            qtyTextBox.Text = "";
            comboBox1.SelectedIndex = -1;
        }
    }
}