using SingleExperience.Entities.DB;
using SingleExperience.Entities.CartEntities;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;

namespace SingleExperience.Views
{
    class SignInView
    {
        public void Login(ParametersModel parameters, bool home)
        {
            var inicio = new HomeView();
            var cart = new CartService();
            var cartDB = new CartDB();
            var signIn = new SignInModel();
            var client = new ClientService();
            var payment = new PaymentMethodView();
            Console.Clear();

            Console.WriteLine("Início > Login\n");
            Console.Write("Email: ");
            signIn.Email = Console.ReadLine();

            Console.Write("Senha: ");
            signIn.Password = ReadPassword();

            var sessionId = client.SignIn(signIn);

            if (sessionId == "")
            {
                System.Console.WriteLine("\nUsuário não existe");
                Console.WriteLine("Tecle enter para continuar");
                Console.ReadKey();
                inicio.ListProducts(parameters);
            }
            else
            {
                parameters.Session = sessionId;
                cartDB.PassItens(parameters);
                parameters.CartMemory = new List<ItemEntitie>();
            }

            parameters.CountProduct = cart.TotalCart(parameters).TotalAmount;


            if (home)
            {
                Menu(parameters, home);
            }
            else
            {
                payment.Methods(parameters);
            }
        }

        public void Menu(ParametersModel parameters, bool home)
        {
            var client = new ClientService();
            var cart = new CartView();
            var inicio = new HomeView();
            var invalid = true;
            var op = 0;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {parameters.CountProduct})");
            if (parameters.Session.Length == 11)
            {
                Console.WriteLine("3. Desconectar-se");
            }
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
                    inicio.ListProducts(parameters);
                    break;
                case 1:
                    inicio.Search(parameters);
                    break;
                case 2:
                    cart.ListCart(parameters);
                    break;
                case 3:
                    parameters.Session = client.SignOut();
                    inicio.ListProducts(parameters);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters, home);
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
