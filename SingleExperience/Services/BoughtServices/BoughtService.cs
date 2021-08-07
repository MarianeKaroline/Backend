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

        public List<BoughtModel> ClientBought(string session)
        {
            var boughtModel = new BoughtModel();
            var client = clientDB.GetClient(session);
            var address = clientDB.ListAddress(client.AddressId);
            var card = clientDB.ListCard(session);
            var itens = cartDB.ListItens(session);
            var listProducts = new List<BoughtModel>();
            var product = new ProductBought();
            boughtModel.Itens = new List<ProductBought>();

            var listBought = boughtDB.List(session);
            boughtModel.ClientName = client.FullName;

            address.ForEach(i =>
            {
                boughtModel.Cep = i.Cep;
                boughtModel.Street = i.Street;
                boughtModel.Number = i.Number;
                boughtModel.City = i.City;
                boughtModel.State = i.State;
            });

            
            listBought.ForEach(i =>
            {
                boughtModel.BoughtId = i.BoughtId;
                boughtModel.paymentMethod = (PaymentMethodEnum)i.PaymentId;
                if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.CreditCard))
                {
                    card
                    .Where(j => j.CardNumber.ToString().Contains(i.NumberPayment))
                    .ToList()
                    .ForEach(k =>
                    {
                        boughtModel.NumberCard = k.CardNumber.ToString();
                    });
                }
                else if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.BankSlip))
                {
                    boughtModel.Code = i.NumberPayment;
                }
                else
                {
                    boughtModel.Pix = i.NumberPayment;
                }

                product.ProductId = i.ProductId;
                product.ProductName = i.Name;
                product.CategoryId = i.CategoryId;
                product.Amount = i.Amount;
                product.StatusId = i.StatusId;
                product.Price = i.Price;
                boughtModel.TotalPrice = i.TotalPrice;


                boughtModel.DateBought = i.DateBought;
            });

            boughtModel.Itens.Add(product);

            listProducts.Add(boughtModel);

            return listProducts;
        }

        //Verifica se o número que o cliente digitou está correto
        public bool HasBought(string session, int boughtId)
        {
            var aux = boughtDB.List(session).Where(i => i.BoughtId == boughtId).ToList();

            return aux.Count > 0;
        }
    }
}
