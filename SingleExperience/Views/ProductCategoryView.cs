using SingleExperience.Services.ProductServices;
using SingleExperience.Views;
using SingleExperience.Entities.Enums;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SingleExperience.Services.ProductServices.Models.ProductModels;

namespace SingleExperience.Views
{
    class ProductCategoryView
    {
        //Chama ListaProdutos pela Categoria
        public void Category(int id, int countProductCart, string ipComputer)
        {
            Console.Clear();
            var category = (CategoryProductEnums)id;
            Console.WriteLine($"\nInício > Pesquisa > {category}\n");

            ListProducts(id, countProductCart);
            Menu(countProductCart, ipComputer);
        }
        
        //Menu dos Produtos
        public void Menu(int countProductCart, string ipComputer)
        {
            var selectedProduct = new SelectedProductView();
            var cart = new CartView();
            var inicio = new HomeView();

            Console.WriteLine("0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            Console.WriteLine("Digite o codigo do produto # para mais detalhes\n");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts(countProductCart, ipComputer);
                    break;
                case 1:
                    inicio.Search(countProductCart, ipComputer);
                    break;
                case 2:
                    cart.ListCart(ipComputer);
                    break;
                default:
                    selectedProduct.SelectedProduct(op, countProductCart, ipComputer);
                    break;
            }

            
        }
        //Listar Produtos selecionado
        public void ListProducts(int categoryId, int countProductCart)
        {
            var productService = new ProductService();
            var itemCategory = productService.ListProductCategory(categoryId)
                .GroupBy(i => i.Name)
                .Select(j => new CategoryModel()
                {
                    ProductId = j.First().ProductId,
                    Name = j.First().Name,
                    Price = j.First().Price,
                    CategoryId = j.First().CategoryId,
                    Available = j.First().Available
                })
                .ToList();
            var j = 41;

            itemCategory.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"|R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
        }
    }
}
