using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class EmployeePerfilView
    {
        public void Menu(ParametersModel parameters)
        {
            bool validate = true;
            var homeView = new ClientHomeView();
            var signUp = new EmployeeRegisterView();
            var employee = new EmployeeService();
            var employeeInventory = new EmployeeInventoryView();
            var allBought = new EmployeeListAllBoughtView();
            int opc = 0;

            Console.Clear();

            Console.WriteLine("\nAdministrador\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");
            Console.WriteLine("2. Ver funcionários cadastrados");
            Console.WriteLine("3. Estoque");
            Console.WriteLine("4. Desconectar-se");
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
                    allBought.Bought(parameters);
                    break;
                case 2:
                    signUp.ListEmployee(parameters);
                    break;
                case 3:
                    employeeInventory.Inventory(parameters);
                    break;
                case 4:
                    parameters.Session = employee.SignOut();
                    homeView.ListProducts(parameters);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters);
                    break;
            }
        }
    }
}
