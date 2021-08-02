using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class SignInView
    {
        public void Login(int countProductCart, string session, bool home)
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
                System.Console.WriteLine("Usuário não existe");
            }
            else
            {
                cartDB.EditUserId(sessionId);
            }

                var total = cart.TotalCart(sessionId);


            if (home)
            {
                Menu(total.TotalAmount, sessionId, home);
            }
            else
            {
                payment.Methods(sessionId);
            }
        }

        public void Menu(int countProductCart, string session, bool home)
        {
            var selectedProduct = new SelectedProductView();
            var client = new ClientService();
            var cart = new CartView();
            var inicio = new HomeView();
            var invalid = true;
            var op = 0;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            if (session.Length == 11)
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
                    inicio.ListProducts(countProductCart,session);
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
