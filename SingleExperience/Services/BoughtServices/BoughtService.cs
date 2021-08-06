using SingleExperience.Entities.DB;
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

        //public List<BoughtModel> ClientBought(string session, string numberBoughtPayment)
        //{
        //    var boughtModel = new List<BoughtModel>();
        //    var client = clientDB.GetClient(session);
        //    var address = clientDB.ListAddress(client.AddressId);
        //    var card = clientDB.ListCard(session);
        //    var itens = cartDB.ListItens(session);
        //    var listProducts = new List<ProductBought>();

        //    var listBought = boughtDB.List(session);

        //    boughtModel.ForEach(j =>
        //    {
        //        j.ClientName = client.FullName;

        //        address.ForEach(i =>
        //        {
        //            j.Cep = i.Cep;
        //            j.Street = i.Street;
        //            j.Number = i.Number;
        //            j.City = i.City;
        //            j.State = i.State;
        //        });

        //        listBought.ForEach(i =>
        //        {
        //            if (i.PaymentId == Convert.ToInt32(j.paymentMethod))
        //            {
        //                card
        //                .Where(i => i.CardNumber.ToString().Contains(numberBoughtPayment))
        //                .ToList()
        //                .ForEach(i =>
        //                {
        //                    j.NumberCard = i.CardNumber.ToString();
        //                });
        //            }


        //        });

        //    });



        //}
    }
}
