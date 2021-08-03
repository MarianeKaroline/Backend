using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Models.CartModels;
using SingleExperience.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            var data = cart.PreviewBoughts(session, payment, lastNumbers, StatusProductEnum.Comprado);
            var j = 41;

            var buy = cart.Buy(buyProducts, session);

            if (buy)
            {
                Console.Clear();

                Console.WriteLine("\nCompra realizada com sucesso!!\n");

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

                if (payment == PaymentMethodEnum.CreditCard)
                    Console.WriteLine($"|(Crédito) com final {data.NumberCard.Substring(12)}{new string(' ', j - $"(Crédito) com final {data.NumberCard.Substring(12)}".Length)}|");
                else if (payment == PaymentMethodEnum.BankSlip)
                    Console.WriteLine($"|(Boleto) {data.Code}{new string(' ', j - $"(Boleto) {data.Code}".Length)}|");
                else
                    Console.WriteLine($"|(PIX) {data.Pix}{new string(' ', j - $"(PIX) {data.Pix}".Length)}|");

                Console.WriteLine($"|{new string(' ', j)}|");
                Console.WriteLine($"+{new string('-', j)}+");

                var item = data.Itens
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
                    Console.WriteLine($"|{i.Price.ToString("F2", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - i.Price.ToString().Length)}|");
                    Console.WriteLine($"|{new string(' ', j)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                });

                var total = cart.TotalCart(session);

                Console.WriteLine($"Total do Pedido: R$ {totalPrice}");
                Console.WriteLine("\nTecle enter para continuar");
                Console.ReadKey();

                home.ListProducts(total.TotalAmount, session);
            }
        }
    }
}
