using SingleExperience.Services.ProductServices;
using SingleExperience.Entities.Enums;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace SingleExperience.Views
{
    class SelectedProductView
    {
        //Listar Produtos
        public void ListProducts(int productId)
        {
            Console.Clear();

            var productService = new ProductService();
            var list = productService.ListProductSelected(productId);
            var j = 41;
            var category = (CategoryProductEnums)list.CategoryId;

            Console.WriteLine($"\nInício > Pesquisa > {category} > {list.Name}\n");

            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|#{list.ProductId}{new string(' ', j - 1 - list.ProductId.ToString().Length)}|");
            Console.WriteLine($"|*{list.Rating.ToString("F1", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - list.Rating.ToString().Length)}|");
            Console.WriteLine($"|{list.Name}{new string(' ', j - list.Name.ToString().Length)}|");
            Console.WriteLine($"|R${list.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - list.Price.ToString().Length)}|");
            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Detalhes{new string(' ', j - "Detalhes".Length)}|");
            Console.WriteLine($"|{list.Detail.Replace(";", "\n|")}|");
            Console.WriteLine($"|Quantidade em estoque: {list.Amount}{new string(' ', j - "Quantidade em estoque".Length - 2 - list.Amount.ToString().Length)}|");
            Console.WriteLine($"+{new string('-', j)}+");

            Menu(productId);
        }

        //Mostra Menu
        public void Menu(int productId)
        {
            var categoryProduct = new ProductCategoryView();
            var inicio = new HomeView();
            var productService = new ProductService();
            var list = productService.ListProductSelected(productId);
            var category = (CategoryProductEnums)list.CategoryId; //Busca o nome da categoria por enum

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine("2. Voltar para a categoria: " + category);
            Console.WriteLine("Digite o código # para adicionar ao carrinho");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts();
                    break;
                case 1:
                    inicio.Search();
                    break;
                case 2:
                    categoryProduct.Category(list.CategoryId);
                    break;
                default:

                    break;
            }
        }
    }
}
