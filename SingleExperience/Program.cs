using System;
using SingleExperience.Views;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices;
using System.Threading;
using SingleExperience.Entities.DB;

namespace SingleExperience
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chama a função para pegar o IP do PC
            ClientDB client = new ClientDB();
            var session = client.ClientId();

            //Chama a função para pegar a quantidade que está no carrinho
            CartService cartService = new CartService();
            var countProducts = cartService.TotalCart(session);

            //Chama a home para ser exibida inicialmente
            HomeView inicio = new HomeView();
            inicio.ListProducts(countProducts.TotalAmount, session);
        }
    }
}
