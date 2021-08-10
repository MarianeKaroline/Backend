using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class ClientProductsBoughtView
    {
        public void ProductsBought(List<BoughtModel> boughtModels, SessionModel parameters, int boughtId)
        {
            int j = 51;

            Console.Clear();

            Console.WriteLine("\nSua conta > Seus pedidos > Dados do pedido\n");

            boughtModels
                .Where(k => k.BoughtId == boughtId)
                .ToList()
                .ForEach(i =>
                {
                    Console.WriteLine($"+{new string('-', j)}+");
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
            Menu(boughtModels, parameters);
        }
        public void Menu(List<BoughtModel> boughtModels, SessionModel parameters)
        {
            var homeView = new ClientHomeView();
            var perfilView = new ClientPerfilView();
            bool validate = true;
            int opc = 0;

            Console.WriteLine("\n\n0. Voltar para o início");
            Console.WriteLine("1. Voltar para sua conta");
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

            if (opc == 0)
            {
                homeView.ListProducts(parameters);
            }
            else
            {
                perfilView.Menu(boughtModels, parameters);
            }
        }
    }
}
