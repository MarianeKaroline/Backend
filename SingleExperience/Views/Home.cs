using System;
using SingleExperience.Services.ProductServices;
using System.Globalization;

namespace SingleExperience.Views
{
    class Home
    {
        public Products products = new Products();
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
                    products.Accessories();
                    break;
                case 2:
                    products.Cellphone();
                    break;
                case 3:
                    products.Computers();
                    break;
                case 4:
                    products.Laptops();
                    break;
                case 5:
                    products.Tablets();
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Search();
                    break;
            }
        }

        public void ListProducts()
        {
            var productService = new ProductService();
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nInício\n");

            var list = productService.ListProducts(); 

            list.ForEach(p =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"| {p.Name}{new string(' ', j - 1 - p.Name.ToString().Length)}|");
                Console.WriteLine($"| R${p.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 6 - p.Price.ToString().Length)}|");
                if (p.Available == true)
                {
                    Console.WriteLine($"| Disponível{new string(' ', j - 1 - "Disponível".Length)}|");
                }
                Console.WriteLine($"+{new string('-', j)}+");
            });

            Menu();
        }
    }
}
