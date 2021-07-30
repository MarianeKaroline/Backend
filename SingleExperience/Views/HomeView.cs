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
        //Tela inicial
        public void Menu(int countProductCart, string ipComputer, long session)
        {
            SelectedProductView selectedProduct = new SelectedProductView();
            SignInView signIn = new SignInView();
            SignUpView signUp = new SignUpView();
            var cart = new CartView();

            Console.WriteLine("\n1. Buscar por categoria");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {countProductCart})");
            if (session == 0)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("Digite o código do produto # para mais detalhes\n");
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 1:
                    Search(countProductCart, ipComputer, session);
                    break;
                case 2:
                    cart.ListCart(ipComputer, session);
                    break;
                case 3:
                    signIn.Login(countProductCart, ipComputer);
                    break;
                case 4:
                    signUp.SignUp(ipComputer);
                    break;
                default:
                    selectedProduct.SelectedProduct(opc, countProductCart, ipComputer, session);
                    break;
            }
        }

        //Pesquisa
        public void Search(int countProductCart, string ipComputer, long session)
        {
            Console.Clear();

            Console.WriteLine("\nInício > Pesquisa\n");

            Console.WriteLine("0. Voltar Início");
            Console.WriteLine("1. Acessórios");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. Computador");
            Console.WriteLine("4. Notebook");
            Console.WriteLine("5. Tablets");
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    ListProducts(countProductCart, ipComputer, session);
                    break;
                case 1:
                    products.Category(Convert.ToInt32(CategoryProductEnums.Acessorio), countProductCart, ipComputer, session);
                    break;
                case 2:
                    products.Category(Convert.ToInt32(CategoryProductEnums.Celular), countProductCart, ipComputer, session);
                    break;
                case 3:
                    products.Category(Convert.ToInt32(CategoryProductEnums.Computador), countProductCart, ipComputer, session);
                    break;
                case 4:
                    products.Category(Convert.ToInt32(CategoryProductEnums.Notebook), countProductCart, ipComputer, session);
                    break;
                case 5:
                    products.Category(Convert.ToInt32(CategoryProductEnums.Tablets), countProductCart, ipComputer, session);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search(countProductCart, ipComputer, session);
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts(int countProductCart, string ipComputer, long session)
        {
            var client = new ClientService();
            var productService = new ProductService();
            var products = productService.ListProductsTable()
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

            if (session > 0)
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
            Menu(countProductCart, ipComputer, session);
        }
    }
}
