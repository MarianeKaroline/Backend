using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class EmployeePerfilView
    {

        private EmployeeService employeeService = new EmployeeService();
        private EmployeeDB employeeDB = new EmployeeDB();

        public void Menu(SessionModel parameters)
        {
            EmployeeInventoryView employeeInventory = new EmployeeInventoryView();
            EmployeeListAllBoughtView allBought = new EmployeeListAllBoughtView();
            EmployeeRegisterView signUp = new EmployeeRegisterView();
            ClientHomeView homeView = new ClientHomeView();

            bool validate = true;
            int opc = 0;

            var aux = employeeDB.Access(parameters.Session);

            Console.Clear();

            Console.WriteLine("\nAdministrador\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");
            if (aux.AccessRegister)
            {
                Console.WriteLine("2. Ver funcionários cadastrados");
            }
            if (aux.AccessInventory)
            {
                Console.WriteLine("3. Estoque");
            }
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
                    if (aux.AccessRegister)
                    {
                        signUp.ListEmployee(parameters);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(parameters);
                    }
                    break;

                case 3:
                    if (aux.AccessInventory)
                    {
                        employeeInventory.Inventory(parameters);
                    }
                    else
                    {
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        Menu(parameters);
                    }
                    break;

                case 4:
                    parameters.Session = employeeService.SignOut();
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
