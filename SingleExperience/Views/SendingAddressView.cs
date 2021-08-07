using SingleExperience.Entities.DB;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class SendingAddressView
    {
        public void Address(ParametersModel parameters)
        {
            var paymentMethod = new PaymentMethodView();
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
                        AddNewAddress(parameters);
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
                AddNewAddress(parameters);
            }
        }

        public void AddNewAddress(ParametersModel parameters)
        {
            var addressModel = new AddressModel();
            var clientDB = new ClientDB();
            var paymentMethod = new PaymentMethodView();
            var validateNumber = true;
            var validateCep = true;

            while (validateCep)
            {
                try
                {
                    Console.Write("CEP: ");
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

            paymentMethod.Methods(parameters, addressId);
        }
    }
}
