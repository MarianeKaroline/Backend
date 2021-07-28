using System;
using SingleExperience.Services.ProductServices;
using System.Globalization;
using System.Linq;

namespace SingleExperience.Views
{
    class HomeView
    {
        public ProductCategoryView products = new ProductCategoryView();
        //Tela inicial
        public void Menu(int countProductCart, string ipComputer)
        {            
            Console.WriteLine("\n1. Buscar por categoria");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {countProductCart})");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    Search(countProductCart, ipComputer);
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }

        //Pesquisa
        public void Search(int countProductCart, string ipComputer)
        {
            Console.Clear();

            Console.WriteLine("\nInício > Pesquisa\n");

            Console.WriteLine("0. Voltar Início");
            Console.WriteLine("1. Acessórios");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. Computador");
            Console.WriteLine("4. Notebook");
            Console.WriteLine("5. Tablets");
            Console.WriteLine($"6. Ver Carrinho {countProductCart}");
            int opc = int.Parse(Console.ReadLine());

            switch (opc)
            {
                case 0:
                    ListProducts(countProductCart, ipComputer);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    products.Category(opc, countProductCart, ipComputer);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search(countProductCart, ipComputer);
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts(int countProductCart, string ipComputer)
        {
            var productService = new ProductService();
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nInício\n");

            var list = productService.ListProductsTable();

            list.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });
            Menu(countProductCart, ipComputer);
        }
    }
}
