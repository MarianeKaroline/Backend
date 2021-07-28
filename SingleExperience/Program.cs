using System;
using SingleExperience.Views;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices;
using System.Threading;

namespace SingleExperience
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chama a função para pegar o IP do PC
            ClientService client = new ClientService();
            string ipComputer = client.ClientId();

            //Chama a função para pegar a quantidade que está no carrinho
            CartService cartService = new CartService();
            int countProducts = cartService.CountProducts(ipComputer);

            //Se a quantidade no carrinho for maior que 0, os produtos irão durar 20min
            if (countProducts > 0)
            {
                var periodTimeSpan = TimeSpan.FromMinutes(20);

                var timer = new Timer((e) =>
                {
                    cartService.CleanAllCart(ipComputer);
                }, null, periodTimeSpan, periodTimeSpan);
            }

            //Chama a home para ser exibida inicialmente
            HomeView inicio = new HomeView();
            inicio.ListProducts(countProducts, ipComputer);
        }
    }
}
