using SingleExperience.Entities.DB;
using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.EmployeeServices;
using SingleExperience.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class EmployeeStatusBoughtView
    {
        private BoughtService boughtService = new BoughtService();
        private EmployeeService employeeService = new EmployeeService();
        private ProductService productService = new ProductService();
        private BoughtDB boughtDB = new BoughtDB();

        public void Bought(SessionModel parameters, StatusBoughtEnum status)
        {
            int j = 51;

            Console.Clear();

            Console.WriteLine($"\nAdministrador > Compras > {status}\n");

            employeeService.BoughtPendent(status).ForEach(i =>
            {
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Pedido n° {i.BoughtId}{new string(' ', j - $"Pedido n° {i.BoughtId}".Length)}|");
                Console.WriteLine($"|Pedido em {i.DateBought}{new string(' ', j - $"Pedido em {i.DateBought}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Status do pedido: {(StatusBoughtEnum)i.StatusId}{new string(' ', j - $"Status do pedido: {(StatusBoughtEnum)i.StatusId}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Endereço de entrega{new string(' ', j - "Endereço de entrega".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|{i.ClientName}{new string(' ', j - i.ClientName.Length)}|");
                Console.WriteLine($"|{i.Street}, {i.Number}{new string(' ', j - i.Street.Length - 2 - i.Number.Length)}|");
                Console.WriteLine($"|{i.City} - {i.State}{new string(' ', j - i.City.Length - 3 - i.State.Length)}|");
                Console.WriteLine($"|{i.Cep}{new string(' ', j - i.Cep.Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Forma de pagamento{new string(' ', j - $"Forma de pagamento".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");

                if (i.paymentMethod == PaymentMethodEnum.CreditCard)
                    Console.WriteLine($"|(Crédito) com final {i.NumberCard.Substring(12)}{new string(' ', j - $"(Crédito) com final {i.NumberCard.Substring(12)}".Length)}|");
                else if (i.paymentMethod == PaymentMethodEnum.BankSlip)
                    Console.WriteLine($"|(Boleto) {i.Code}{new string(' ', j - $"(Boleto) {i.Code}".Length)}|");
                else
                    Console.WriteLine($"|(PIX) {i.Pix}{new string(' ', j - $"(PIX) {i.Pix}".Length)}|");

                Console.WriteLine($"|{new string(' ', j)}|");

                Console.WriteLine($"+{new string('-', j)}+");
                Console.WriteLine($"|Resumo do pedido{new string(' ', j - "Resumo do pedido".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"|Subtotal do(s) item(ns): R$ {i.TotalPrice.ToString("F2")}{new string(' ', j - $"Subtotal do(s) item(ns): R$ {i.TotalPrice.ToString("F2")}".Length)}|");
                Console.WriteLine($"|Frete: R$ 0,00{new string(' ', j - "Frete: R$ 0,00".Length)}|");
                Console.WriteLine($"|Total do Pedido: R$ {i.TotalPrice.ToString("F2")}{new string(' ', j - $"Total do Pedido: R$ {i.TotalPrice.ToString("F2")}".Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");

                i.Itens.ForEach(k =>
                {
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"|{k.ProductName}{new string(' ', j - k.ProductName.Length)}|");
                    Console.WriteLine($"|Qtde: {k.Amount}{new string(' ', j - $"Qtde: {k.Amount}".Length)}|");
                    Console.WriteLine($"|R${k.Price}{new string(' ', j - $"R${k.Price}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                });
            });

            Menu(parameters, employeeService.BoughtPendent(StatusBoughtEnum.ConfirmacaoPendente), status);
        }

        public void Menu(SessionModel parameters, List<BoughtModel> list, StatusBoughtEnum status)
        {
            ClientHomeView homeView = new ClientHomeView();
            EmployeeListAllBoughtView listAllBought = new EmployeeListAllBoughtView();

            bool validate = true;
            int opc = 0;

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("100. Ver todas as compras");
            Console.WriteLine("101. Desconectar-se");

            if (status == StatusBoughtEnum.ConfirmacaoPendente)
            {
                Console.WriteLine("102. Confirmar um produto");
                Console.WriteLine("103. Cancelar um produto");
            }

            Console.WriteLine("9. Sair do programa");

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
                case 100:
                    listAllBought.Bought(parameters);
                    break;
                case 101:
                    parameters.Session = employeeService.SignOut();
                    homeView.ListProducts(parameters);
                    break;
                case 102:

                    Console.Write("\nDigite o código da compra que deseja confirmar: ");
                    var op = int.Parse(Console.ReadLine());

                    if (boughtService.HasBoughts(op))
                    {
                        var aux = list.FirstOrDefault(i => i.BoughtId == op).Itens;

                        boughtDB.UpdateStatus(op, StatusBoughtEnum.Confirmado);
                        var confirmed = productService.Confirm(aux);

                        if (confirmed)
                        {
                            Console.WriteLine("\nProduto confirmado com sucesso. (Tecle enter para continuar)\n");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("\nErro inesperado. (Tecle enter para continuar)\n");
                            Console.ReadKey();
                        }

                        Bought(parameters, status);
                    }
                    break;
                case 103:

                    Console.Write("\nDigite o código da compra que deseja cancelar: ");
                    var opt = int.Parse(Console.ReadLine());

                    if (boughtService.HasBoughts(opt))
                    {
                        var aux = list.FirstOrDefault(i => i.BoughtId == opt).Itens;

                        boughtDB.UpdateStatus(opt, StatusBoughtEnum.Cancelado);
                        var confirmed = productService.Confirm(aux);

                        if (confirmed)
                        {
                            Console.WriteLine("\nProduto cancelado com sucesso. (Tecle enter para continuar)\n");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("\nErro inesperado. (Tecle enter para continuar)\n");
                            Console.ReadKey();
                        }

                        Bought(parameters, status);
                    }
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente.");
                    Console.WriteLine("\nTente novamente");
                    Menu(parameters, list, status);
                    break;
            }
        }
    }
}
