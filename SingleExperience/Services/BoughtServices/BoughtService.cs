using SingleExperience.Entities.ClientEntities;
using SingleExperience.Entities.DB;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.BoughtServices
{
    class BoughtService
    {
        private BoughtDB boughtDB;
        private CartDB cartDB;
        private ClientDB clientDB;
        public BoughtService()
        {
            boughtDB = new BoughtDB();
            cartDB = new CartDB();
            clientDB = new ClientDB();
        }

        //Listar as compras do cliente
        public List<BoughtModel> ClientBought(string session)
        {
            var client = clientDB.GetClient(session);
            var address = clientDB.ListAddress(session);
            var card = clientDB.ListCard(session);
            var cart = cartDB.GetCart(session);
            var itens = cartDB.ListItens(cart.CartId);
            var listProducts = new List<BoughtModel>();

            var listBought = boughtDB.List(session);

            listBought.ForEach(i =>
            {
                var boughtModel = new BoughtModel();
                boughtModel.Itens = new List<ProductBought>();

                boughtModel.ClientName = client.FullName;
                var aux = address
                .FirstOrDefault(j => j.AddressId == i.AddressId);

                boughtModel.Cep = aux.Cep;
                boughtModel.Street = aux.Street;
                boughtModel.Number = aux.Number;
                boughtModel.City = aux.City;
                boughtModel.State = aux.State;

                boughtModel.BoughtId = i.BoughtId;
                boughtModel.paymentMethod = (PaymentMethodEnum)i.PaymentId;

                if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.CreditCard))
                {
                    card
                    .Where(j => j.CardNumber.ToString().Contains(i.CodeBought))
                    .ToList()
                    .ForEach(k =>
                    {
                        boughtModel.NumberCard = k.CardNumber.ToString();
                    });
                }
                else if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.BankSlip))
                {
                    boughtModel.Code = i.CodeBought;
                }
                else
                {
                    boughtModel.Pix = i.CodeBought;
                }
                boughtModel.TotalPrice = i.TotalPrice;
                boughtModel.DateBought = i.DateBought;

                boughtDB.ListProductBought(i.BoughtId)
                .ToList()
                .ForEach(j =>
                {
                    var product = new ProductBought();

                    product.ProductId = j.ProductId;
                    product.ProductName = j.Name;
                    product.CategoryId = j.CategoryId;
                    product.Amount = j.Amount;
                    product.StatusId = j.StatusId;
                    product.Price = j.Price;

                    boughtModel.Itens.Add(product);
                });

                listProducts.Add(boughtModel);
            });


            return listProducts;
        }

        //Verifica se o número que o cliente digitou está correto
        public bool HasBought(string session, int boughtId)
        {
            var aux = boughtDB.List(session).FirstOrDefault(i => i.BoughtId == boughtId);

            return aux != null;
        }

        //Verifica se cliente já cadastrou algum endereço
        public bool HasAddress(string session)
        {
            var aux = clientDB.ListAddress(session).FirstOrDefault();

            return aux != null;
        }
    }
}
