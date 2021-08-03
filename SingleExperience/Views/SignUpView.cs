using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
using System.Linq;

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
            var validate = true;
            var validateNumber = true;
            var validateCep = true;
            var validatePhone = true;
            var validateBirth = true;
            var j = 41;

            Console.Clear();

            Console.WriteLine("Inicio > Cadastrar-se\n");
            Console.WriteLine("Informações pessoais\n");
            Console.Write("Nome Completo: ");
            client.FullName = Console.ReadLine();

            while (validatePhone)
            {
                try
                {
                    Console.Write("Telefone: ");            
                    string phone = Console.ReadLine();
                    if (phone.All(char.IsDigit))
                    {
                        client.Phone = phone;
                        validatePhone = false;
                    }
                    else
                    {
                        Console.WriteLine("O número de telefone deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O número de telefone deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }
                        
            Console.Write("E-mail: ");
            client.Email = Console.ReadLine();

            while (validate)
            {
                try
                {
                    Console.Write("CPF: ");
                    string cpf = Console.ReadLine();
                    if (cpf.All(char.IsDigit) && cpf.Length == 11)
                    {
                        client.UserId = cpf;
                        validate = false;
                    }
                    else
                    {
                        Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                        Console.WriteLine("Por favor, tente novamente.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }     

            while (validateBirth)
            {
                try
                {
                    Console.Write("Data de Nascimento: (00/00/0000) ");
                    DateTime birthDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    client.BirthDate = birthDate;
                    validateBirth = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }
            
            var equal = passwords();

            if (!equal)
            {
                Console.WriteLine("As senhas são diferentes, tente novamente. (Tecle enter para continuar)");
                Console.ReadKey();
                equal = passwords();
            }
            if (equal)
            {
                client.Password = password;
            }

            Console.WriteLine($"\n+{new string('-', j)}+\n");

            Console.WriteLine("Endereço\n");

            while (validateCep)
            {
                try
                {
                    Console.Write("CEP: ");
                    string cep = Console.ReadLine();
                    if (cep.All(char.IsDigit))
                    {
                        client.Cep = cep;
                        validateCep = false;
                    }
                    else
                    {
                        Console.WriteLine("O cep deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O cep deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.\n");
                }
            }

            Console.Write("Rua: ");
            client.Street = Console.ReadLine();

            while (validateNumber)
            {
                try
                {
                    Console.Write("Número: ");
                    string number = Console.ReadLine();
                    if (number.All(char.IsDigit))
                    {
                        client.Number = number;
                        validateNumber = false;
                    }
                    else
                    {
                        Console.WriteLine("O número de residência deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O número de residência deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.\n");
                }
            }

            Console.Write("Cidade: ");
            client.City = Console.ReadLine();
            Console.Write("Estado: ");
            client.State = Console.ReadLine();

            var signUp = clientDB.SignUp(client);

            if (signUp)
            {
                cartDB.EditUserId(client.UserId);
                var total = cart.TotalCart(client.UserId);
                if (home)
                {
                    Menu(total.TotalAmount, client.UserId, home);
                }
                else
                {
                    payment.Methods(client.UserId);
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
