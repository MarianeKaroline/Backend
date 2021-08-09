using SingleExperience.Entities.DB;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class ClientSendingAddressView
    {
        public void ListAddress(ParametersModel parameters)
        {
            var boughtService = new BoughtService();
            var clientService = new ClientService();
            var opc = '\0';
            var validate = true;
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nSua conta > Seus endereços cadastrados\n");

            if (boughtService.HasAddress(parameters.Session))
            {
                clientService.ShowAddress(parameters.Session)
                    .ForEach(p =>
                    {
                        Console.WriteLine($"+{new string('-', j)}+");
                        Console.WriteLine($"|{new string(' ', j)}|");
                        Console.WriteLine($"|Endereço #{p.AddressId}{new string(' ', j - $"Endereço #{p.AddressId}".Length)}|");
                        Console.WriteLine($"|{p.ClientName}{new string(' ', j - $"{p.ClientName}".Length)}|");
                        Console.WriteLine($"|{p.Street}, {p.Number}{new string(' ', j - $"{p.Street}, {p.Number}".Length)}|");
                        Console.WriteLine($"|{p.City} - {p.State}{new string(' ', j - $"{p.City} - {p.State}".Length)}|");
                        Console.WriteLine($"|Telefone: {p.ClientPhone}{new string(' ', j - $"Telefone: {p.ClientPhone}".Length)}|");
                        Console.WriteLine($"+{new string('-', j)}+");
                    });

                Console.WriteLine("Deseja cadastrar um novo endereço? (s/n)");

                while (validate)
                {
                    try
                    {
                        opc = char.Parse(Console.ReadLine().ToLower());
                        validate = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nOpção inválida, tente novamente.\n");
                    }
                }

                switch (opc)
                {
                    case 's':
                        AddNewAddress(parameters, true);
                        break;
                    case 'n':
                        Menu(parameters);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Você não possui nenhum endereço cadastrado");
            }
        }

        public void Address(ParametersModel parameters)
        {
            var paymentMethod = new ClientPaymentMethodView();
            var boughtService = new BoughtService();
            var clientService = new ClientService();
            var opc = '\0';
            var validate = true;
            var j = 41;

            Console.Clear();

            Console.WriteLine("\nCarrinho > Informações pessoais > Endereço\n");

            if (boughtService.HasAddress(parameters.Session))
            {
                Console.WriteLine($"Endereços cadastrados");
                clientService.ShowAddress(parameters.Session)
                    .ForEach(p =>
                    {
                        Console.WriteLine($"+{new string('-', j)}+");
                        Console.WriteLine($"|{new string(' ', j)}|");
                        Console.WriteLine($"|Endereço #{p.AddressId}{new string(' ', j - $"Endereço #{p.AddressId}".Length)}|");
                        Console.WriteLine($"|{p.ClientName}{new string(' ', j - $"{p.ClientName}".Length)}|");
                        Console.WriteLine($"|{p.Street}, {p.Number}{new string(' ', j - $"{p.Street}, {p.Number}".Length)}|");
                        Console.WriteLine($"|{p.City} - {p.State}{new string(' ', j - $"{p.City} - {p.State}".Length)}|");
                        Console.WriteLine($"|Telefone: {p.ClientPhone}{new string(' ', j - $"Telefone: {p.ClientPhone}".Length)}|");
                        Console.WriteLine($"+{new string('-', j)}+");
                    });

                Console.Write("\nEscolher um desses endereços: (s/n) ");

                while (validate)
                {
                    try
                    {
                        opc = char.Parse(Console.ReadLine().ToLower());
                        validate = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nOpção inválida, tente novamente.\n");
                    }
                }

                switch (opc)
                {
                    case 's':
                        validate = true;
                        var op = 0;

                        Console.Write("\nDigite o código do endereço #: ");

                        while (validate)
                        {
                            try
                            {
                                op = int.Parse(Console.ReadLine());
                                validate = false;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("\nOpção inválida, tente novamente.\n");
                            }
                        }

                        paymentMethod.Methods(parameters, op);

                        break;
                    case 'n':
                        AddNewAddress(parameters, false);
                        break;
                    default:
                        Console.WriteLine("\nEssa opção não existe. Tente novamente. (Tecle enter para continuar)\n");
                        Console.ReadKey();
                        Address(parameters);
                        break;
                }
            }
            else
            {
                AddNewAddress(parameters, false);
            }
        }

        public void AddNewAddress(ParametersModel parameters, bool home)
        {
            var addressModel = new AddressModel();
            var clientDB = new ClientDB();
            var paymentMethod = new ClientPaymentMethodView();
            var validateNumber = true;
            var validateCep = true;

            while (validateCep)
            {
                try
                {
                    Console.Write("\nCEP: ");
                    string cep = Console.ReadLine();
                    if (cep.All(char.IsDigit))
                    {
                        addressModel.Cep = cep;
                        validateCep = false;
                    }
                    else
                    {
                        Console.WriteLine("O cep deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O cep deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.\n");
                }
            }

            Console.Write("Rua: ");
            addressModel.Street = Console.ReadLine();

            while (validateNumber)
            {
                try
                {
                    Console.Write("Número: ");
                    string number = Console.ReadLine();
                    if (number.All(char.IsDigit))
                    {
                        addressModel.Number = number;
                        validateNumber = false;
                    }
                    else
                    {
                        Console.WriteLine("O número de residência deve conter apenas números.");
                        Console.WriteLine("Por favor, tente novamente.\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("O número de residência deve conter apenas números.");
                    Console.WriteLine("Por favor, tente novamente.\n");
                }
            }

            Console.Write("Cidade: ");
            addressModel.City = Console.ReadLine();
            Console.Write("Estado: ");
            addressModel.State = Console.ReadLine();
            addressModel.ClientId = parameters.Session;

            var addressId = clientDB.AddAddress(parameters.Session, addressModel);

            if (home)
            {
                ListAddress(parameters);
            }
            else
            {
                paymentMethod.Methods(parameters, addressId);
            }
        }

        public void Menu(ParametersModel parameters)
        {
            var home = new ClientHomeView();
            var cartService = new CartService();
            var client = new ClientService();
            var cart = new ClientCartView();
            var opc = 0;
            var invalid = true;

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Voltar para o início");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {parameters.CountProduct})");
            Console.WriteLine("3. Desconectar-se");
            Console.WriteLine("9. Sair do Sistema");
            while (invalid)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");
                    Console.WriteLine("Tecle enter para continuar");
                    Console.ReadLine();

                    home.ListProducts(parameters);
                    break;
                case 1:
                    home.ListProducts(parameters);
                    break;
                case 2:
                    cart.ListCart(parameters);
                    break;
                case 3:
                    parameters.Session = client.SignOut();
                    parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                    home.ListProducts(parameters);
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
