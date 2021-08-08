using System;
using SingleExperience.Services.ProductServices;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.EmployeeServices;

namespace SingleExperience.Views
{
    class ClientHomeView
    {
        public ClientProductCategoryView products = new ClientProductCategoryView();
        public ProductService product = new ProductService();
        //Tela inicial
        public void Menu(ParametersModel parameters)
        {
            var cartService = new CartService();
            var selectedProduct = new ClientSelectedProductView();
            var signIn = new ClientSignInView();
            var signUp = new ClientSignUpView();
            var perfilClientView = new ClientPerfilView();
            var employee = new EmployeeService();
            var client = new ClientService();
            var boughtService = new BoughtService();
            var pefilEmployee = new EmployeePerfilView();
            var cart = new ClientCartView();
            var opc = 0;
            var invalid = true;
            var invalidCode = true;

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Buscar por categoria");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {parameters.CountProduct})");
            if (parameters.Session.Length != 11)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Ver perfil");
                Console.WriteLine("4. Desconectar-se");
            }
            Console.WriteLine("5. Selecionar um produto");
            Console.WriteLine("6. Área funcionário");
            Console.WriteLine("9. Sair do Sistema");
            while (invalid)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");
                    Console.WriteLine("Tecle enter para continuar");
                    Console.ReadLine();

                    ListProducts(parameters);
                    break;
                case 1:
                    Search(parameters);
                    break;
                case 2:
                    cart.ListCart(parameters);
                    break;
                case 3:
                    if (parameters.Session.Length == 11)
                    {
                        var listBought = boughtService.ClientBought(parameters.Session);
                        perfilClientView.Menu(listBought, parameters);
                    }
                    else
                    {
                        signIn.Login(parameters, true);
                    }
                    break;
                case 4:
                    if (parameters.Session.Length == 11)
                    {
                        parameters.Session = client.SignOut();
                        parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                        ListProducts(parameters);
                    }
                    else if (parameters.Session.Length > 11)
                    {
                        parameters.Session = employee.SignOut();
                        ListProducts(parameters);
                    }
                    else
                    {
                        signUp.SignUp(parameters, true);
                    }
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: "); 
                    while (invalidCode)
                    {
                        try
                        {
                            int code = int.Parse(Console.ReadLine());
                            if (product.HasProduct(code))
                            {
                                selectedProduct.SelectedProduct(code, parameters);
                            }
                            else
                            {
                                Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                                Console.ReadKey();
                                Menu(parameters);
                            }
                            invalidCode = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                        }
                    }
                    break;
                case 6:
                    if (parameters.Session.Length < 12)
                    {
                        signIn.LoginEmployee(parameters);
                    }
                    else
                    {
                        pefilEmployee.Menu(parameters);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters);
                    break;
            }
        }

        //Pesquisa
        public void Search(ParametersModel parameters)
        {
            Console.Clear();

            Console.WriteLine("\nInício > Pesquisa\n");

            Console.WriteLine("0. Voltar Início");
            Console.WriteLine("1. Acessórios");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. Computador");
            Console.WriteLine("4. Notebook");
            Console.WriteLine("5. Tablets");
            Console.WriteLine("6. Precisa de ajuda?");
            Console.WriteLine("9. Sair do Sistema");

            //Colocar try catch
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    ListProducts(parameters);
                    break;
                case 1:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Acessorio), parameters);
                    break;
                case 2:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Celular), parameters);
                    break;
                case 3:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Computador), parameters);
                    break;
                case 4:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Notebook), parameters);
                    break;
                case 5:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Tablets), parameters);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");

                    Menu(parameters);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search(parameters);
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts(ParametersModel parameters)
        {
            var client = new ClientService();
            var productService = new ProductService();
            var products = productService.ListProducts();
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nInício\n");

            if (parameters.Session.Length == 11)
            {
                Console.WriteLine($"Usuário: {client.ClientName(parameters.Session)}");
            }

            products.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
            Menu(parameters);
        }
    }
}
