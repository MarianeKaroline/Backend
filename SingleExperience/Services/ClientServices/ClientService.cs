using SingleExperience.Entities.ProductEntities.DB;
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
                    session = client.UserId;
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
        public List<ShowCard> ShowCards(string session)
        {
            var card = clientDB.ListCard(session);
            var cards = new List<ShowCard>();

            cards = card
                .Select(i => new ShowCard
                {
                    CardNumber = i.CardNumber.ToString(),
                    Name = i.Name,
                    ShelfLife = i.ShelfLife
                })
                .ToList();
            return cards;
        }        
    }
}
