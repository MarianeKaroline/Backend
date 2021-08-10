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
        private BoughtDB boughtDB = new BoughtDB();
        private CartDB cartDB = new CartDB();
        private ClientDB clientDB = new ClientDB();

        //Listar as compras do cliente
        public List<BoughtModel> ClientBought(string session)
        {
            var client = clientDB.GetEnjoyer(session);
            var address = clientDB.ListAddress(session);
            var card = clientDB.ListCard(session);
            var cart = cartDB.GetCart(session);
            var productDB = new ProductDB();
            var itens = cartDB.ListItens(cart.CartId);
            var listProducts = new List<BoughtModel>();

            var listBought = boughtDB.List(session);

            listBought.ForEach(i =>
            {
                var boughtModel = new BoughtModel();
                boughtModel.Itens = new List<ProductBoughtModel>();

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
                boughtModel.StatusId = i.StatusId;


                boughtDB.ListProductBought(i.BoughtId)
                .ToList()
                .ForEach(j =>
                {
                    var product = new ProductBoughtModel();

                    product.ProductId = j.ProductId;
                    product.ProductName = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Name;
                    product.Amount = j.Amount;
                    product.Price = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Price;

                    boughtModel.Itens.Add(product);
                });

                listProducts.Add(boughtModel);
            });


            return listProducts;
        }

        //Verifica se o número que o cliente digitou está correto
        public bool HasBought(string session, int boughtId)
        {
            return boughtDB.List(session).FirstOrDefault(i => i.BoughtId == boughtId) != null;
        }

        //Verifica se cliente já cadastrou algum endereço
        public bool HasAddress(string session)
        {
            return clientDB.ListAddress(session).FirstOrDefault() != null;
        }

        //Verifica se o funcionário digitou o código correto da compra
        public bool HasBoughts(int boughtId)
        {
            return boughtDB.ListAll().FirstOrDefault(i => i.BoughtId == boughtId) != null;
        }
    }
}
