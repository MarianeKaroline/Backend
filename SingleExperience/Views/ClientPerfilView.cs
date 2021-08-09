﻿using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class ClientPerfilView
    {       
        public void Menu(List<BoughtModel> boughtModels, ParametersModel parameters)
        {
            bool validate = true;
            var homeView = new ClientHomeView();
            var boughtsView = new ClientBoughtsView();
            var address = new ClientSendingAddressView();
            var card = new ClientPaymentMethodView();
            AddBoughtModel addBought = new AddBoughtModel();
            int opc = 0;

            Console.Clear();

            Console.WriteLine("\nSua conta\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver produtos comprados");
            Console.WriteLine("2. Seus endereços cadastrados");
            Console.WriteLine("3. Alterar cartões de pagamento");
            Console.WriteLine("9. Sair do Sistema");
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
                    address.ListAddress(parameters);
                    break;
                case 3:
                    card.CreditCard(parameters, addBought, true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(boughtModels, parameters);
                    break;
            }
        }
    }
}
