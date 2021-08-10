﻿using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SingleExperience.Views
{
    class ClientPreviewBoughtView
    {
        private CartService cart = new CartService();

        public void Bought(SessionModel parameters, AddBoughtModel addBought)
        {
            var ids = new List<int>();
            var bought = new BuyModel();

            bought.Session = parameters.Session;
            bought.Method = addBought.Payment;
            bought.Confirmation = addBought.CodeConfirmation;
            bought.Status = StatusProductEnum.Ativo;
            bought.Ids = ids;

            var data = cart.PreviewBoughts(parameters, bought, addBought.AddressId);
            var total = cart.TotalCart(parameters);
            var listConfirmation = new List<BuyProductModel>();
            var j = 51;


            Console.Clear();
            Console.WriteLine("\nCarrinho > Informações pessoais > Método de pagamento > Confirma compra\n");

            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|Endereço de entrega{new string(' ', j - "Endereço de entrega".Length)}|");
            Console.WriteLine($"|{data.FullName}{new string(' ', j - data.FullName.Length)}|");
            Console.WriteLine($"|{data.Street}, {data.Number}{new string(' ', j - data.Street.Length - 2 - data.Number.Length)}|");
            Console.WriteLine($"|{data.City} - {data.State}{new string(' ', j - data.City.Length - 3 - data.State.Length)}|");
            Console.WriteLine($"|{data.Cep}{new string(' ', j - data.Cep.Length)}|");
            Console.WriteLine($"|Telefone: {data.Phone}{new string(' ', j - $"Telefone: {data.Phone}".Length)}|");
            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|Forma de pagamento{new string(' ', j - $"Forma de pagamento".Length)}|");

            if (addBought.Payment == PaymentMethodEnum.CreditCard)
                Console.WriteLine($"|(Crédito) com final {data.NumberCard.Substring(12)}{new string(' ', j - $"(Crédito) com final {data.NumberCard.Substring(12)}".Length)}|");
            else if (addBought.Payment == PaymentMethodEnum.BankSlip)
                Console.WriteLine($"|(Boleto) {data.Code}{new string(' ', j - $"(Boleto) {data.Code}".Length)}|");
            else
                Console.WriteLine($"|(PIX) {data.Pix}{new string(' ', j - $"(PIX) {data.Pix}".Length)}|");

            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"+{new string('-', j)}+");

            data.Itens.ForEach(i =>
            {
                Console.WriteLine($"|#{i.ProductId}{new string(' ', j - 1 - i.ProductId.ToString().Length)}|");
                Console.WriteLine($"|{i.Name}{new string(' ', j - i.Name.Length)}|");
                Console.WriteLine($"|Qtde: {i.Amount}{new string(' ', j - 6 - i.Amount.ToString().Length)}|");
                Console.WriteLine($"|{i.Price.ToString("F2", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - i.Price.ToString().Length)}|");
                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");


                var bought = new BuyProductModel();

                bought.ProductId = i.ProductId;
                bought.Amount = i.Amount;
                bought.Status = 0;

                listConfirmation.Add(bought);
            });

            Console.WriteLine("\nResumo do pedido");
            Console.WriteLine($"Itens: R$ {total.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine("Frete: R$ 0,00");
            Console.WriteLine($"\nTotal do Pedido: R$ {total.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)}");

            addBought.TotalPrice = total.TotalPrice;
            addBought.BuyProducts = listConfirmation;
            Menu(parameters, addBought);
        }

        public void Menu(SessionModel parameters, AddBoughtModel addBought)
        {
            ClientFinishedView finished = new ClientFinishedView();
            ClientCartView cartView = new ClientCartView();

            var total = cart.TotalCart(parameters);
            var validate = true;
            var op = 0;

            Console.WriteLine("\n1. Confirmar Compra");
            Console.WriteLine($"2. Voltar para o carrinho {total.TotalAmount}");
            while (validate)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    validate = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.\n");
                }
            }

            switch (op)
            {
                case 1:
                    addBought.BuyProducts.ForEach(i =>
                    {
                        i.Status = StatusProductEnum.Comprado;
                    });

                    finished.ProductsBought(parameters, addBought);
                    break;
                case 2:
                    cartView.ListCart(parameters);
                    break;
                default:
                    Console.WriteLine("Opção inválida, tente novamente");
                    Menu(parameters, addBought);
                    break;
            }
        }
    }
}
