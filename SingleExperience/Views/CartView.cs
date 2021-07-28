using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class CartView
    {
        public void ListCart(string ipComputer)
        {
            Console.Clear();
            var cart = new CartService();
            var list = cart.ItemCard(ipComputer);
            var j = 41; 
            var count = 0;

            Console.WriteLine($"\nInício > Carrinho\n");

            var itens = list
                .GroupBy(p => p.ProductId)
                .Where(p => p.Count() > 1)
                .ToList();

            list.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"|{p.Name}{new string(' ', j - 1 - p.Name.Length)}|");
                Console.WriteLine($"|{p.Amount}{new string(' ', j - 1 - p.Amount.ToString().Length)}|");
                Console.WriteLine($"|R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");

                if (count == list.Count - 1)
                {
                    Console.WriteLine($"\nPreço Total: R${p.TotalPrice}");
                }

                count++;
            });
            
        }
    }
}
