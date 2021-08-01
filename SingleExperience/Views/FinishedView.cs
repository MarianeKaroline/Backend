using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Models.CartModels;
using SingleExperience.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class FinishedView
    {
        public void ProductsBought(List<BuyProductModel> buyProducts, string session, Enum method, string lastNumbers, double totalPrice)
        {
            var home = new HomeView();
            var cart = new CartService();
            var bought = new BuyProductModel();
            var payment = (PaymentMethodEnum)method;
            var j = 41;

            var buy = cart.Buy(buyProducts, session);

            if (buy)
            {
                Console.Clear();

                Console.WriteLine("\nCompra realizada com sucesso!!\n");

                cart.PreviewBoughts(session, payment, lastNumbers, StatusProductEnum.Comprado)
                .ForEach(p =>
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|Endereço de entrega{new string(' ', j - "Endereço de entrega".Length)}|");
                    Console.WriteLine($"|{p.FullName}{new string(' ', p.FullName.Length)}|");
                    Console.WriteLine($"|{p.Street}, {p.Number}{new string(' ', j - p.Street.Length - 2 - p.Number.Length)}|");
                    Console.WriteLine($"|{p.City} - {p.State}{new string(' ', j - p.City.Length - 3 - p.State.Length)}|");
                    Console.WriteLine($"|{p.Cep}{new string(' ', j - p.Cep.Length)}|");
                    Console.WriteLine($"|Telefone: {p.Phone}{new string(' ', j - $"Telefone: {p.Phone}".Length)}|");
                    Console.WriteLine($"\n+{new string('-', j)}+\n");
                    Console.WriteLine($"|Forma de pagamento{new string(' ', j - $"Forma de pagamento".Length)}|");
                    if (payment == PaymentMethodEnum.CreditCard)
                        Console.WriteLine($"(Crédito) com final {p.NumberCard.Substring(12, p.NumberCard.Length - 12)}{new string(' ', j - $"(Crédito) com final {p.NumberCard.Substring(12, p.NumberCard.Length - 12)}".Length)}|");
                    else if (payment == PaymentMethodEnum.BankSlip)
                        Console.WriteLine($"(Boleto){new string(' ', j - "(Boleto)".Length)}|");
                    else
                        Console.WriteLine($"(PIX){new string(' ', j - "(PIX)".Length)}|");

                    Console.WriteLine($"\n+{new string('-', j)}+\n");
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
                        Console.WriteLine($"|Qtde: {i.Amount}{new string(' ', j - 6 - i.Amount.ToString().Length)}|");
                        Console.WriteLine($"|{i.Price.ToString("F2", CultureInfo.InvariantCulture)}{new string(' ', j - i.Price.ToString().Length)}|");

                    });
                    var total = cart.TotalCart(session);
                    Console.WriteLine($"Total do Pedido: R$ {totalPrice}");
                    Console.WriteLine("\nTecle enter para continuar");
                    Console.ReadKey();

                    home.ListProducts(total.TotalAmount, session);
                });
            }
        }
    }
}
