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

            cart.EditUserId(sessionId);

            if (home)
            {
                Menu(countProductCart, sessionId);
            }
            else
            {
                payment.Methods(sessionId);
            }
        }

        public void Menu(int countProductCart, string session)
        {
            var selectedProduct = new SelectedProductView();
            var client = new ClientService();
            var cart = new CartView();
            var inicio = new HomeView();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {countProductCart})");
            if (session.Length == 11)
            {
                Console.WriteLine("3. Desconectar-se");
            }
            Console.WriteLine("Digite o codigo do produto # para mais detalhes\n");
            int op = int.Parse(Console.ReadLine());

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
                    client.SignOut();
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
