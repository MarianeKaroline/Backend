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

            //Se a quantidade no carrinho for maior que 0, os produtos irão durar 20min
            //if (countProducts.TotalAmount > 0)
            //{
            //    var periodTimeSpan = TimeSpan.FromMinutes(20);

            //    var timer = new Timer((e) =>
            //    {
            //        cartService.RemoveAllCart(session);
            //    }, null, periodTimeSpan, periodTimeSpan);
            //}

            //Chama a home para ser exibida inicialmente
            HomeView inicio = new HomeView();
            inicio.ListProducts(countProducts.TotalAmount, session);
        }
    }
}
