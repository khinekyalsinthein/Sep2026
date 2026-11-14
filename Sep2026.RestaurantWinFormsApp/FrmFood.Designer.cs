namespace Sep2026.RestaurantWinFormsApp
{
    partial class FrmFood
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            label1 = new Label();
            txtFoodName = new TextBox();
            txtPrice = new TextBox();
            label2 = new Label();
            txtCategory = new TextBox();
            label3 = new Label();
            orderDetailBindingSource = new BindingSource(components);
            dataGridView1 = new DataGridView();
            btnView = new Button();
            txtFoodID = new TextBox();
            label4 = new Label();
            btnAddOrder = new Button();
            btnCheckOut = new Button();
            dataGridView2 = new DataGridView();
            label5 = new Label();
            label7 = new Label();
            txtID = new TextBox();
            txtQuantity = new TextBox();
            label6 = new Label();
            label8 = new Label();
            btnRemoveFood = new Button();
            ((System.ComponentModel.ISupportInitialize)orderDetailBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(315, 217);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 42);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(315, 281);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(121, 42);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(159, 281);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(116, 42);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(553, 246);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(114, 42);
            btnClear.TabIndex = 3;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(177, 96);
            label1.Name = "label1";
            label1.Size = new Size(87, 20);
            label1.TabIndex = 4;
            label1.Text = "Food Name";
            // 
            // txtFoodName
            // 
            txtFoodName.Location = new Point(287, 89);
            txtFoodName.Name = "txtFoodName";
            txtFoodName.Size = new Size(149, 27);
            txtFoodName.TabIndex = 5;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(287, 133);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(149, 27);
            txtPrice.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(213, 133);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 6;
            label2.Text = "Price";
            // 
            // txtCategory
            // 
            txtCategory.Location = new Point(287, 172);
            txtCategory.Name = "txtCategory";
            txtCategory.Size = new Size(149, 27);
            txtCategory.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(195, 175);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 8;
            label3.Text = "Category";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(50, 340);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(512, 279);
            dataGridView1.TabIndex = 10;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btnView
            // 
            btnView.Location = new Point(155, 217);
            btnView.Name = "btnView";
            btnView.Size = new Size(120, 42);
            btnView.TabIndex = 11;
            btnView.Text = "View";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += btnView_Click;
            // 
            // txtFoodID
            // 
            txtFoodID.Location = new Point(287, 46);
            txtFoodID.Name = "txtFoodID";
            txtFoodID.Size = new Size(149, 27);
            txtFoodID.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(194, 49);
            label4.Name = "label4";
            label4.Size = new Size(60, 20);
            label4.TabIndex = 12;
            label4.Text = "Food Id";
            // 
            // btnAddOrder
            // 
            btnAddOrder.Location = new Point(729, 249);
            btnAddOrder.Name = "btnAddOrder";
            btnAddOrder.Size = new Size(104, 42);
            btnAddOrder.TabIndex = 14;
            btnAddOrder.Text = "Add Order";
            btnAddOrder.UseVisualStyleBackColor = true;
            btnAddOrder.Click += btnAddOrder_Click;
            // 
            // btnCheckOut
            // 
            btnCheckOut.Location = new Point(999, 248);
            btnCheckOut.Name = "btnCheckOut";
            btnCheckOut.Size = new Size(115, 43);
            btnCheckOut.TabIndex = 15;
            btnCheckOut.Text = "Check Out";
            btnCheckOut.UseVisualStyleBackColor = true;
            btnCheckOut.Click += btnCheckOut_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(665, 340);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(489, 279);
            dataGridView2.TabIndex = 16;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(195, 9);
            label5.Name = "label5";
            label5.Size = new Size(189, 28);
            label5.TabIndex = 21;
            label5.Text = "Food Management";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(800, 25);
            label7.Name = "label7";
            label7.Size = new Size(118, 28);
            label7.TabIndex = 22;
            label7.Text = "Order Food";
            // 
            // txtID
            // 
            txtID.Location = new Point(861, 99);
            txtID.Name = "txtID";
            txtID.Size = new Size(149, 27);
            txtID.TabIndex = 23;
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(861, 157);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(148, 27);
            txtQuantity.TabIndex = 24;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(784, 106);
            label6.Name = "label6";
            label6.Size = new Size(62, 20);
            label6.TabIndex = 25;
            label6.Text = "Food ID";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(781, 164);
            label8.Name = "label8";
            label8.Size = new Size(65, 20);
            label8.TabIndex = 26;
            label8.Text = "Quantity";
            // 
            // btnRemoveFood
            // 
            btnRemoveFood.Location = new Point(861, 248);
            btnRemoveFood.Name = "btnRemoveFood";
            btnRemoveFood.Size = new Size(111, 43);
            btnRemoveFood.TabIndex = 27;
            btnRemoveFood.Text = "Remove Food";
            btnRemoveFood.UseVisualStyleBackColor = true;
            btnRemoveFood.Click += btnRemoveFood_Click;
            // 
            // FrmFood
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1233, 640);
            Controls.Add(btnRemoveFood);
            Controls.Add(label8);
            Controls.Add(label6);
            Controls.Add(txtQuantity);
            Controls.Add(txtID);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(dataGridView2);
            Controls.Add(btnCheckOut);
            Controls.Add(btnAddOrder);
            Controls.Add(txtFoodID);
            Controls.Add(label4);
            Controls.Add(btnView);
            Controls.Add(dataGridView1);
            Controls.Add(txtCategory);
            Controls.Add(label3);
            Controls.Add(txtPrice);
            Controls.Add(label2);
            Controls.Add(txtFoodName);
            Controls.Add(label1);
            Controls.Add(btnClear);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Name = "FrmFood";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Restaurent Ordering System";
            ((System.ComponentModel.ISupportInitialize)orderDetailBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label label1;
        private TextBox txtFoodName;
        private TextBox txtPrice;
        private Label label2;
        private TextBox txtCategory;
        private Label label3;
        private BindingSource orderDetailBindingSource;
        private DataGridView dataGridView1;
        private Button btnView;
        private TextBox txtFoodID;
        private Label label4;
        private Button btnAddOrder;
        private Button btnCheckOut;
        private DataGridView dataGridView2;
        private Label label5;
        private Label label7;
        private TextBox txtID;
        private TextBox txtQuantity;
        private Label label6;
        private Label label8;
        private Button btnRemoveFood;
    }
}
