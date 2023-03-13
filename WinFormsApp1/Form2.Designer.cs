namespace IT488_FoodPantryApp
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shoppingList = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.AddRecipe = new System.Windows.Forms.Button();
            this.ClearRecipe = new System.Windows.Forms.Button();
            this.ClearSList = new System.Windows.Forms.Button();
            this.PrintSL = new System.Windows.Forms.Button();
            this.ingredientTextBox = new System.Windows.Forms.TextBox();
            this.qtyTextBox = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.shoppingList)).BeginInit();
            this.SuspendLayout();
            // 
            // shoppingList
            // 
            this.shoppingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.shoppingList.Location = new System.Drawing.Point(577, 11);
            this.shoppingList.Name = "shoppingList";
            this.shoppingList.RowTemplate.Height = 25;
            this.shoppingList.Size = new System.Drawing.Size(444, 519);
            this.shoppingList.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(577, 533);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Shopping List";
            // 
            // AddRecipe
            // 
            this.AddRecipe.Location = new System.Drawing.Point(242, 189);
            this.AddRecipe.Name = "AddRecipe";
            this.AddRecipe.Size = new System.Drawing.Size(75, 23);
            this.AddRecipe.TabIndex = 4;
            this.AddRecipe.Text = "Add Ingredient";
            this.AddRecipe.UseVisualStyleBackColor = true;
            this.AddRecipe.Click += new System.EventHandler(this.AddRecipe_Click);
            // 
            // ClearRecipe
            // 
            this.ClearRecipe.Location = new System.Drawing.Point(55, 189);
            this.ClearRecipe.Name = "ClearRecipe";
            this.ClearRecipe.Size = new System.Drawing.Size(75, 23);
            this.ClearRecipe.TabIndex = 5;
            this.ClearRecipe.Text = "Clear";
            this.ClearRecipe.UseVisualStyleBackColor = true;
            this.ClearRecipe.Click += new System.EventHandler(this.ClearRecipe_Click);
            // 
            // ClearSList
            // 
            this.ClearSList.Location = new System.Drawing.Point(865, 536);
            this.ClearSList.Name = "ClearSList";
            this.ClearSList.Size = new System.Drawing.Size(75, 23);
            this.ClearSList.TabIndex = 6;
            this.ClearSList.Text = "Clear";
            this.ClearSList.UseVisualStyleBackColor = true;
            this.ClearSList.Click += new System.EventHandler(this.ClearSList_Click);
            // 
            // PrintSL
            // 
            this.PrintSL.Location = new System.Drawing.Point(946, 536);
            this.PrintSL.Name = "PrintSL";
            this.PrintSL.Size = new System.Drawing.Size(75, 23);
            this.PrintSL.TabIndex = 7;
            this.PrintSL.Text = "Print";
            this.PrintSL.UseVisualStyleBackColor = true;
            this.PrintSL.Click += new System.EventHandler(this.PrintSL_Click);
            // 
            // ingredientTextBox
            // 
            this.ingredientTextBox.Location = new System.Drawing.Point(55, 72);
            this.ingredientTextBox.Name = "ingredientTextBox";
            this.ingredientTextBox.Size = new System.Drawing.Size(262, 23);
            this.ingredientTextBox.TabIndex = 8;
            // 
            // qtyTextBox
            // 
            this.qtyTextBox.Location = new System.Drawing.Point(55, 130);
            this.qtyTextBox.Name = "qtyTextBox";
            this.qtyTextBox.Size = new System.Drawing.Size(75, 23);
            this.qtyTextBox.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(196, 130);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Ingredient";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Quantity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Unit";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 622);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.qtyTextBox);
            this.Controls.Add(this.ingredientTextBox);
            this.Controls.Add(this.PrintSL);
            this.Controls.Add(this.ClearSList);
            this.Controls.Add(this.ClearRecipe);
            this.Controls.Add(this.AddRecipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.shoppingList);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.shoppingList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataGridView shoppingList;
        private Label label2;
        private Button AddRecipe;
        private Button ClearRecipe;
        private Button ClearSList;
        private Button PrintSL;
        private TextBox ingredientTextBox;
        private TextBox qtyTextBox;
        private ComboBox comboBox1;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}