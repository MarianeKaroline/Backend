using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class SignInView
    {
        public void Login(int countProductCart, string ipComputer)
        {
            SignInModel signIn = new SignInModel();
            ClientService client = new ClientService();
            Console.Clear();

            Console.WriteLine("Início > Login\n");
            Console.Write("Email: ");
            signIn.Email = Console.ReadLine();

            Console.Write("Senha: ");
            signIn.Password = Console.ReadLine();

            var sessionId = client.SignIn(signIn);
            Menu(countProductCart, ipComputer, sessionId);
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
