using SingleExperience.Entities.ClientEntities;
using SingleExperience.Entities.ClientsEntities;
using SingleExperience.Entities.DB;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

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

            if (client.Password == signIn.Password)
            {
                session = client.Cpf;
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
            var card = clientDB.GetCard(session);
            var hasCard = false;

            if (card != null)
            {
                hasCard = true;
            }

            return hasCard;
        }

        //Traz todos os cartões cadastrados do usuário
        public List<ShowCard> ShowCards(string session)
        {
            var clientDB = new ClientDB();
            var client = clientDB.GetClient(session);
            var card = clientDB.GetCard(session);
            var cards = new List<ShowCard>();

            if (card.ClientId == client.Cpf)
            {
                var showCards = new ShowCard();
                showCards.CardNumber = card.CardNumber.ToString();
                showCards.Name = card.Name;
                showCards.ShelfLife = card.ShelfLife;

                cards.Add(showCards);
            }
            return cards;
        }        
    }
}
