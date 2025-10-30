// Jalen Robbins

using System;
using System.Collections.Generic;
using System.Linq;

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

    static void ShowBanner()
    {
        Console.WriteLine("===================================");
        Console.WriteLine($" Welcome to {cafeName}");
        Console.WriteLine($" Tax Rate: {taxRate * 100:F0}%");
        Console.WriteLine("===================================\n");
    }

    static void ShowMenuItems()
    {
        Console.WriteLine("Menu:");
        for (int i = 0; i < menuItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {menuItems[i]} - ${menuPrices[i]:F2}");
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
        Console.WriteLine($"Added {quantity} x {name} at ${price:F2} each.\n");
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
            Console.WriteLine($"{i + 1}. {cartNames[i]} - ${cartPrices[i]:F2} x {cartQuantities[i]} = ${lineTotal:F2}");
        }

        double averageLineTotal = subtotal / cartNames.Count;
        double maxPrice = cartPrices.Max();
        int maxIndex = cartPrices.IndexOf(maxPrice);

        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Subtotal: ${subtotal:F2}");
        Console.WriteLine($"Average Line Total: ${averageLineTotal:F2}");
        Console.WriteLine($"Most Expensive Item: {cartNames[maxIndex]} at ${maxPrice:F2}\n");
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
            return totalWithTax * discountRate;
        else
            return 0.0;
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

        if (!string.IsNullOrEmpty(code))
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
        Console.WriteLine($"Subtotal:     ${subtotal:F2}");
        Console.WriteLine($"Tax:          ${tax:F2}");
        Console.WriteLine($"Discount:    -${discount:F2}");
        Console.WriteLine($"Total Due:    ${finalTotal:F2}");
        Console.WriteLine("-----------------------------\n");

        // Clear the cart
        cartNames.Clear();
        cartPrices.Clear();
        cartQuantities.Clear();
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
            Console.WriteLine("5. Quit");
            Console.Write("Choose an option (1-5): ");
            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    ShowMenuItems();
                    Console.Write("Enter item number: ");
                    string itemInput = Console.ReadLine().Trim();
                    Console.Write("Enter quantity: ");
                    string quantityInput = Console.ReadLine().Trim();

                    if (int.TryParse(itemInput, out int itemNum) && int.TryParse(quantityInput, out int quantity))
                    {
                        if (itemNum >= 1 && itemNum <= menuItems.Count && quantity > 0)
                            AddItem(itemNum - 1, quantity);
                        else
                            Console.WriteLine("Invalid item number or quantity.\n");
                    }
                    else
                    {
                        Console.WriteLine("Please enter numbers only.\n");
                    }
                    break;

                case "2":
                    ViewCart();
                    break;

                case "3":
                    if (cartNames.Count == 0)
                    {
                        Console.WriteLine("Cart is empty.\n");
                        continue;
                    }
                    ViewCart();
                    Console.Write("Enter item number to remove: ");
                    string removeInput = Console.ReadLine().Trim();
                    if (int.TryParse(removeInput, out int removeNum))
                        RemoveItem(removeNum);
                    else
                        Console.WriteLine("Invalid input.\n");
                    break;

                case "4":
                    Checkout();
                    break;

                case "5":
                    Console.WriteLine("Thanks for visiting!");
                    return;

                default:
                    Console.WriteLine("Invalid option. Please choose 1–5.\n");
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
