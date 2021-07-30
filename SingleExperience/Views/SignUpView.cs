using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class SignUpView
    {
        public string password = null;
        public void SignUp(int countProductCart, string ipComputer)
        {
            Random random = new Random();
            var rand = random.Next(100000000, 200000000);
            Console.Clear();
            var client = new SignUpModel();
            var clientService = new ClientService();
            var j = 41;

            Console.WriteLine("Inicio > Cadastrar-se\n");
            Console.Write("Nome Completo: ");
            client.FullName = Console.ReadLine();
            Console.Write("Telefone: ");
            client.Phone = Console.ReadLine();
            Console.Write("E-mail: ");
            client.Email = Console.ReadLine();
            Console.Write("CPF: ");
            client.Cpf = long.Parse(Console.ReadLine());
            Console.Write("Data de Nascimento: ");
            client.BirthDate = DateTime.Parse(Console.ReadLine());
            var equal = passwords();

            if (!equal)
            {
                Console.WriteLine("As senhas são diferentes, tente novamente. (Tecle enter para continuar)");
                Console.ReadKey();
                equal = passwords();
            }
            else if (equal)
            {
                client.Password = password;
            }

            client.SessionId = rand;

            Console.WriteLine($"\n+{new string('-', j)}+\n");

            Console.WriteLine("Endereço\n");
            Console.Write("CEP: ");
            client.Cep = Console.ReadLine();
            Console.Write("Rua: ");
            client.Street = Console.ReadLine();
            Console.Write("Número: ");
            client.Number = Console.ReadLine();
            Console.Write("Cidade: ");
            client.City = Console.ReadLine();
            Console.Write("Estado: ");
            client.State = Console.ReadLine();

            var msg = clientService.SignUp(client);

            Console.WriteLine(msg);
            Console.WriteLine("Tecle enter para continuar");
            Console.ReadKey();

            Menu(countProductCart, ipComputer, rand);
        }

        public bool passwords()
        {
            var equal = false;

            Console.Write("\nDigite uma senha de usuário: ");
            password = Console.ReadLine();
            Console.Write("Confirmar senha: ");
            string confirmPassword = Console.ReadLine();

            if (password == confirmPassword)
            {
                equal = true;
            }

            return equal;
        }

        public void Menu(int countProductCart, string ipComputer, long session)
        {
            var selectedProduct = new SelectedProductView();
            var cart = new CartView();
            var inicio = new HomeView();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            if (session == 0)
            {
                Console.WriteLine("3. Fazer Login");
                Console.WriteLine("4. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("Digite o codigo do produto # para mais detalhes\n");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 0:
                    inicio.ListProducts(countProductCart, ipComputer, session);
                    break;
                case 1:
                    inicio.Search(countProductCart, ipComputer, session);
                    break;
                case 2:
                    cart.ListCart(ipComputer, session);
                    break;
                default:
                    selectedProduct.SelectedProduct(op, countProductCart, ipComputer, session);
                    break;
            }
        }
    }
}
