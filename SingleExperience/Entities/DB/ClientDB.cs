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
    class ClientDB
    {
        private string CurrentDirectory = null;
        private string path = null;
        private string pathAddress = null;
        private string pathCard = null;

        public ClientDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Client.csv";
            pathAddress = CurrentDirectory + @"..\..\..\..\\Database\Address.csv";
            pathCard = CurrentDirectory + @"..\..\..\..\\Database\Card.csv";
        }


        //Pega o endereço do Computador
        public string ClientId()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string session = "";

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    session = ip.ToString().Replace(".", "");
                }
            }
            return session;
        }

        /* Lê Arquivo CSV */
        //Client
        public ClientEntitie GetClient(string authentication)
        {
            var client = new ClientEntitie();
            try
            {
                string[] clientList = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    client = clientList
                        .Skip(1)
                        .Select(i => new ClientEntitie
                        {
                            Cpf = i.Split(',')[0],
                            FullName = i.Split(',')[1],
                            Phone = i.Split(',')[2],
                            Email = i.Split(',')[3],
                            BirthDate = DateTime.Parse(i.Split(',')[4]),
                            Password = i.Split(',')[5],
                            AddressId = int.Parse(i.Split(',')[6])
                        })
                        .FirstOrDefault(i => i.Cpf == authentication || i.Email == authentication);                    
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return client;
        }

        //Address
        public List<AddressEntitie> ListAddress(int addressId)
        {
            var address = new List<AddressEntitie>();
            try
            {
                string[] addressList = File.ReadAllLines(pathAddress, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(pathAddress))
                {
                    address = addressList
                        .Skip(1)
                        .Select(i => new AddressEntitie
                        {
                            AddressId = int.Parse(i.Split(',')[0]),
                            Cep = i.Split(',')[1],
                            Street = i.Split(',')[2],
                            Number = i.Split(',')[3],
                            City = i.Split(',')[4],
                            State = i.Split(',')[5],
                        })
                        .Where(i => i.AddressId == addressId)
                        .ToList();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return address;
        }

        //Card
        public List<CardEntitie> ListCard(string userId)
        {
            var card = new List<CardEntitie>();
            try
            {
                string[] cardList = File.ReadAllLines(pathCard, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(pathAddress))
                {
                    card = cardList
                        .Skip(1)
                        .Where(i => i.Split(',')[4] == userId)
                        .Select(i => new CardEntitie
                        {
                            CardNumber = long.Parse(i.Split(',')[0]),
                            Name = i.Split(',')[1],
                            ShelfLife = DateTime.Parse(i.Split(',')[2]),
                            CVV = int.Parse(i.Split(',')[3]),
                            Cpf = i.Split(',')[4],
                        })
                        .ToList();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return card;
        }

        /* Cadastro */
        //Client
        public bool SignUp(SignUpModel client)
        {
            var existClient = GetClient(client.Cpf);
            var address = File.ReadAllLines(pathAddress, Encoding.UTF8);
            var signUp = false;

            try
            {
                var lines = new List<string>();
                var linesAddress = new List<string>();

                if (existClient == null)
                {
                    var aux = new string[]
                    {
                        client.Cpf.ToString(),
                        client.FullName.ToString(),
                        client.Phone.ToString(), 
                        client.Email.ToString(),
                        client.BirthDate.ToString(),
                        client.Password.ToString(),
                        address.Length.ToString()
                    };

                    var auxAddress = new string[]
                    {
                        address.Length.ToString(),
                        client.Cep.ToString(),
                        client.Street.ToString(),
                        client.Number.ToString(),
                        client.City.ToString(),
                        client.State.ToString()
                    };

                    lines.Add(String.Join(",", aux));

                    linesAddress.Add(String.Join(",", auxAddress));

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        lines.ForEach(p =>
                        {
                            sw.WriteLine(p);
                        });
                    }

                    using (StreamWriter sw = File.AppendText(pathAddress))
                    {
                        linesAddress.ForEach(p =>
                        {
                            sw.WriteLine(p);
                        });
                    }

                    signUp = true;
                }
                else
                {
                    signUp = false;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

            return signUp;
        }

        //CreditCard
        public void AddCard(string session, CardModel card)
        {
            var client = GetClient(card.Cpf);
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
                        card.CardNumber.ToString(),
                        card.Name,
                        card.ShelfLife.ToString(),
                        card.CVV.ToString(),
                        session,
                    };
                    lines.Add(String.Join(",", aux));

                    using (StreamWriter sw = File.AppendText(pathCard))
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
