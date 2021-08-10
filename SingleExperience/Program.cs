using SingleExperience.Views;
using SingleExperience.Services.CartServices;
using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices.Models;

namespace SingleExperience
{
    class Program
    {
        static void Main(string[] args)
        {
            //Carrinho memória
            SessionModel parameters = new SessionModel();
            CartModel model = new CartModel();
            CartDB cart = new CartDB();
            parameters.CartMemory = cart.AddItensMemory(model, parameters.CartMemory);            

            //Chama a função para pegar o IP do PC
            ClientDB client = new ClientDB();
            parameters.Session = client.GetIP();

            //Chama a função para pegar a quantidade que está no carrinho
            CartService cartService = new CartService();
            var countProducts = cartService.TotalCart(parameters);
            parameters.CountProduct = countProducts.TotalAmount;

            //Chama a home para ser exibida inicialmente
            ClientHomeView inicio = new ClientHomeView();
            inicio.ListProducts(parameters);
        }
    }
}
