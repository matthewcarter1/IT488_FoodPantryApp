using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
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
        private PrintDocument pd = new PrintDocument();

        // Add a private variable to store the font to use for printing
        private Font printFont = new Font("Arial", 12);
        public Form2()
        {
            InitializeComponent();

            // Set the PrintPage event handler for the PrintDocument object
            pd.PrintPage += new PrintPageEventHandler(PrintPage);

            shoppingList.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn itemColumn = new DataGridViewTextBoxColumn();
            itemColumn.Name = "Item";
            itemColumn.DataPropertyName = "Ingredient";
            shoppingList.Columns.Add(itemColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.Name = "Quantity";
            quantityColumn.DataPropertyName = "Quantity";
            quantityColumn.DefaultCellStyle.Format = "N2"; // Round, dammit!
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

            // Register the Click event of the ClearListButton
            ClearSList.Click += new EventHandler(ClearSList_Click);
        }

        public class ShoppingListItem
        {
            public string Ingredient { get; set; }
            public decimal Quantity { get; set; }
            public string Unit { get; set; }
        }

        private List<ShoppingListItem> shoppingListTemp = new List<ShoppingListItem>();

        private void AddRecipe_Click(object sender, EventArgs e)
        {
            _dbContext = new FoodPantryDbContext();
            // Get the ingredient and quantity values from the text boxes
            string ingredient = ingredientTextBox.Text;
            decimal quantity = Convert.ToDecimal(qtyTextBox.Text);
            string requestedUnit = comboBox1.SelectedItem.ToString();

            // Check if the ingredient is already in the pantry
            var pantryItem = _dbContext.Items.FirstOrDefault(i => i.ItemName.ToLower() == ingredient.ToLower());

            if (pantryItem != null) // Pantry item exists
            {
                // Get the unit in which the item is stored in the database
                string storedUnit = pantryItem.Unit.ToLower();

                // Convert the requested quantity to the unit stored in the database, if necessary
                if (requestedUnit.ToLower() != storedUnit)
                {
                    decimal conversionFactor = ConvertToUnit(1, requestedUnit, storedUnit);
                    quantity = quantity * conversionFactor;
                }

                // Check if enough is available in the pantry
                if (pantryItem.Quantity >= quantity)
                {
                    // Prompt the user to confirm if the item can be removed from the pantry
                    var confirmResult = MessageBox.Show($"The ingredient {ingredient} is already in the pantry. Can it be removed?", "Confirm Remove from Pantry", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Subtract the ingredients from the pantry
                        pantryItem.Quantity = pantryItem.Quantity - quantity;
                        _dbContext.SaveChanges();

                        // Refresh the dataGridView1 on Form1
                        var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        form1?.LoadItems();
                    }
                }
                else
                {
                    // Prompt the user to confirm if the available items in the pantry can be used
                    var confirmResult = MessageBox.Show($"The ingredient {ingredient} is already in the pantry, but only {pantryItem.Quantity} {pantryItem.Unit} is available. Can it be used?", "Confirm Use from Pantry", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Subtract the available ingredients from the requested quantity
                        decimal remainingQuantity = quantity - pantryItem.Quantity;

                        // Remove the item from the pantry
                        _dbContext.Items.Remove(pantryItem);
                        _dbContext.SaveChanges();

                        // Refresh the dataGridView1 on Form1
                        var form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                        form1?.LoadItems();

                        // Check if the item already exists in the shopping list
                        var existingItem = shoppingListTemp.FirstOrDefault(i => i.Ingredient.ToLower() == ingredient.ToLower());

                        if (existingItem != null)
                        {
                            if (existingItem.Unit.ToLower() == storedUnit)
                            {
                                // Units match, update the quantity
                                existingItem.Quantity = existingItem.Quantity + remainingQuantity;
                                MessageBox.Show($"Added {remainingQuantity} {storedUnit} of {ingredient} to the shopping list.");
                            }
                            else
                            {
                                // Units don't match, convert the quantity to the correct unit and update the quantity
                                decimal convertedQuantity = ConvertToUnit(remainingQuantity, storedUnit, existingItem.Unit.ToLower());
                                existingItem.Quantity = existingItem.Quantity + convertedQuantity;
                                MessageBox.Show($"Added {convertedQuantity} {existingItem.Unit.ToLower()} of {ingredient} to the shopping list.");
                            }
                        }
                        else
                        {
                            // Add the remaining quantity to the shopping list
                            var newItem = new ShoppingListItem { Ingredient = ingredient, Quantity = remainingQuantity, Unit = storedUnit };
                            shoppingListTemp.Add(newItem);
                            MessageBox.Show($"Added {remainingQuantity} {storedUnit} of {ingredient} to the shopping list.");
                        }
                    }

                    // Update the binding source
                    shoppingListBindingSource.DataSource = shoppingListTemp;
                    shoppingListBindingSource.ResetBindings(false);

                    // Clear the text boxes
                    ingredientTextBox.Text = "";
                    qtyTextBox.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
            }
            else
            {
                if (pantryItem == null) // Item not in pantry
                {
                    // Check if the item is already on the shopping list
                    var shoppingListItem = shoppingListTemp.FirstOrDefault(i => i.Ingredient.ToLower() == ingredient.ToLower());

                    if (shoppingListItem != null) // Item already on shopping list
                    {
                        if (shoppingListItem.Unit.ToLower() == requestedUnit.ToLower())
                        {
                            // Units match, update the quantity
                            shoppingListItem.Quantity = shoppingListItem.Quantity + quantity;
                            MessageBox.Show($"Added {quantity} {requestedUnit} of {ingredient} to the shopping list.");
                        }
                        else
                        {
                            // Units don't match, convert the quantity to the correct unit and update the quantity
                            decimal convertedQuantity = ConvertToUnit(quantity, requestedUnit.ToLower(), shoppingListItem.Unit.ToLower());
                            shoppingListItem.Quantity = shoppingListItem.Quantity + convertedQuantity;
                            MessageBox.Show($"Added {convertedQuantity} {shoppingListItem.Unit.ToLower()} of {ingredient} to the shopping list.");
                        }
                    }
                    else
                    {
                        // Add the requested quantity to the shopping list
                        var newItem = new ShoppingListItem { Ingredient = ingredient, Quantity = quantity, Unit = requestedUnit };
                        shoppingListTemp.Add(newItem);
                        MessageBox.Show($"Added {quantity} {requestedUnit} of {ingredient} to the shopping list.");
                    }
                    // Update the binding source
                    shoppingListBindingSource.DataSource = shoppingListTemp;
                    shoppingListBindingSource.ResetBindings(false);

                    // Clear the text boxes
                    ingredientTextBox.Text = "";
                    qtyTextBox.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        // Method to convert the quantity to milliliters
        private decimal ConvertToMilliliters(decimal quantity, string unit)
        {
            switch (unit.ToLower())
            {
                case "grams":
                    return quantity * 1.0m;
                case "kilograms":
                    return quantity * 1000.0m;
                case "pounds":
                    return quantity * 453.592m;
                case "ounces":
                    return quantity * 28.3495m;
                case "liters":
                    return quantity * 1000.0m;
                default:
                    return quantity;
            }
        }

        // Method to convert the quantity from milliliters to the given unit
        private decimal ConvertFromMilliliters(decimal quantity, string unit)
        {
            switch (unit.ToLower())
            {
                case "grams":
                    return quantity / 1.0m;
                case "kilograms":
                    return quantity / 1000.0m;
                case "pounds":
                    return quantity / 453.592m;
                case "ounces":
                    return quantity / 28.3495m;
                case "liters":
                    return quantity / 1000.0m;
                default:
                    return quantity;
            }
        }

        // Method to convert the quantity to the given unit
        private decimal ConvertToUnit(decimal quantity, string fromUnit, string toUnit)
        {
            // Convert the quantity to milliliters
            decimal quantityInMilliliters = ConvertToMilliliters(quantity, fromUnit);
            // Convert the quantity from milliliters to the given unit
            decimal convertedQuantity = ConvertFromMilliliters(quantityInMilliliters, toUnit);

            return convertedQuantity;

        }
        private void ClearRecipe_Click(object sender, EventArgs e)
        {
            // Clear the text boxes
            ingredientTextBox.Text = "";
            qtyTextBox.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void PrintSL_Click(object sender, EventArgs e)
        {
            // Get the shopping list items from the data source
            List<ShoppingListItem> items = shoppingListBindingSource.DataSource as List<ShoppingListItem>;

            // Check if there are items in the shopping list
            if (items != null && items.Any())
            {
                // Call the Print method of the PrintDocument object
                pd.Print();
            }
            else
            {
                MessageBox.Show("There are no items in the shopping list to print.");
            }
        }


        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Get the shopping list items from the data source
            List<ShoppingListItem> items = shoppingListBindingSource.DataSource as List<ShoppingListItem>;

            if (items != null && items.Any())
            {
                // Set the x and y coordinates for drawing the text
                float x = 100;
                float y = 100;

                // Draw the shopping list items on the printed page
                foreach (ShoppingListItem item in items)
                {
                    string line = string.Format("{0} {1} {2}", item.Quantity, item.Unit, item.Ingredient);
                    e.Graphics.DrawString(line, printFont, Brushes.Black, x, y);
                    y += printFont.GetHeight();
                }
            }
            else
            {
                MessageBox.Show("There are no items in the shopping list to print.");
            }
        }

        private void ClearSList_Click(object sender, EventArgs e)
        {
                shoppingListTemp.Clear();

                // Update the binding source
                shoppingListBindingSource.DataSource = shoppingListTemp;
                shoppingListBindingSource.ResetBindings(false);
            }
        }
    }


