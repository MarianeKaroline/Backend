using System;
using SingleExperience.Services.ProductServices;
using System.Globalization;
using System.Linq;
using SingleExperience.Services.ProductServices.Model;
using SingleExperience.Entities.Enums;
using SingleExperience.Services.ClientServices;

namespace SingleExperience.Views
{
    class HomeView
    {
        public ProductCategoryView products = new ProductCategoryView();
        public ProductService product = new ProductService();
        //Tela inicial
        public void Menu(int countProductCart, string session)
        {
            var selectedProduct = new SelectedProductView();
            var signIn = new SignInView();
            var signUp = new SignUpView();
            var client = new ClientService();
            var cart = new CartView();

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Buscar por categoria");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {countProductCart})");
            if (session.Length == 10)
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
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");

                    Menu(countProductCart, session);
                    break;
                case 1:
                    Search(countProductCart, session);
                    break;
                case 2:
                    cart.ListCart(session);
                    break;
                case 3:
                    if (session.Length == 11)
                    {
                        client.SignOut();
                        ListProducts(countProductCart, session);
                    }
                    else
                    {
                        signIn.Login(countProductCart, session, true);
                    }
                    break;
                case 4:
                    signUp.SignUp(countProductCart, true);
                    break;
                case 5:
                    Console.Write("\nDigite o código # do produto: ");
                    int code = int.Parse(Console.ReadLine());
                    if (product.HasProduct(code))
                    {
                        selectedProduct.SelectedProduct(code, countProductCart, session);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(countProductCart, session);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(countProductCart, session);
                    break;
            }
        }

        //Pesquisa
        public void Search(int countProductCart, string session)
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
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    ListProducts(countProductCart, session);
                    break;
                case 1:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Acessorio), countProductCart, session);
                    break;
                case 2:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Celular), countProductCart, session);
                    break;
                case 3:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Computador), countProductCart, session);
                    break;
                case 4:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Notebook), countProductCart, session);
                    break;
                case 5:
                    products.Category(Convert.ToInt32(CategoryProductEnum.Tablets), countProductCart, session);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");

                    Menu(countProductCart, session);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search(countProductCart, session);
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts(int countProductCart, string session)
        {
            var client = new ClientService();
            var productService = new ProductService();
            var products = productService.ListProducts()
                .GroupBy(i => i.Name)
                .Select(j => new BestSellingModel()
                {
                    ProductId = j.First().ProductId,
                    Name = j.First().Name,
                    Price = j.First().Price,
                    Available = j.First().Available,
                    Ranking = j.Sum(ij => ij.Ranking)
                })
                .ToList();
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nInício\n");

            if (session.Length == 11)
            {
                Console.WriteLine($"Usuário: {client.ClientName(session)}");
            }

            products.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
            Menu(countProductCart, session);
        }
    }
}
