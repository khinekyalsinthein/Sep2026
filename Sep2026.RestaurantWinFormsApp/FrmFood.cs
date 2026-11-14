
using Sep2026.ConsoleApp4.Database.AppDbContextModels;

namespace Sep2026.RestaurantWinFormsApp
{
    public partial class FrmFood : Form
    {
        public FrmFood()
        {
            InitializeComponent();
        }


        private void btnView_Click(object sender, EventArgs e)
        {

            try
            {
                AppDbContext db = new AppDbContext();

                dataGridView1.DataSource = db.Foods
                    .Where(x => x.IsAvailable == true)
                    .Select(x => new
                    {
                        x.FoodId,
                        x.FoodName,
                        x.Price,
                        x.Category
                    })
                    .ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AppDbContext db = new AppDbContext();

                if (string.IsNullOrWhiteSpace(txtFoodName.Text))
                {
                    MessageBox.Show("Please enter Food Name.");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Please enter a valid price.");
                    return;
                }

                string categoryName = txtCategory.Text.Trim();

                var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

                if (category is null)
                {
                    MessageBox.Show("Category Not Found!");
                    return;
                }

                db.Foods.Add(new Food
                {
                    FoodName = txtFoodName.Text.Trim(),
                    Price = price,
                    Category = category.CategoryName,
                    CategoryId = category.CategoryId,
                    CategoryNavigation = category,
                    IsAvailable = true
                });

                int result = db.SaveChanges();

                string message = result > 0 ? "Saving Successful." : "Saving Failed.";

                MessageBox.Show(message);

                ClearControls();

                LoadFoods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void ClearControls()
        {
            txtFoodID.Clear();
            txtFoodName.Clear();
            txtPrice.Clear();
            txtCategory.Clear();
            txtID.Clear();
            txtQuantity.Clear();

            txtFoodID.Focus();
          

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                AppDbContext db = new AppDbContext();

                if (string.IsNullOrWhiteSpace(txtFoodID.Text))
                {
                    MessageBox.Show("Please select a food.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFoodName.Text))
                {
                    MessageBox.Show("Please enter Food Name.");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Please enter a valid price.");
                    return;
                }

                if (!int.TryParse(txtFoodID.Text, out int id))
                {
                    MessageBox.Show("Please select a valid food.");
                    return;
                }

                Food food = db.Foods.FirstOrDefault(f => f.FoodId == id);

                if (food is null)
                {
                    MessageBox.Show("Food Not Found!");
                    return;
                }

                string categoryName = txtCategory.Text.Trim();

                var category = db.Categories.FirstOrDefault(
                    c => c.CategoryName == categoryName
                );

                if (category is null)
                {
                    MessageBox.Show("Category Not Found!");
                    return;
                }

                food.FoodName = txtFoodName.Text.Trim();
                food.Price = price;
                food.Category = category.CategoryName;
                food.CategoryId = category.CategoryId;

                int result = db.SaveChanges();

                string message = result > 0 ? "Updating Successful." : "No Changes Found.";

                MessageBox.Show(message);

                ClearControls();

                LoadFoods();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                AppDbContext db = new AppDbContext();

                if (!int.TryParse(txtFoodID.Text, out int id))
                {
                    MessageBox.Show("Please select a food.");
                    return;
                }

                Food food = db.Foods.FirstOrDefault(f => f.FoodId == id);

                if (food is null)
                {
                    MessageBox.Show("Food Not Found!");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to delete this food?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    food.IsAvailable = false;

                    db.SaveChanges();

                    MessageBox.Show("Deleting Successful.");

                    ClearControls();

                    LoadFoods();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFoods()
        {
            AppDbContext db = new AppDbContext();

            dataGridView1.DataSource = db.Foods
                .Where(x => x.IsAvailable == true)
                .Select(x => new
                {
                    x.FoodId,
                    x.FoodName,
                    x.Price,
                    x.Category
                })
                .ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtFoodID.Text = row.Cells["FoodId"].Value.ToString();
                txtFoodName.Text = row.Cells["FoodName"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {

            try
            {
                AppDbContext db = new AppDbContext();

                if (!int.TryParse(txtID.Text, out int foodId) ||
                    !int.TryParse(txtQuantity.Text, out int quantity))
                {
                    MessageBox.Show("Please enter Food ID and Quantity.");
                    return;
                }

                if (quantity <= 0)
                {
                    MessageBox.Show("Invalid Quantity!");
                    return;
                }

                Food food = db.Foods.FirstOrDefault(x => x.FoodId == foodId && x.IsAvailable == true);

                if (food is null)
                {
                    MessageBox.Show("Food Not Found!");
                    return;
                }

                Order order = db.Orders.FirstOrDefault(x => x.Status == "Pending");

                if (order is null)
                {
                    order = new Order
                    {
                        OrderDate = DateTime.Now,
                        Status = "Pending"
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();
                }

                OrderDetail detail = db.OrderDetails.FirstOrDefault(x => x.OrderId == order.OrderId && x.FoodId == foodId);

                if (detail != null)
                {
                    detail.Quantity += quantity;
                }
                else
                {
                    db.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        FoodId = food.FoodId,
                        Quantity = quantity,
                        FoodPrice = food.Price
                    });
                }

                int result = db.SaveChanges();

                MessageBox.Show(result > 0 ? "Food Added to Order Successfully." : "Adding Failed.");

                ShowCurrentOrder();
                ClearControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowCurrentOrder()
        {
            AppDbContext db = new AppDbContext();

            Order order = db.Orders.FirstOrDefault(x => x.Status == "Pending");

            if (order is null)
            {
                dataGridView2.DataSource = null;
                return;
            }

            dataGridView2.DataSource = db.OrderDetails
                .Where(x => x.OrderId == order.OrderId)
                .Select(x => new
                {
                    x.OrderDetailId,
                    x.FoodId,
                    x.Food.FoodName,
                    x.Quantity,
                    x.FoodPrice,
                    Total = x.Quantity * x.FoodPrice
                })
                .ToList();
        }

        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.CurrentRow == null)
                {
                    MessageBox.Show("Please select a food from the order.");
                    return;
                }

                int id = Convert.ToInt32(
                    dataGridView2.CurrentRow.Cells["OrderDetailId"].Value
                );

                AppDbContext db = new AppDbContext();

                OrderDetail detail = db.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id && x.Order.Status == "Pending");

                if (detail is null)
                {
                    MessageBox.Show("Food Not Found!");
                    return;
                }

                if (MessageBox.Show("Are you sure you want to remove this food?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.OrderDetails.Remove(detail);

                    db.SaveChanges();

                    MessageBox.Show("Food Removed Successfully.");

                    ShowCurrentOrder();
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {

            try
            {
                AppDbContext db = new AppDbContext();

                Order order = db.Orders.FirstOrDefault(x => x.Status == "Pending");

                if (order is null)
                {
                    MessageBox.Show("No Current Order!");
                    return;
                }

                var details = db.OrderDetails
                    .Where(x => x.OrderId == order.OrderId)
                    .Select(x => new
                    {
                        x.Food.FoodName,
                        x.Quantity,
                        x.FoodPrice
                    })
                    .ToList();

                if (details.Count == 0)
                {
                    MessageBox.Show("Your Order is Empty!");
                    return;
                }

                decimal subtotal = details.Sum(
                    x => x.Quantity * x.FoodPrice
                );

                decimal tax = subtotal * 0.05m;
                decimal grandTotal = subtotal + tax;

                string receipt = "RESTAURANT RECEIPT\n";
                receipt += "------------------------------\n";

                foreach (var item in details)
                {
                    decimal total = item.Quantity * item.FoodPrice;

                    receipt += $"{item.FoodName}\n";
                    receipt += $"{item.Quantity} x {item.FoodPrice:N0} = {total:N0} MMK\n";
                }

                receipt += "------------------------------\n";
                receipt += $"Subtotal : {subtotal:N0} MMK\n";
                receipt += $"Tax (5%) : {tax:N0} MMK\n";
                receipt += $"Grand Total : {grandTotal:N0} MMK\n";
                receipt += "------------------------------\n";
                receipt += "Thank You!";

                order.Status = "Completed";

                int result = db.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show(
                        receipt,
                        "Checkout Receipt"
                    );

                    dataGridView2.DataSource = null;
                }
                else
                {
                    MessageBox.Show("Checkout Failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      
    }
}

