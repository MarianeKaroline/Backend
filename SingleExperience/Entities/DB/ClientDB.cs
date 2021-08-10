using SingleExperience.Entities.ClientEntities;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class ClientDB : EmployeeDB
    {
        private string pathAddress = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Address.csv";
        private string pathCard = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Card.csv";

        //Address
        public List<AddressEntitie> ListAddress(string userId)
        {
            string[] addressList = File.ReadAllLines(pathAddress, Encoding.UTF8);

            return addressList
                .Skip(1)
                .Select(i => new AddressEntitie
                {
                    AddressId = int.Parse(i.Split(',')[0]),
                    Cep = i.Split(',')[1],
                    Street = i.Split(',')[2],
                    Number = i.Split(',')[3],
                    City = i.Split(',')[4],
                    State = i.Split(',')[5],
                    Cpf = i.Split(',')[6]
                })
                .Where(i => i.Cpf == userId)
                .ToList();
        }

        //Card
        public List<CardEntitie> ListCard(string userId)
        {
            string[] cardList = File.ReadAllLines(pathCard, Encoding.UTF8);

            return cardList
                    .Skip(1)
                    .Where(i => i.Split(',')[5] == userId)
                    .Select(i => new CardEntitie
                    {
                        CardId = int.Parse(i.Split(',')[0]),
                        CardNumber = long.Parse(i.Split(',')[1]),
                        Name = i.Split(',')[2],
                        ShelfLife = DateTime.Parse(i.Split(',')[3]),
                        CVV = int.Parse(i.Split(',')[4]),
                        Cpf = i.Split(',')[5],
                    })
                    .ToList();
        }

        /* Cadastro */
        //Client
        public bool SignUpClient(SignUpModel client)
        {
            var existClient = GetEnjoyer(client.Cpf);

            if (existClient == null)
            {
                SignUp(client);
            }

            return existClient == null;
        }

        //Address
        public int AddAddress(string session, AddressModel addressModel)
        {
            var linesAddress = new List<string>();
            try
            {
                var auxAddress = new string[]
                {
                    (ListAddress(session).Count() + 1).ToString(),
                    addressModel.Cep,
                    addressModel.Street,
                    addressModel.Number,
                    addressModel.City,
                    addressModel.State,
                    addressModel.ClientId
                };
                linesAddress.Add(String.Join(",", auxAddress));


                using (StreamWriter sw = File.AppendText(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Address.csv"))
                {
                    linesAddress.ForEach(p =>
                    {
                        sw.WriteLine(p);
                    });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

            return (ListAddress(session).Count() + 1);
        }

        //CreditCard
        public void AddCard(string session, CardModel card)
        {
            var client = GetEnjoyer(card.Cpf);
            var existCard = ListCard(session);
            var lines = new List<string>();
            var exist = 0;

            try
            {
                existCard
                    .ForEach(i =>
                    {
                        if (i.CardNumber == card.CardNumber)
                        {
                            exist++;
                        }
                    });
                if (exist == 0)
                {
                    var aux = new string[]
                    {
                        (ListCard(session).Count() + 1).ToString(),
                        card.CardNumber.ToString(),
                        card.Name,
                        card.ShelfLife.ToString(),
                        card.CVV.ToString(),
                        session,
                    };
                    lines.Add(String.Join(",", aux));

                    using (StreamWriter sw = File.AppendText(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Card.csv"))
                    {
                        lines.ForEach(p =>
                        {
                            sw.WriteLine(p);
                        });
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
        }
    }
}
