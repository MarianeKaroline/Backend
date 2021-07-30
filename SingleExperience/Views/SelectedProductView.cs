using SingleExperience.Services.ProductServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Entities.Enums;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace SingleExperience.Views
{
    class SelectedProductView
    {
        private ProductService productService = null;
        public SelectedProductView()
        {
            productService = new ProductService();
        }

        //Listar Produtos
        public void SelectedProduct(int productId, int countProduct, string ipComputer, long session)
        {
            Console.Clear();
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

            Menu(productId, countProduct, ipComputer, session);
        }

        //Mostra Menu
        public void Menu(int productId, int countProduct, string ipComputer, long session)
        {
            var categoryProduct = new ProductCategoryView();
            var inicio = new HomeView();
            var cartList = new CartView();
            var list = productService.ListProductSelected(productId);
            var category = (CategoryProductEnums)list.CategoryId;
            var cart = new CartService();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {category}");
            Console.WriteLine("3. Adicionar produto ao carrinho");
            Console.WriteLine($"4. Ver Carrinho (Quantidade: {countProduct})");
            if (session == 0)
            {
                Console.WriteLine("5. Fazer Login");
                Console.WriteLine("6. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("5. Desconectar-se");
            }
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts(countProduct, ipComputer, session);
                    break;
                case 1:
                    inicio.Search(countProduct, ipComputer, session);
                    break;
                case 2:
                    categoryProduct.Category(list.CategoryId, countProduct, ipComputer, session);
                    break;
                case 3:
                    cart.AddCart(productId, ipComputer);
                    var count = cart.TotalCart(ipComputer);

                    Console.WriteLine("Produto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();
                    SelectedProduct(productId, count.TotalAmount, ipComputer, session);
                    break;
                case 4:
                    cartList.ListCart(ipComputer, session);
                    break;
                case 5:

                    break;
                case 6:

                    break;
                default:
                    break;
            }
        }
    }
}
