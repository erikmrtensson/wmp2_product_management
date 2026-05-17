using wmp2_product_management;

ProductManager manager = new ProductManager();

while (true)
{
    Console.WriteLine("===================================");
    Console.WriteLine("      PRODUCT MANAGEMENT SYSTEM   ");
    Console.WriteLine("===================================");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. Show Products");
    Console.WriteLine("3. Search Product");
    Console.WriteLine("4. Edit Product");
    Console.WriteLine("5. Delete Product");
    Console.WriteLine("6. Statistics");
    Console.WriteLine("7. Save Products");
    Console.WriteLine("8. Load Products");
    Console.WriteLine("9. Exit");
    Console.Write("\nSelect Option: ");

    string choice = Console.ReadLine() ?? string.Empty;
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            {
                manager.AddProduct();
                break;
            }
        case "2":
            {
                manager.ShowProducts();
                break;
            }
        case "3":
            {
                manager.SearchProduct();
                break;
            }
        case "4":
            {

                manager.UpdateProduct();
                break;
            }
        case "5":
            {

                manager.RemoveProduct();
                break;
            }
        case "6":
            {
                manager.ShowStatistics();
                break;
            }
        case "7":
            {
                manager.SaveToFile();
                break;
            }
        case "8":
            {
                manager.LoadFromFile();
                break;
            }
        case "9":
            {
                Console.WriteLine("Exiting application...");
                return;
            }
        default:
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ResetColor();
                break;
            }
    }
}
