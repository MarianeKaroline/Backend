using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class PerfilClientView
    {       
        public void Menu(List<BoughtModel> boughtModels, ParametersModel parameters)
        {
            bool validate = true;
            var homeView = new HomeView();
            var boughtsView = new BoughtsView();
            int opc = 0;

            Console.Clear();

            Console.WriteLine("\nSua conta\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver produtos comprados");
            Console.WriteLine("2. Alterar endereços");
            Console.WriteLine("3. Alterar cartões de pagamento");
            while (validate)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    homeView.ListProducts(parameters);
                    break;
                case 1:
                    boughtsView.Boughts(boughtModels, parameters);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }
    }
}
