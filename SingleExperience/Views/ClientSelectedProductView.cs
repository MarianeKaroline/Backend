using SingleExperience.Services.ProductServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Enums;
using System;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Model;
using SingleExperience.Entities.DB;
using System.Collections.Generic;
using SingleExperience.Entities.CartEntities;

namespace SingleExperience.Views
{
    class ClientSelectedProductView
    {
        private ProductService productService = null;
        public ClientSelectedProductView()
        {
            productService = new ProductService();
        }

        //Listar Produtos
        public void SelectedProduct(int productId, ParametersModel parameters)
        {
            Console.Clear();
            var product = productService.SelectedProduct(productId);
            var j = 41;
            var category = (CategoryProductEnum)product.CategoryId;

            Console.WriteLine($"\nInício > Pesquisa > {category} > {product.Name}\n");

            var aux = product.Detail.Split(';');

            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|#{product.ProductId}{new string(' ', j - 1 - product.ProductId.ToString().Length)}|");
            Console.WriteLine($"|*{product.Rating.ToString("F1", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - product.Rating.ToString().Length)}|");
            Console.WriteLine($"|{product.Name}{new string(' ', j - product.Name.ToString().Length)}|");
            Console.WriteLine($"|R${product.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - product.Price.ToString().Length)}|");
            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Detalhes{new string(' ', j - "Detalhes".Length)}|");

            for (int i = 0; i < aux.Length; i++)
            {
                Console.WriteLine($"|{aux[i]}{new string(' ', j - aux[i].Length)}|");
            }

            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Quantidade em estoque: {product.Amount}{new string(' ', j - "Quantidade em estoque".Length - 2 - product.Amount.ToString().Length)}|");
            Console.WriteLine($"+{new string('-', j)}+");


            Menu(product, parameters, productId);
        }

        //Mostra Menu
        public void Menu(ProductSelectedModel list, ParametersModel parameters, int productId)
        {
            var signIn = new ClientSignInView();
            var signUp = new ClientSignUpView();
            var client = new ClientService();
            var categoryProduct = new ClientProductCategoryView();
            var inicio = new ClientHomeView();
            var cartList = new ClientCartView();
            var category = (CategoryProductEnum)list.CategoryId;
            var cart = new CartService();
            var cartDB = new CartDB();
            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {category}");
            Console.WriteLine("3. Adicionar produto ao carrinho");
            Console.WriteLine($"4. Ver Carrinho (Quantidade: {parameters.CountProduct})");
            if (parameters.Session.Length < 11)
            {
                Console.WriteLine("5. Fazer Login");
                Console.WriteLine("6. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("5. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema"); 
            while (invalid)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (op)
            {
                case 0:
                    inicio.ListProducts(parameters);
                    break;
                case 1:
                    inicio.Search(parameters);
                    break;
                case 2:
                    categoryProduct.Category(list.CategoryId, parameters);
                    break;
                case 3:
                    CartModel cartModel = new CartModel();
                    cartModel.ProductId = list.ProductId;
                    cartModel.UserId = parameters.Session;
                    cartModel.Name = list.Name;
                    cartModel.CategoryId = list.CategoryId;
                    cartModel.StatusId = Convert.ToInt32(StatusProductEnum.Ativo);
                    cartModel.Price = list.Price;

                    if (parameters.Session.Length < 11)
                    {

                        parameters.CartMemory = cartDB.AddItensMemory(cartModel, parameters.CartMemory);
                    }
                    else
                    {
                        cartDB.AddItemCart(parameters, cartModel);

                        parameters.CartMemory = new List<ItemEntitie>();
                    }

                    parameters.CountProduct = cart.TotalCart(parameters).TotalAmount;

                    Console.WriteLine("\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();
                    SelectedProduct(list.ProductId, parameters);
                    break;
                case 4:
                    cartList.ListCart(parameters);
                    break;
                case 5:
                    if (parameters.Session.Length == 11)
                    {
                        parameters.Session = client.SignOut();
                        parameters.CountProduct = cart.TotalCart(parameters).TotalAmount;
                        SelectedProduct(productId, parameters);
                    }
                    else
                    {
                        signIn.Login(parameters, true);
                    }
                    break;
                case 6:
                    signUp.SignUp(parameters, true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
