using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
            dataGridView1.DataSource = items.Select(i => new { i.ItemID, i.ItemName, i.ItemDescription, CategoryName = i.Category.CategoryName, CategoryID = i.Category.CategoryID, i.Quantity, i.Unit }).ToList();
            dataGridView1.Columns["ItemID"].Visible = false;
            dataGridView1.Columns["CategoryID"].Visible = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var itemService = new ItemService(_dbContext);
            var item = new Item
            {
                ItemName = textBox1.Text,
                ItemDescription = richTextBox1.Text,
                CategoryID = GetCategoryId(textBox2.Text),
                Quantity = int.Parse(textBox3.Text),
                Unit = comboBoxUnit.SelectedItem.ToString()
            };
            itemService.AddItem(item);
            LoadItems();
        }

        private int GetCategoryId(string categoryName)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
            if (category != null)
            {
                return category.CategoryID;
            }
            else
            {
                var newCategory = new Category { CategoryName = categoryName };
                MessageBox.Show(categoryName);
                _dbContext.Categories.Add(newCategory);
                MessageBox.Show(newCategory.CategoryName);
                _dbContext.SaveChanges();
                category = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
                category.CategoryID = category.CategoryID - 1;
                return category.CategoryID;
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            int selectedRowIndex = e.RowIndex;
            if (selectedRowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                textBox1.Text = selectedRow.Cells["ItemName"].Value.ToString();
                richTextBox1.Text = selectedRow.Cells["ItemDescription"].Value.ToString();
                textBox2.Text = selectedRow.Cells["CategoryName"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Quantity"].Value.ToString();
                comboBoxUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.RowIndex >= 0)
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];

                textBox1.Text = selectedRow.Cells["ItemName"].Value.ToString();
                richTextBox1.Text = selectedRow.Cells["ItemDescription"].Value.ToString();
                textBox2.Text = selectedRow.Cells["CategoryName"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Quantity"].Value.ToString();
                comboBoxUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString();
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["ItemName"].Value.ToString();
                richTextBox1.Text = selectedRow.Cells["ItemDescription"].Value.ToString();
                textBox2.Text = selectedRow.Cells["CategoryName"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Quantity"].Value.ToString();
                comboBoxUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = e.RowIndex;
            if (selectedRowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                textBox1.Text = selectedRow.Cells["ItemName"].Value.ToString();
                richTextBox1.Text = selectedRow.Cells["ItemDescription"].Value.ToString();
                textBox2.Text = selectedRow.Cells["CategoryName"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Quantity"].Value.ToString();
                comboBoxUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            using (var dbContext = new FoodPantryDbContext())
            {
                var itemService = new ItemService(dbContext);
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int itemId = (int)selectedRow.Cells["ItemID"].Value;
                var item = dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);
                item.ItemName = textBox1.Text;
                item.ItemDescription = richTextBox1.Text;
                item.CategoryID = GetCategoryId(textBox2.Text);
                item.Quantity = int.Parse(textBox3.Text);
                item.Unit = comboBoxUnit.SelectedItem.ToString();
                dbContext.SaveChanges();
                LoadItems();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var dbContext = new FoodPantryDbContext())
            {
                var itemService = new ItemService(dbContext);
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int itemId = (int)selectedRow.Cells["ItemID"].Value;
                var item = dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);
                dbContext.Items.Remove(item);
                dbContext.SaveChanges();
                LoadItems();
            }
        }

        private void button4_Click(object sender, EventArgs e)
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



    }

}
