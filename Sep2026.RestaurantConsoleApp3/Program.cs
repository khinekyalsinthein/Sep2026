// See https://aka.ms/new-console-template for more information
// See https://aka.ms/new-console-template for more information

using Sep2026.RestaurantConsoleApp3;
using static System.Runtime.InteropServices.JavaScript.JSType;

OrderService orderService = new OrderService();
FoodService foodService = new FoodService();

Start:
Console.WriteLine("RESTAURANT ORDERING SYSTEM");
Console.WriteLine("1. Food Management");
Console.WriteLine("2. View Menu & Order Food");
Console.WriteLine("3. View Cart");
Console.WriteLine("4. Remove Item from Cart");
Console.WriteLine("5. Checkout & Calculate Bill");
Console.WriteLine("6. Exit");
Console.Write("Choose an option: ");
string option = Console.ReadLine();

switch (option)
{
    case "1":

        FoodManagement(foodService);
        break;

    case "2":

        bool hasFood = foodService.ShowFoods();

        if (hasFood is false)
        {
            break;
        }

        Console.Write("Please enter food ID: ");
        int foodId = ReadInt();

        Console.Write("Please enter quantity: ");
        int quantity = ReadInt();

        orderService.AddToCart(foodId, quantity);
        break;


    case "3":
        orderService.ShowCart();
        break;

    case "4":

        bool hasCart = orderService.ShowCart();

        if (hasCart is false)
        {
            break;
        }

        Console.Write("Enter Food ID to Remove (or 0 to cancel): ");
        int removeID = ReadInt();

        if (removeID != 0)
        {
            orderService.RemoveFromCart(removeID);
        }
        break;


    case "5":
        orderService.CheckOut();
        break;
    case "6":
        goto End;
    default:
        break;

}
goto Start;
Console.ReadLine();
End:
Console.WriteLine("Exiting app...");

static void FoodManagement(FoodService foodService)
{
    while (true)
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("             FOOD MANAGEMENT");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Add New Food");
        Console.WriteLine("2. View Food Menu");
        Console.WriteLine("3. Update Food");
        Console.WriteLine("4. Delete Food");
        Console.WriteLine("0. Back to Main Menu");
        Console.WriteLine("========================================");

        Console.Write("Choose an option: ");
        int choice = ReadInt();

        switch (choice)
        {
            case 1:
                Console.Write("Enter Food Name: ");
                string foodName = (Console.ReadLine() ?? "").Trim();

                Console.Write("Enter Price: ");
                decimal price = ReadDecimal();

                Console.Write("Enter Category: ");
                string category = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(foodName) || string.IsNullOrWhiteSpace(category) ||
                            price <= 0)
                {
                    Console.WriteLine("Please enter valid food information.");
                    break;
                }
                foodService.AddFood(foodName, price, category);
                break;

            case 2:
                foodService.ShowFoods();
                break;

            case 3:
                foodService.ShowFoods();

                Console.Write("\nEnter Food ID to update: ");
                int updateId = ReadInt();

                Console.Write("Enter New Food Name: ");
                string newName = Console.ReadLine() ?? "";

                Console.Write("Enter New Price: ");
                decimal newPrice = ReadDecimal();

                Console.Write("Enter New Category: ");
                string newCategory = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(newName) ||
                    string.IsNullOrWhiteSpace(newCategory) ||
                    newPrice <= 0)
                {
                    Console.WriteLine("Please enter valid food information.");
                    break;
                }

                foodService.UpdateFood(updateId, newName, newPrice, newCategory);
                break;

            case 4:
                foodService.ShowFoods();

                Console.Write("\nEnter Food ID to delete(or 0 to cancel): ");
                int deleteId = ReadInt();

                if (deleteId != 0)
                {
                    foodService.DeleteFood(deleteId);
                }
                break;

            case 0:
                return;

            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
}

static int ReadInt()
{
    int number;

    while (!int.TryParse(Console.ReadLine(), out number))
    {
        Console.Write("Please enter a valid number: ");
    }

    return number;
}

static decimal ReadDecimal()
{
    decimal number;

    while (!decimal.TryParse(Console.ReadLine(), out number))
    {
        Console.Write("Please enter a valid price: ");
    }

    return number;
}

