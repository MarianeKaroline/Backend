using SingleExperience.Entities.DB;
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

        private SignUpModel clientModel = new SignUpModel();
        private CartDB cartDB = new CartDB();
        private ClientDB clientDB = new ClientDB();
        private CartService cartService = new CartService();
        private ClientService clientService = new ClientService();
        private SignUpEmployeeModel employee = new SignUpEmployeeModel();
        private EmployeeDB employeeDB = new EmployeeDB();


        public string password = null;
        public void SignUp(SessionModel parameters, bool home)
        {
            ClientSendingAddressView sendingAddress = new ClientSendingAddressView();
            ClientCartView cartView = new ClientCartView();
            ClientCartView cart = new ClientCartView();
            ClientHomeView inicio = new ClientHomeView();
            EmployeeRegisterView employeeRegister = new EmployeeRegisterView();

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
            clientModel.FullName = Console.ReadLine();

            while (validatePhone)
            {
                try
                {
                    Console.Write("Telefone: ");
                    string phone = Console.ReadLine();
                    if (phone.All(char.IsDigit))
                    {
                        clientModel.Phone = phone;
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
            clientModel.Email = Console.ReadLine();

            while (validate)
            {
                try
                {
                    Console.Write("CPF: ");
                    string cpf = Console.ReadLine();
                    if (cpf.All(char.IsDigit) && cpf.Length == 11)
                    {
                        clientModel.Cpf = cpf;
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
                    clientModel.BirthDate = birthDate;
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
                clientModel.Password = password;
            }

            var signUp = clientDB.SignUp(clientModel);

            if (signUp)
            {
                parameters.Session = clientModel.Cpf;
                cartDB.PassItens(parameters);
                parameters.CartMemory = new List<ItemEntitie>();
                parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
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
                parameters.Session = clientDB.GetIP();
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

        public void Menu(SessionModel parameters, bool home)
        {
            ClientCartView cart = new ClientCartView();
            ClientHomeView inicio = new ClientHomeView();

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
                    parameters.Session = clientService.SignOut();
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

        public void SignUpEmployee(SessionModel parameters)
        {
            ClientSendingAddressView sendingAddress = new ClientSendingAddressView();
            ClientCartView cartView = new ClientCartView();
            ClientCartView cart = new ClientCartView();
            ClientHomeView inicio = new ClientHomeView();
            EmployeeRegisterView employeeRegister = new EmployeeRegisterView();

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
