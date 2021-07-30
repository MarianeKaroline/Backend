using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.ProductServices;
using SingleExperience.Services.ProductServices.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class CartView
    {
        private CartService cart = null;

        public CartView()
        {
            cart = new CartService();
        }

        public void ListCart(string ipComputer)
        {
            Console.Clear();
            var total = cart.TotalCart(ipComputer);
            var j = 41; 

            Console.WriteLine($"\nInício > Carrinho\n");

            var itens = cart.ItemCard(ipComputer)
                .GroupBy(p => p.Name)
                .Select(p => new ProductsCartModel()
                { 
                    ProductId = p.First().ProductId,
                    Name = p.First().Name,
                    Price = p.First().Price,
                    StatusId = p.First().StatusId,
                    CategoryId = p.First().CategoryId,
                    Amount = p.Sum(j => j.Amount)
                })
                .ToList();

            itens.ForEach(p =>
            {
                if (p.StatusId == Convert.ToInt32(StatusProductEnums.Ativo))
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                    Console.WriteLine($"|{p.Name}{new string(' ', j - p.Name.Length)}|");
                    Console.WriteLine($"|Qtd: {p.Amount}{new string(' ', j - 5 - p.Amount.ToString().Length)}|");
                    Console.WriteLine($"|R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - p.Price.ToString().Length)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                }
            });

            Console.WriteLine($"\nSubtotal ({total.TotalAmount} itens): R${total.TotalPrice}");
            Menu(ipComputer);
        }

        public void Menu(string ipComputer)
        {
            var inicio = new HomeView();
            var productCategory = new ProductCategoryView();
            var total = cart.TotalCart(ipComputer);
            var category = cart.ItemCard(ipComputer)
                .Select(p => p.CategoryId)
                .FirstOrDefault();

            Console.WriteLine("0. Início");
            Console.WriteLine("1. Buscar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {(CategoryProductEnums)category}");
            Console.WriteLine("3. Adicionar o produto mais uma vez ao carrinho");
            Console.WriteLine("4. Excluir um produto");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts(total.TotalAmount, ipComputer);
                    break;
                case 1:
                    inicio.Search(total.TotalAmount, ipComputer);
                    break;
                case 2:
                    productCategory.Category(category, total.TotalAmount, ipComputer);
                    break;
                case 3:
                    Console.Write("\nPor favor digite o código do produto #");
                    int code = int.Parse(Console.ReadLine());
                    cart.AddCart(code, ipComputer);
                    var count = cart.TotalCart(ipComputer);

                    Console.WriteLine("\n\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();

                    ListCart(ipComputer);
                    break;
                case 4:
                    Console.Write("\nPor favor digite o codigo do produto #");
                    int codeRemove = int.Parse(Console.ReadLine());

                    cart.RemoveItem(codeRemove, ipComputer);
                    var countItens = cart.TotalCart(ipComputer);
                    Console.WriteLine("\n\nProduto removido com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();

                    ListCart(ipComputer);
                    break;
                default:
                    break;
            }
        }
    }
}
