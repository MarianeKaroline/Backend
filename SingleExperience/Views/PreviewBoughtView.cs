using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices.Models;
using SingleExperience.Services.ProductServices.Models.CartModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SingleExperience.Views
{
    class PreviewBoughtView
    {
        private CartService cart;

        public PreviewBoughtView()
        {
            cart = new CartService();
        }
        public void Bought(string session, Enum method, string lastNumbers)
        {
            var payment = (PaymentMethodEnum)method;
            var total = cart.TotalCart(session);
            var listConfirmation = new List<BuyProductModel>();
            var j = 41;

            Console.WriteLine("\nCarrinho > Informações pessoais > Método de pagamento > Confirma compra\n");

            Console.Clear();
            cart.PreviewBoughts(session, payment, lastNumbers, StatusProductEnum.Ativo)
                .ForEach(p => 
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|Endereço de entrega{new string(' ', j - "Endereço de entrega".Length)}|");
                    Console.WriteLine($"|{p.FullName}{new string(' ', j - p.FullName.Length)}|");
                    Console.WriteLine($"|{p.Street}, {p.Number}{new string(' ', j - p.Street.Length - 2 - p.Number.Length)}|");
                    Console.WriteLine($"|{p.City} - {p.State}{new string(' ', j - p.City.Length - 3 - p.State.Length)}|");
                    Console.WriteLine($"|{p.Cep}{new string(' ', j - p.Cep.Length)}|");
                    Console.WriteLine($"|Telefone: {p.Phone}{new string(' ', j - $"Telefone: {p.Phone}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|Forma de pagamento{new string(' ', j - $"Forma de pagamento".Length)}|");
                    if (payment == PaymentMethodEnum.CreditCard)
                        Console.WriteLine($"|(Crédito) com final {p.NumberCard.Substring(12)}{new string(' ', j - $"(Crédito) com final {p.NumberCard.Substring(12)}".Length)}|");
                    else if (payment == PaymentMethodEnum.BankSlip)
                        Console.WriteLine($"|(Boleto) {p.Code}{new string(' ', j - $"(Boleto) {p.Code}".Length)}|");
                    else
                        Console.WriteLine($"|(PIX) {p.Pix}{new string(' ', j - $"(PIX) {p.Pix}".Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                    var item = p.Itens
                    .GroupBy(j => j.Name)
                    .Select(i => new ProductCartModel()
                    {
                        ProductId = i.First().ProductId,
                        Name = i.First().Name,
                        Price = i.First().Price,
                        StatusId = i.First().StatusId,
                        CategoryId = i.First().CategoryId,
                        Amount = i.Sum(j => j.Amount)
                    })
                    .ToList();
                    item.ForEach(i =>
                    {
                        Console.WriteLine($"|#{i.ProductId}{new string(' ', j - 1 - i.ProductId.ToString().Length)}|");
                        Console.WriteLine($"|{i.Name}{new string(' ', j - i.Name.Length)}|");
                        Console.WriteLine($"|Qtde: {i.Amount}{new string(' ', j - 6 -  i.Amount.ToString().Length)}|");
                        Console.WriteLine($"|{i.Price.ToString("F2", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - i.Price.ToString().Length)}|");
                        Console.WriteLine($"|{new string(' ', j)}|");
                        Console.WriteLine($"+{new string('-', j)}+");


                        var bought = new BuyProductModel();

                        bought.ProductId = i.ProductId;
                        bought.Amount = i.Amount;
                        bought.Status = 0;

                        listConfirmation.Add(bought);
                    });
                });

            Console.WriteLine("\nResumo do pedido");
            Console.WriteLine($"Itens: R$ {total.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine("Frete: R$ 0,00");
            Console.WriteLine($"\nTotal do Pedido: R$ {total.TotalPrice.ToString("F2", CultureInfo.InvariantCulture)}");

            Menu(listConfirmation, session, method, lastNumbers, total.TotalPrice);
        }

        public void Menu(List<BuyProductModel> list, string session, Enum method, string lastNumbers, double totalPrice)
        {
            var total = cart.TotalCart(session);
            var finished = new FinishedView();
            var cartView = new CartView();

            Console.WriteLine("\n1. Confirmar Compra");
            Console.WriteLine($"2. Voltar para o carrinho {total.TotalAmount}");
            var op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    list.ForEach(i => 
                    {
                        i.Status = StatusProductEnum.Comprado;
                    });

                    finished.ProductsBought(list, session, method, lastNumbers, totalPrice);
                    break;
                case 2:
                    cartView.ListCart(session);
                    break;
                default:
                    break;
            }
        }
    }
}
