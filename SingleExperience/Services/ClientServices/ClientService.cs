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
            var listClient = clientDB.ListClient();
            string session = "";

            foreach (var item in listClient)
            {
                if (item.Email == signIn.Email && item.Password == signIn.Password)
                {
                    session = item.Cpf;
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
            var listClient = clientDB.ListClient();
            string name = null;

            listClient.ForEach(p =>
            {
                if (p.Cpf == session)
                {
                    name = p.FullName;
                    var lines = new List<string>();
                }
            });

            return name;
        }        

        //Verifica se o cliente possui cartão de crédito
        public bool HasCard(string session)
        {
            var clientDB = new ClientDB();
            var listClient = clientDB.ListClient();
            var listCard = clientDB.ListCard();
            var hasCard = false;
            string aux = "";

            listClient.ForEach(p =>
            {
                if (p.Cpf == session)
                {
                    aux = p.Cpf;
                }
            });

            listCard.ForEach(p => 
            {
                if (p.ClientId == aux)
                {
                    hasCard = true;
                }
            });
            return hasCard;
        }

        //Traz todos os cartões cadastrados do usuário
        public List<ShowCard> ShowCards(string session)
        {
            var clientDB = new ClientDB();
            var listClient = clientDB.ListClient();
            var listCard = clientDB.ListCard();
            var cards = new List<ShowCard>();
            string aux = "";

            listClient.ForEach(p =>
            {
                if (p.Cpf == session)
                {
                    aux = p.Cpf;
                }
            });

            listCard.ForEach(p =>
            {
                if (p.ClientId == aux)
                {
                    var card = new ShowCard();
                    card.CardNumber = p.CardNumber.ToString();
                    card.Name = p.Name;
                    card.ShelfLife = p.DateTime;
                    cards.Add(card);
                }
            });
            return cards;
        }        
    }
}
