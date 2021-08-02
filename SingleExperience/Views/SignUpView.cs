using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;

namespace SingleExperience.Views
{
    class SignUpView
    {
        public string password = null;
        public void SignUp(int countProductCart, bool home)
        {
            var payment = new PaymentMethodView();
            var cartView = new CartView();
            var cart = new CartService();
            var client = new SignUpModel();
            var cartDB = new CartDB();
            var clientDB = new ClientDB();
            var j = 41;

            Console.Clear();

            Console.WriteLine("Inicio > Cadastrar-se\n");
            Console.Write("Nome Completo: ");
            client.FullName = Console.ReadLine();
            Console.Write("Telefone: ");
            client.Phone = Console.ReadLine();
            Console.Write("E-mail: ");
            client.Email = Console.ReadLine();
            Console.Write("CPF: ");
            client.Cpf = Console.ReadLine();
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

            var signUp = clientDB.SignUp(client);

            if (signUp)
            {
                cartDB.EditUserId(client.Cpf);
                var total = cart.TotalCart(client.Cpf);
                if (home)
                {
                    Menu(total.TotalAmount, client.Cpf, home);
                }
                else
                {
                    payment.Methods(client.Cpf);
                }
            }
            else
            {
                Console.WriteLine("Tecle enter para continuar");
                Console.ReadKey();
                cartView.ListCart(clientDB.ClientId());
            }
            


        }

        public bool passwords()
        {
            var equal = false;

            Console.Write("\nDigite uma senha de usuário: ");
            password = ReadPassword();
            Console.Write("Confirmar senha: ");
            string confirmPassword = ReadPassword();

            if (password == confirmPassword)
            {
                equal = true;
            }

            return equal;
        }

        public void Menu(int countProductCart, string session, bool home)
        {
            var selectedProduct = new SelectedProductView();
            var client = new ClientService();
            var cart = new CartView();
            var inicio = new HomeView();
            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            Console.WriteLine("3. Desconectar-se");
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
                    inicio.ListProducts(countProductCart, session);
                    break;
                case 1:
                    inicio.Search(countProductCart, session);
                    break;
                case 2:
                    cart.ListCart(session);
                    break;
                case 3:
                    var ip = client.SignOut();
                    inicio.ListProducts(countProductCart, ip);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(countProductCart, session, home);
                    break;
            }
        }

        public string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }
    }
}
