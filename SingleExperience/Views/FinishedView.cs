using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Models.CartModels;
using SingleExperience.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class FinishedView
    {
        public void ProductsBought(List<BuyProductsModel> buyProducts, string session, Enum method, string lastNumbers)
        {
            var home = new HomeView();
            var cart = new CartService();
            var bought = new BuyProductsModel();
            var total = cart.TotalCart(session);
            var payment = (MethodPaymentEnum)method;
            var j = 41;

            var buy = cart.Buy(buyProducts, session);

            if (buy)
            {
                Console.Clear();

                Console.WriteLine("\nCompra realizada com sucesso!!\n");

                cart.PreviewBoughts(session, payment, lastNumbers)
                .ForEach(p =>
                {
                    Console.WriteLine("Endereço de entrega");
                    Console.WriteLine(p.FullName);
                    Console.WriteLine($"{p.Street}, {p.Number}");
                    Console.WriteLine($"{p.City} - {p.State}");
                    Console.WriteLine(p.Cep);
                    Console.WriteLine($"Telefone: {p.Phone}");
                    Console.WriteLine($"\n+{new string('-', j)}+\n");
                    Console.WriteLine("Forma de pagamento");
                    if (payment == MethodPaymentEnum.CreditCard)
                        Console.WriteLine($"(Crédito) com final {p.NumberCard.Substring(12, p.NumberCard.Length - 12)}");
                    else if (payment == MethodPaymentEnum.Bullet)
                        Console.WriteLine($"(Boleto)");
                    else
                        Console.WriteLine($"(PIX)");

                    Console.WriteLine($"\n+{new string('-', j)}+\n");
                    var item = p.Itens
                    .GroupBy(j => j.Name)
                    .Select(i => new ProductsCartModel()
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
                        Console.WriteLine(i.ProductId);
                        Console.WriteLine(i.Name);
                        Console.WriteLine(i.Amount);
                        Console.WriteLine(i.Price);


                        var bought = new BuyProductsModel();
                    });
                });
                Console.WriteLine($"Total do Pedido: R$ {total.TotalPrice}");
                Console.WriteLine("\nTecle enter para continuar");
                Console.ReadKey();
                home.ListProducts(total.TotalAmount, session);
            }
        }
    }
}
