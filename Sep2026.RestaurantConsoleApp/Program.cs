// See https://aka.ms/new-console-template for more information

using Sep2026.RestaurantConsoleApp;

OrderService orderService = new OrderService();
FoodService foodService = new FoodService();

Start:
Console.WriteLine("RESTAURANT ORDERING SYSTEM");
Console.WriteLine("1. View Menu & Order Food");
Console.WriteLine("2. View Cart");
Console.WriteLine("3. Remove Item from Cart");
Console.WriteLine("4. Checkout & Print Bill");
Console.WriteLine("5. Exit");
Console.Write("Choose an option: ");
string option = Console.ReadLine();

switch (option)
{
    case "1":
        foodService.DisplayMenu();
        Console.Write("Please enter food ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Please enter quantity: ");
        int quantity = int.Parse(Console.ReadLine());

        orderService.AddToCart(id, quantity);
        break;
    case "2":
        orderService.DisplayCart();
        break;
    case "3":
        orderService.DisplayCart();
        if (orderService.IsCartEmpty)
        {
            break;
        }
        Console.Write("Enter Food ID to Remove (or 0 to Cancel): ");

        int removeID = Convert.ToInt32(Console.ReadLine());

        if (removeID != 0)
        {

            orderService.RemoveFromCart(removeID);
        }
        else
        {
            break;
        }
        break;
    case "4":
        orderService.CheckOut();
        break;
    case "5":
        goto End;
    default:
        break;

}
goto Start;
Console.ReadLine();
End:
Console.WriteLine("Exiting app...");
