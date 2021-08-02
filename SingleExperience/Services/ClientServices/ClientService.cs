using SingleExperience.Entities.DB;
using SingleExperience.Services.ClientServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace SingleExperience.Services.ClientServices
{
    class ClientService
    {   
        //Login
        public string SignIn(SignInModel signIn)
        {
            var clientDB = new ClientDB();
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
            ClientDB client = new ClientDB();
            return client.ClientId();
        }

        //Puxa o nome do cliente
        public string ClientName(string session)
        {
            var clientDB = new ClientDB();
            var client = clientDB.GetClient(session);

            return client.FullName;
        }        

        //Verifica se o cliente possui cartão de crédito
        public bool HasCard(string session)
        {
            var clientDB = new ClientDB();
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
            var clientDB = new ClientDB();
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
