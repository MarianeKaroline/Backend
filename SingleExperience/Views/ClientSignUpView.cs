﻿using SingleExperience.Entities.DB;
using SingleExperience.Entities.CartEntities;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SingleExperience.Services.EmployeeServices.Models;

namespace SingleExperience.Views
{
    class ClientSignUpView
    {
        public string password = null;
        public void SignUp(ParametersModel parameters, bool home)
        {
            var sendingAddress = new ClientSendingAddressView();
            var cartView = new ClientCartView();
            var cart = new CartService();
            var client = new SignUpModel();
            var cartDB = new CartDB();
            var clientDB = new ClientDB();
            var validate = true;
            var validatePhone = true;
            var validateBirth = true;

            Console.Clear();

            if (home)
            {
                Console.WriteLine("Inicio > Cadastrar-se\n");
            }
            else
            {
                Console.WriteLine("\nCarrinho > Informações pessoais\n");
            }

            Console.WriteLine("Informações pessoais\n");
            Console.Write("Nome Completo: ");
            client.FullName = Console.ReadLine();

            while (validatePhone)
            {
                try
                {
                    Console.Write("Telefone: ");            
                    string phone = Console.ReadLine();
                    if (phone.All(char.IsDigit))
                    {
                        client.Phone = phone;
                        validatePhone = false;
                    }
                    else
                    {
                        Console.WriteLine("O número de telefone deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O número de telefone deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }
                        
            Console.Write("E-mail: ");
            client.Email = Console.ReadLine();

            while (validate)
            {
                try
                {
                    Console.Write("CPF: ");
                    string cpf = Console.ReadLine();
                    if (cpf.All(char.IsDigit) && cpf.Length == 11)
                    {
                        client.Cpf = cpf;
                        validate = false;
                    }
                    else
                    {
                        Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                        Console.WriteLine("Por favor, tente novamente.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }     

            while (validateBirth)
            {
                try
                {
                    Console.Write("Data de Nascimento: (00/00/0000) ");
                    DateTime birthDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    client.BirthDate = birthDate;
                    validateBirth = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }
            
            var equal = passwords();

            if (!equal)
            {
                Console.WriteLine("As senhas são diferentes, tente novamente. (Tecle enter para continuar)");
                Console.ReadKey();
                equal = passwords();
            }
            if (equal)
            {
                client.Password = password;
            }                       

            var signUp = clientDB.SignUp(client);

            if (signUp)
            {
                parameters.Session = client.Cpf;
                cartDB.PassItens(parameters);
                parameters.CartMemory = new List<ItemEntitie>();
                parameters.CountProduct = cart.TotalCart(parameters).TotalAmount;
                if (home)
                {
                    Menu(parameters, home);
                }
                else
                {
                    sendingAddress.Address(parameters);
                }
            }
            else
            {
                parameters.Session = clientDB.ClientId();
                Console.WriteLine("Tecle enter para continuar");
                Console.ReadKey();
                cartView.ListCart(parameters);
            }         

        }

        public bool passwords()
        {
            var equal = false;

            Console.Write("\nDigite uma senha de usuário: ");
            password = ReadPassword();
            Console.Write("Confirmar senha: ");
            string confirmPassword = ReadPassword();

            if (password == confirmPassword)
            {
                equal = true;
            }

            return equal;
        }

        public void Menu(ParametersModel parameters, bool home)
        {
            var cartService = new CartService();
            var client = new ClientService();
            var cart = new ClientCartView();
            var inicio = new ClientHomeView();
            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Ver Carrinho (quantidade: {parameters.CountProduct})");
            Console.WriteLine("3. Desconectar-se");
            while (invalid)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }

            }

            switch (op)
            {
                case 0:
                    inicio.ListProducts(parameters);
                    break;
                case 1:
                    inicio.Search(parameters);
                    break;
                case 2:
                    cart.ListCart(parameters);
                    break;
                case 3:
                    parameters.Session = client.SignOut();
                    parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                    inicio.ListProducts(parameters);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters, home);
                    break;
            }
        }

        public string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return password;
        }

        public void SignUpEmployee(ParametersModel parameters)
        {
            var sendingAddress = new ClientSendingAddressView();
            var employeePerfil = new EmployeePerfilView();
            var employeeRegister = new EmployeeRegisterView();
            var cartView = new ClientCartView();
            var cart = new CartService();
            var employee = new SignUpEmployeeModel();
            var cartDB = new CartDB();
            var clientDB = new ClientDB();
            var employeeDB = new EmployeeDB();
            var validate = true;

            Console.Clear();
            
            Console.WriteLine("\nAdministrador > Cadastrar funcionário\n");

            Console.Write("Nome Completo: ");
            employee.FullName = Console.ReadLine();            

            Console.Write("E-mail: ");
            employee.Email = Console.ReadLine();

            while (validate)
            {
                try
                {
                    Console.Write("CPF: ");
                    string cpf = Console.ReadLine();
                    if (cpf.All(char.IsDigit) && cpf.Length == 11)
                    {
                        employee.Cpf = cpf;
                        validate = false;
                    }
                    else
                    {
                        Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                        Console.WriteLine("Por favor, tente novamente.");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O cpf deve conter apenas números e deve conter 11 digitos.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            var equal = passwords();

            if (!equal)
            {
                Console.WriteLine("As senhas são diferentes, tente novamente. (Tecle enter para continuar)");
                Console.ReadKey();
                equal = passwords();
            }
            if (equal)
            {
                employee.Password = password;
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Acesso ao estoque: ");
                    employee.AccessInventory = bool.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            validate = true;
            while (validate)
            {
                try
                {
                    Console.Write("Acesso aos funcionários cadastrados: ");
                    employee.RegisterEmployee = bool.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor inválido.");
                    Console.WriteLine("Por favor, tente novamente.");
                }
            }

            var signUp = employeeDB.Register(employee);
            if (signUp)
            {
                Console.WriteLine("\nFuncionário cadastrado com sucesso\n");
            }
            else
            {
                Console.WriteLine("\nErro inesperado!\n");
            }

            Console.WriteLine("Tecle enter para continuar");
            Console.ReadKey();
            employeeRegister.ListEmployee(parameters);

        }
    }
}