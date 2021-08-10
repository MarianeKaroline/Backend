using SingleExperience.Entities.DB;
using SingleExperience.Services.ClientServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.ClientServices
{
    class ClientService : EnjoyerDB
    {

        private ClientDB clientDB = new ClientDB();               

        //Puxa o nome do cliente
        public string ClientName(string session)
        {
            return clientDB.GetEnjoyer(session).FullName;
        }        

        //Verifica se o cliente possui cartão de crédito
        public bool HasCard(string session)
        {
            return clientDB.ListCard(session).Count != 0;
        }

        //Traz todos os cartões cadastrados do usuário
        public List<ShowCardModel> ShowCards(string session)
        {
            return clientDB.ListCard(session)
                .Select(i => new ShowCardModel
                {
                    CardNumber = i.CardNumber.ToString(),
                    Name = i.Name,
                    ShelfLife = i.ShelfLife
                })
                .ToList();
        }

        //Traz todos os endereços do usuário
        public List<ShowAddressModel> ShowAddress(string session)
        {
            var client = GetEnjoyer(session);
            var listAddress = clientDB.ListAddress(session);

            return listAddress
                .Select(i => new ShowAddressModel
                {
                    ClientName = client.FullName,
                    ClientPhone = client.Phone,
                    AddressId = i.AddressId,
                    Cep = i.Cep,
                    Street = i.Street,
                    Number = i.Number,
                    City = i.City,
                    State = i.State
                })
                .ToList();
        }
    }
}
