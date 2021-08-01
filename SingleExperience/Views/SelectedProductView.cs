using SingleExperience.Services.ProductServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Entities.Enums;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Models.ProductModels;
using SingleExperience.Entities.DB;

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
        public void SelectedProduct(int productId, int countProduct, string session)
        {
            Console.Clear();
            var list = productService.ProductSelected(productId);
            var j = 41;
            var category = (CategoryProductEnum)list.CategoryId;

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


            Menu(list, countProduct, session);
        }

        //Mostra Menu
        public void Menu(ProductSelectedModel list, int countProduct, string session)
        {
            var signIn = new SignInView();
            var signUp = new SignUpView();
            var client = new ClientService();
            var categoryProduct = new ProductCategoryView();
            var inicio = new HomeView();
            var cartList = new CartView();
            var category = (CategoryProductEnum)list.CategoryId;
            var cart = new CartService();
            var cartDB = new CartDB();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {category}");
            Console.WriteLine("3. Adicionar produto ao carrinho");
            Console.WriteLine($"4. Ver Carrinho (Quantidade: {countProduct})");
            if (session.Length == 10)
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
                    inicio.ListProducts(countProduct, session);
                    break;
                case 1:
                    inicio.Search(countProduct, session);
                    break;
                case 2:
                    categoryProduct.Category(list.CategoryId, countProduct, session);
                    break;
                case 3:
                    CartModel cartModel = new CartModel();
                    cartModel.ProductId = list.ProductId;
                    cartModel.UserId = session;
                    cartModel.Name = list.Name;
                    cartModel.CategoryId = list.CategoryId;
                    cartModel.StatusId = Convert.ToInt32(StatusProductEnum.Ativo);
                    cartModel.Price = list.Price;

                    cartDB.AddItensCart(cartModel);
                    var count = cart.TotalCart(session);

                    Console.WriteLine("Produto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();
                    SelectedProduct(list.ProductId, count.TotalAmount, session);
                    break;
                case 4:
                    cartList.ListCart(session);
                    break;
                case 5:
                    if (session.Length == 11)
                    {
                        client.SignOut();
                    }
                    else
                    {
                        signIn.Login(countProduct, session, true);
                    }
                    break;
                case 6:
                    signUp.SignUp(countProduct, true);
                    break;
                default:
                    break;
            }
        }
    }
}
