using Microsoft.EntityFrameworkCore;

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
            comboBoxUnit.SelectedIndex = 0; // set the default selected item
        }

        private void LoadItems()
        {
            var itemService = new ItemService(_dbContext);
            var items = itemService.GetItems();
            dataGridView1.DataSource = items.Select(i => new { i.ItemID, i.ItemName, i.ItemDescription, Category = i.Category.CategoryName, i.Quantity, i.Unit }).ToList();
            dataGridView1.Columns["ItemID"].Visible = false;
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
                // Add the new category to the database
                var newCategory = new Category { CategoryName = categoryName };
                _dbContext.Categories.Add(newCategory);
                _dbContext.SaveChanges();
                return newCategory.CategoryID;
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
                textBox2.Text = selectedRow.Cells["Category"].Value.ToString();
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
    }
}