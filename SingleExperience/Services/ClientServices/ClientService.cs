using SingleExperience.Entities.DB;
using SingleExperience.Services.ClientServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.ClientServices
{
    class ClientService
    {

        private ClientDB clientDB;

        public ClientService()
        {
            clientDB = new ClientDB();
        }

        //Login
        public string SignIn(SignInModel signIn)
        {
            var client = clientDB.GetClient(signIn.Email);
            string session = "";

            if (client != null)
            {
                if (client.Password == signIn.Password)
                {
                    session = client.Cpf;
                }
            }

            return session;
        }

        //Sair
        public string SignOut()
        {
            return clientDB.ClientId();
        }

        //Puxa o nome do cliente
        public string ClientName(string session)
        {
            var client = clientDB.GetClient(session);

            return client.FullName;
        }        

        //Verifica se o cliente possui cartão de crédito
        public bool HasCard(string session)
        {
            var card = clientDB.ListCard(session);
            var hasCard = false;

            if (card.Count != 0)
            {
                hasCard = true;
            }

            return hasCard;
        }

        //Traz todos os cartões cadastrados do usuário
        public List<ShowCardModel> ShowCards(string session)
        {
            var card = clientDB.ListCard(session);
            var cards = new List<ShowCardModel>();

            cards = card
                .Select(i => new ShowCardModel
                {
                    CardNumber = i.CardNumber.ToString(),
                    Name = i.Name,
                    ShelfLife = i.ShelfLife
                })
                .ToList();
            return cards;
        }

        //Traz todos os endereços do usuário
        public List<ShowAddressModel> ShowAddress(string session)
        {
            var client = clientDB.GetClient(session);
            var listAddress = clientDB.ListAddress(session);
            var showAddress = new List<ShowAddressModel>();

            showAddress = listAddress
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

            return showAddress;
        }
    }
}
