using SingleExperience.Services.ProductServices;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class Products
    {
        public void Accessories()
        {
            Console.Clear();
            Console.WriteLine("\nInício > Pesquisa > Acessórios\n");

            ListProducts(1);
            Menu();
        }
        public void Cellphone()
        {
            Console.Clear();
            Console.WriteLine("\nInício > Pesquisa > Celulares\n");

            ListProducts(2);
            Menu();
        }
        public void Computers()
        {
            Console.Clear();
            Console.WriteLine("\nInício > Pesquisa > Computadores\n");

            ListProducts(3);
            Menu();
        }
        public void Laptops()
        {
            Console.Clear();
            Console.WriteLine("\nInício > Pesquisa > Notebooks\n");

            ListProducts(4);
            Menu();
        }
        public void Tablets()
        {
            Console.Clear();
            Console.WriteLine("\nInício > Pesquisa > Tablets\n");

            ListProducts(5);
            Menu();
        }
        public void Menu()
        {
            Home inicio = new Home();
            Console.WriteLine("0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine("Digite o id do produto que deseja adicionar ao carrinho");
            string op = Console.ReadLine();

            switch (op)
            {
                case "0":
                    inicio.Menu();
                    break;
                case "1":
                    inicio.Search();
                    break;
                default:
                    break;
            }
        }

        public void ListProducts(int categoryId)
        {
            var productService = new ProductService();
            var list = productService.ListProducts();
            var j = 41;
            var teste = list
                .Where(a => a.CategoryId == categoryId)
                .Select(b => new
                {
                    b.ProdutoID,
                    b.Name,
                    b.Price,
                    b.Rating
                })
                .ToList();

            teste.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"| {p.ProdutoID}{new string(' ', j - 1 - p.ProdutoID.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
        }
    }
}
