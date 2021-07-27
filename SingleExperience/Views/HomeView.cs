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
        public void Menu()
        {            
            Console.WriteLine("\n1. Buscar por categoria");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    Search();
                    break;
                default:
                    break;
            }
        }

        //Pesquisa
        public void Search()
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
                    ListProducts();
                    break;
                case 1:
                    products.Category(opc);
                    break;
                case 2:
                    products.Category(opc);
                    break;
                case 3:
                    products.Category(opc);
                    break;
                case 4:
                    products.Category(opc);
                    break;
                case 5:
                    products.Category(opc);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search();
                    break;
            }
        }

        //Listar produtos na página inicial 
        public void ListProducts()
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
            Menu();
        }
    }
}
