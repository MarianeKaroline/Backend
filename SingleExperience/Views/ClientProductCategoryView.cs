using SingleExperience.Services.ProductServices;
using SingleExperience.Views;
using SingleExperience.Enums;
using System;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.CartServices;

namespace SingleExperience.Views
{
    class ClientProductCategoryView
    {
        private CartService cartService = new CartService();
        private ClientService clientService = new ClientService();
        private ProductService productService = new ProductService();

        //Chama ListaProdutos pela Categoria
        public void Category(int id, SessionModel parameters)
        {
            Console.Clear();
            var category = (CategoryProductEnum)id;
            Console.WriteLine($"\nInício > Pesquisa > {category}\n");

            ListProducts(id);
            Menu(parameters, id);
        }

        //Menu dos Produtos
        public void Menu(SessionModel parameters, int id)
        {
            ClientSelectedProductView selectedProduct = new ClientSelectedProductView();
            ClientSignInView signIn = new ClientSignInView();
            ClientSignUpView signUp = new ClientSignUpView();
            ClientCartView cartView = new ClientCartView();
            ClientHomeView inicio = new ClientHomeView();

            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {parameters.CountProduct})");
            if (parameters.Session.Length < 11)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema");
            Console.WriteLine("\n5. Selecionar um produto");
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
                    cartView.ListCart(parameters);
                    break;
                case 3:
                    if (parameters.Session.Length == 11)
                    {
                        parameters.Session = clientService.SignOut();
                        parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                        Category(id, parameters);
                    }
                    else
                    {
                        signIn.Login(parameters, true);
                    }
                    break;
                case 4:
                    signUp.SignUp(parameters, true);
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: ");
                    int code = int.Parse(Console.ReadLine());
                    if (productService.HasProduct(code))
                    {
                        selectedProduct.SelectedProduct(code, parameters);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(parameters, id);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters, id);
                    break;
            }


        }
        //Listar Produtos selecionado
        public void ListProducts(int categoryId)
        {
            var itemCategory = productService.ListProductCategory(categoryId);
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
