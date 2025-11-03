// Jalen Robbins
// Cafe Manager - C# Version
// Simulates a small café ordering system with cart saving/loading via File I/O

using System;
using System.Collections.Generic;
using System.IO;

class CafeManager
{
    static string cafeName = "Brew & Bytes Café";
    static double taxRate = 0.07;
    static string discountCode = "STUDENT10";
    static double discountRate = 0.10;

    static List<string> menuItems = new List<string> { "Cookie", "Latte", "Water" };
    static List<double> menuPrices = new List<double> { 2.00, 4.50, 1.25 };

    static List<string> cartNames = new List<string>();
    static List<double> cartPrices = new List<double>();
    static List<int> cartQuantities = new List<int>();

    static bool discountUsed = false;
    static string cartFile = "cart.txt"; // File to save/load the cart

    static void ShowBanner()
    {
        Console.WriteLine("===================================");
        Console.WriteLine($" Welcome to {cafeName}");
        Console.WriteLine($" Tax Rate: {taxRate * 100:0}%");
        Console.WriteLine("===================================\n");
    }

    static void ShowMenuItems()
    {
        Console.WriteLine("Menu:");
        for (int i = 0; i < menuItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {menuItems[i]} - ${menuPrices[i]:0.00}");
        }
        Console.WriteLine();
    }

    static void AddItem(int itemIndex, int quantity)
    {
        string name = menuItems[itemIndex];
        double price = menuPrices[itemIndex];

        cartNames.Add(name);
        cartPrices.Add(price);
        cartQuantities.Add(quantity);

        Console.WriteLine($"Added {quantity} x {name} at ${price:0.00} each.\n");
    }

    static void ViewCart()
    {
        if (cartNames.Count == 0)
        {
            Console.WriteLine("Your cart is empty.\n");
            return;
        }

        Console.WriteLine("Your Cart:");
        Console.WriteLine("-----------------------------------");
        double subtotal = 0;

        for (int i = 0; i < cartNames.Count; i++)
        {
            double lineTotal = cartPrices[i] * cartQuantities[i];
            subtotal += lineTotal;
            Console.WriteLine($"{i + 1}. {cartNames[i]} - ${cartPrices[i]:0.00} x {cartQuantities[i]} = ${lineTotal:0.00}");
        }

        double avgLineTotal = subtotal / cartNames.Count;
        double maxPrice = 0;
        int maxIndex = 0;

        for (int i = 0; i < cartPrices.Count; i++)
        {
            if (cartPrices[i] > maxPrice)
            {
                maxPrice = cartPrices[i];
                maxIndex = i;
            }
        }

        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Subtotal: ${subtotal:0.00}");
        Console.WriteLine($"Average Line Total: ${avgLineTotal:0.00}");
        Console.WriteLine($"Most Expensive Item: {cartNames[maxIndex]} at ${maxPrice:0.00}\n");
    }

    static void RemoveItem(int itemNumber)
    {
        if (itemNumber < 1 || itemNumber > cartNames.Count)
        {
            Console.WriteLine("Invalid item number.\n");
            return;
        }

        string removed = cartNames[itemNumber - 1];
        cartNames.RemoveAt(itemNumber - 1);
        cartPrices.RemoveAt(itemNumber - 1);
        cartQuantities.RemoveAt(itemNumber - 1);

        Console.WriteLine($"Removed {removed} from the cart.\n");
    }

    static double ComputeSubtotal()
    {
        double subtotal = 0;
        for (int i = 0; i < cartNames.Count; i++)
        {
            subtotal += cartPrices[i] * cartQuantities[i];
        }
        return subtotal;
    }

    static double ComputeTax(double subtotal)
    {
        return subtotal * taxRate;
    }

    static double ApplyDiscount(double totalWithTax, string code)
    {
        if (code.ToUpper() == discountCode)
        {
            return totalWithTax * discountRate;
        }
        else
        {
            return 0.0;
        }
    }

    static void Checkout()
    {
        if (cartNames.Count == 0)
        {
            Console.WriteLine("Cart is empty. Nothing to checkout.\n");
            return;
        }

        double subtotal = ComputeSubtotal();
        double tax = ComputeTax(subtotal);
        double total = subtotal + tax;

        Console.Write("Enter discount code (or press Enter to skip): ");
        string code = Console.ReadLine().Trim();
        double discount = 0.0;

        if (code != "")
        {
            if (code.ToUpper() == discountCode && !discountUsed)
            {
                discount = ApplyDiscount(total, code);
                discountUsed = true;
                Console.WriteLine("Discount applied.\n");
            }
            else
            {
                Console.WriteLine("Invalid or already used discount code.\n");
            }
        }

        double finalTotal = total - discount;

        Console.WriteLine("---------- Receipt ----------");
        Console.WriteLine($"Subtotal:     ${subtotal:0.00}");
        Console.WriteLine($"Tax:          ${tax:0.00}");
        Console.WriteLine($"Discount:    -${discount:0.00}");
        Console.WriteLine($"Total Due:    ${finalTotal:0.00}");
        Console.WriteLine("-----------------------------\n");

        cartNames.Clear();
        cartPrices.Clear();
        cartQuantities.Clear();
    }

    // === File I/O Features ===

    static void SaveCart()
    {
        using (StreamWriter writer = new StreamWriter(cartFile))
        {
            for (int i = 0; i < cartNames.Count; i++)
            {
                writer.WriteLine($"{cartNames[i]},{cartPrices[i]},{cartQuantities[i]}");
            }
        }
        Console.WriteLine("Cart saved successfully!\n");
    }

    static void LoadCart()
    {
        if (!File.Exists(cartFile))
        {
            Console.WriteLine("No saved cart found.\n");
            return;
        }

        cartNames.Clear();
        cartPrices.Clear();
        cartQuantities.Clear();

        string[] lines = File.ReadAllLines(cartFile);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 3)
            {
                cartNames.Add(parts[0]);
                cartPrices.Add(double.Parse(parts[1]));
                cartQuantities.Add(int.Parse(parts[2]));
            }
        }
        Console.WriteLine("Cart loaded successfully!\n");
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View Cart");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Checkout");
            Console.WriteLine("5. Save Cart");
            Console.WriteLine("6. Load Cart");
            Console.WriteLine("7. Quit");
            Console.Write("Choose an option (1-7): ");
            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    ShowMenuItems();
                    Console.Write("Enter item number: ");
                    if (int.TryParse(Console.ReadLine(), out int itemNum) && itemNum >= 1 && itemNum <= menuItems.Count)
                    {
                        Console.Write("Enter quantity: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                        {
                            AddItem(itemNum - 1, quantity);
                        }
                        else Console.WriteLine("Invalid quantity.\n");
                    }
                    else Console.WriteLine("Invalid item number.\n");
                    break;

                case "2":
                    ViewCart();
                    break;

                case "3":
                    if (cartNames.Count == 0)
                    {
                        Console.WriteLine("Cart is empty.\n");
                        break;
                    }
                    ViewCart();
                    Console.Write("Enter item number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int removeNum))
                    {
                        RemoveItem(removeNum);
                    }
                    else Console.WriteLine("Invalid input.\n");
                    break;

                case "4":
                    Checkout();
                    break;

                case "5":
                    SaveCart();
                    break;

                case "6":
                    LoadCart();
                    break;

                case "7":
                    Console.WriteLine("Thanks for visiting!");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please choose 1–7.\n");
                    break;
            }
        }
    }

    static void Main(string[] args)
    {
        ShowBanner();
        MainMenu();
    }
}

