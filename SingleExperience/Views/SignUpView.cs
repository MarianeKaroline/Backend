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

            var msg = clientDB.SignUp(client);

            Console.WriteLine(msg);
            Console.WriteLine("Tecle enter para continuar");
            Console.ReadKey();

            cartDB.EditUserId(client.Cpf);
            if (home)
            {
                Menu(countProductCart, client.Cpf);
            }
            else
            {
                payment.Methods(client.Cpf);
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

        public void Menu(int countProductCart, string session)
        {
            var selectedProduct = new SelectedProductView();
            var cart = new CartView();
            var inicio = new HomeView();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            Console.WriteLine("3. Desconectar-se");
            int op = int.Parse(Console.ReadLine());

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
                default:
                    selectedProduct.SelectedProduct(op, countProductCart, session);
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
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
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
