using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices.Models;
using SingleExperience.Services.ProductServices.Models.CartModels;
using System;
using System.Collections.Generic;
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
            var payment = (MethodPaymentEnum)method;
            var total = cart.TotalCart(session);
            var listConfirmation = new List<BuyProductModel>();
            var j = 41;

            Console.WriteLine("\nCarrinho > Informações pessoais > Método de pagamento > Confirma compra\n");

            Console.Clear();
            cart.PreviewBoughts(session, payment, lastNumbers, StatusProductEnum.Ativo)
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
                        Console.WriteLine(i.ProductId);
                        Console.WriteLine(i.Name);
                        Console.WriteLine(i.Amount);
                        Console.WriteLine(i.Price);


                        var bought = new BuyProductModel();

                        bought.ProductId = i.ProductId;
                        bought.Amount = i.Amount;
                        bought.Status = 0;

                        listConfirmation.Add(bought);
                    });
                });

            Console.WriteLine("Resumo do pedido");
            Console.WriteLine($"Itens: R$ {total.TotalPrice}");
            Console.WriteLine("Frete: R$ 0,00");
            Console.WriteLine($"Total do Pedido: R$ {total.TotalPrice}");

            Menu(listConfirmation, session, method, lastNumbers);
        }

        public void Menu(List<BuyProductModel> list, string session, Enum method, string lastNumbers)
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

                    finished.ProductsBought(list, session, method, lastNumbers);
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
