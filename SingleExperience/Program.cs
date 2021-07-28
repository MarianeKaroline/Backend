using System;
using SingleExperience.Views;
namespace SingleExperience
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chama a home para ser exibida inicialmente
            HomeView inicio = new HomeView();
            inicio.ListProducts(0);
        }
    }
}
