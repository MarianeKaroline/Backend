using SingleExperience.Entities.DB;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SingleExperience.Views
{
    class ClientFinishedView
    {
        private BoughtDB boughtDB = new BoughtDB();
        private CartService cart = new CartService();

        public void ProductsBought(SessionModel parameters, AddBoughtModel addBought)
        {
            ClientHomeView home = new ClientHomeView();

            var ids = new List<int>();

            addBought.BuyProducts.ForEach(i =>
            {
                ids.Add(i.ProductId);
            });

            var boughtModel = new BuyModel();

            boughtModel.Session = parameters.Session;
            boughtModel.Method = addBought.Payment;
            boughtModel.Confirmation = addBought.CodeConfirmation;
            boughtModel.Status = StatusProductEnum.Comprado;
            boughtModel.Ids = ids;

            var buy = cart.Buy(addBought.BuyProducts, parameters.Session);

            boughtDB.AddBought(parameters, addBought);

            var j = 51;


            if (buy)
            {
                var data = cart.PreviewBoughts(parameters, boughtModel, addBought.AddressId);

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
                });

                var total = cart.TotalCart(parameters);

                Console.WriteLine($"Total do Pedido: R$ {addBought.TotalPrice}");
                Console.WriteLine("\nTecle enter para continuar");
                Console.ReadKey();
                parameters.CountProduct = 0;
                home.ListProducts(parameters);
            }
        }
    }
}
