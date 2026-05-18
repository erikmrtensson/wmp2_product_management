using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace wmp2_product_management
{
    internal class ProductManager
    {
        private List<Product> _products = new();
        private int _nextId = 1001;
        private const string FilePath = "products.json";

        public void AddProduct()
        {
            while (true)
            {
                Console.Write("Enter Category (or Q to stop): ");
                string category = Console.ReadLine()?.Trim() ?? "";

                if (category.ToLower() == "q")
                {
                    break;
                }

                if (string.IsNullOrEmpty(category)) 
                { 
                    PrintError("Category cannot be empty."); 
                    continue; 
                }

                Console.Write("Enter Product Name: ");
                string name = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(name)) 
                { 
                    PrintError("Product name cannot be empty.");
                    continue; 
                }

                decimal price = ReadPrice();

                _products.Add(new Product { Id = _nextId++, Category = category, Name = name, Price = price });
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product added successfully!\n");
                Console.ResetColor();
            }
        }

        public void ShowProducts()
        {
            if (_products.Count == 0)
            {
                PrintError("No products available.\n");
                return;
            }

            Console.WriteLine("\n========= PRODUCT LIST =========\n");


            foreach (var product in _products.OrderBy(p => p.Price))
            {
                Console.WriteLine($"{product.Id,-6}| {product.Category,-16}| {product.Name,-16}| {product.Price} kr"); // Aligned columns
            }

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine($"TOTAL PRICE: {CalculateTotal()} kr");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();
        }

        public void SearchProduct()
        {
            if (_products.Count == 0)
            {
                PrintError("No products available.\n");
                return;
            }

            Console.Write("Search by (1) Name  (2) Category: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            if (choice != "1" && choice != "2")
            {
                PrintError("Invalid option. Please enter 1 or 2.");
                return;
            }

            Console.Write("Search: ");
            string query = (Console.ReadLine() ?? "").Trim().ToLower();

            List<Product> results;

            if (choice == "2")
            {
                results = _products.Where(p => p.Category.ToLower().Contains(query)).ToList();
            }
            else
            {
                results = _products.Where(p => p.Name.ToLower().Contains(query)).ToList();
            }

            if (results.Count == 0) 
            { 
                PrintError("No products found."); 
                return; 
            }

            Console.WriteLine("\nFOUND PRODUCTS:");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var p in results)
            {
                Console.WriteLine($"{p.Id,-6}| {p.Category,-16}| {p.Name,-16}| {p.Price} kr");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public void UpdateProduct()
        {
            ShowProducts();
            if (_products.Count == 0)
            {
                return;
            }

            Console.Write("Enter Product ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) 
            { 
                PrintError("Invalid ID."); return; 
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null) 
            { 
                PrintError("Product not found."); 
                return; 
            }

            Console.Write($"New Category [{product.Category}]: ");
            string cat = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrEmpty(cat))
            {
                product.Category = cat;
            }

            Console.Write($"New Name [{product.Name}]: ");
            string name = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrEmpty(name))
            {
                product.Name = name;
            }

            Console.Write($"New Price [{product.Price}]: ");
            string priceInput = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrEmpty(priceInput))
            {
                if (!decimal.TryParse(priceInput, out decimal newPrice) || newPrice < 0)
                {
                    PrintError("Invalid price. Edit cancelled for price.");
                }
                else
                {
                    product.Price = newPrice;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Product updated.\n");
            Console.ResetColor();
        }

        public void RemoveProduct()
        {
            ShowProducts();
            if (_products.Count == 0)
            {
                return;
            }

            Console.Write("Enter Product ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) 
            { 
                PrintError("Invalid ID."); 
                return; 
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null) 
            { 
                PrintError("Product not found."); 
                return; 
            }

            _products.Remove(product);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nProduct '{product.Name}' deleted.\n");
            Console.ResetColor();
        }


        public void ShowStatistics()
        {

            if (_products.Count == 0)
            {
                PrintError("No products to show statistics for.");
                return;
            }

            var mostExpensive = _products.MaxBy(p => p.Price)!;
            var cheapest = _products.MinBy(p => p.Price)!;
            decimal average = _products.Average(p => p.Price);

            Console.WriteLine("\n========= STATISTICS =========\n");
            Console.WriteLine($"Most Expensive Product: \n{mostExpensive.Name} - {mostExpensive.Price} kr\n");
            Console.WriteLine($"Cheapest Product: \n{cheapest.Name} - {cheapest.Price} kr\n");
            Console.WriteLine($"Average Price: \n{average:F2} kr\n");
            Console.WriteLine("\nProducts per Category:");

            foreach (var group in _products.GroupBy(p => p.Category)) // Group products by category and count them
            {
                Console.WriteLine($"{group.Key}: {group.Count()}"); // Print category name and count of products in that category
            }

            Console.WriteLine("==============================\n");
        }


        public void SaveToFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string json = JsonSerializer.Serialize(_products, options);

            File.WriteAllText(FilePath, json);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Products saved to {FilePath}\n");
            Console.ResetColor();
        }

        public void LoadFromFile()
        {
            if (!File.Exists(FilePath)) 
            { 
                PrintError("Save file not found."); 
                return; 
            }

            string json = File.ReadAllText(FilePath);

            List<Product> loadedProducts = JsonSerializer.Deserialize<List<Product>>(json);

            if (loadedProducts != null)
            {
                _products = loadedProducts;
            }
            else
            {
                _products = new List<Product>();
            }

            if (_products.Count > 0)
            {
                int highestId = _products.Max(p => p.Id);
                _nextId = highestId + 1;
            }
            else
            {
                _nextId = 1001;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded {_products.Count} product(s).\n");
            Console.ResetColor();
        }

        private decimal CalculateTotal()
        {
            return _products.Sum(p => p.Price);
        } 


        private decimal ReadPrice()
        {
            while (true)
            {
                Console.Write("Enter Price: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (!decimal.TryParse(input, out decimal price))
                { 
                    PrintError("Invalid price. Please enter a numeric value."); 
                    continue; 
                }

                if (price < 0)
                { 
                    PrintError("Price cannot be negative."); 
                    continue; 
                }

                return price;
            }
        }

        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERROR:\n{message}\n");
            Console.ResetColor();
        }
    }
}
