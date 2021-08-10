using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class EmployeeRegisterView
    {
        private EmployeeListAllBoughtView allBought = new EmployeeListAllBoughtView();
        private EmployeeService employeeService = new EmployeeService();
        private EmployeeInventoryView employeeInventory = new EmployeeInventoryView();
        private EmployeeDB employeeDB = new EmployeeDB();

        public void ListEmployee(SessionModel parameters)
        {
            var signUp = new ClientSignUpView();
            var j = 51;
            char opc = '\0';
            var invalid = true;
            var employeeService = new EmployeeService();

            Console.Clear();

            Console.WriteLine("\nAdministrador > Lista funcionários\n");

            employeeService.listEmployee().ForEach(i =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|CPF: {i.Cpf}{new string(' ', j - $"CPF: {i.Cpf}".Length)}|");
                Console.WriteLine($"|Nome Completo: {i.FullName}{new string(' ', j - $"Nome Completo: {i.FullName}".Length)}|");
                Console.WriteLine($"|Email: {i.Email}{new string(' ', j - $"Email: {i.Email}".Length)}|");
                Console.WriteLine($"|Acesso ao inventário: {i.AccessInventory}{new string(' ', j - $"Acesso ao inventário: {i.AccessInventory}".Length)}|");
                Console.WriteLine($"|Acesso ao Regitro de funcionário: {i.RegisterEmployee}{new string(' ', j - $"Acesso ao Regitro de funcionário: {i.RegisterEmployee}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");
            });

            Console.Write("Adicionar um novo funcionário? (s/n) \n");
            while (invalid)
            {
                try
                {
                    opc = char.Parse(Console.ReadLine().ToLower());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nOpção inválida, tente novamente.\n");
                }
            }

            switch (opc)
            {
                case 's':
                    signUp.SignUpEmployee(parameters);
                    break;
                case 'n':
                    Menu(parameters);
                    break;
                default:
                    break;
            }
        }
        public void Menu(SessionModel parameters)
        {
            ClientHomeView homeView = new ClientHomeView();

            bool validate = true;
            int opc = 0;

            var aux = employeeDB.Access(parameters.Session);

            Console.Clear();

            Console.WriteLine("\nAdministrador\n");

            Console.WriteLine("0. Voltar para o início");
            Console.WriteLine("1. Ver lista de compras");

            if (aux.AccessInventory)
            {
                Console.WriteLine("2. Estoque");
            }

            Console.WriteLine("3. Desconectar-se");
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
                case 3:
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
