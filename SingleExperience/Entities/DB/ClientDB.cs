using SingleExperience.Entities.ClientEntities;
using SingleExperience.Entities.ClientsEntities;
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
        private string header = null;

        public ClientDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Client.csv";
            pathAddress = CurrentDirectory + @"..\..\..\..\\Database\Address.csv";
            pathCard = CurrentDirectory + @"..\..\..\..\\Database\Card.csv";
            header = "";
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
        public List<ClientEntitie> ListClient()
        {
            var client = new List<ClientEntitie>();
            try
            {
                string[] clientList = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    clientList
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var cli = new ClientEntitie();
                            cli.Cpf = fields[0];
                            cli.FullName = fields[1];
                            cli.Phone = fields[2];
                            cli.Email = fields[3];
                            cli.BirthDate = DateTime.Parse(fields[4]);
                            cli.Password = fields[5];
                            cli.AddressId = int.Parse(fields[6]);

                            client.Add(cli);
                        });
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
        public List<AddressEntitie> ListAddress()
        {
            var address = new List<AddressEntitie>();
            try
            {
                string[] AddressList = File.ReadAllLines(pathAddress, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(pathAddress))
                {
                    AddressList
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var addr = new AddressEntitie();
                            addr.AddressId = int.Parse(fields[0]);
                            addr.Cep = fields[1];
                            addr.Street = fields[2];
                            addr.Number = fields[3];
                            addr.City = fields[4];
                            addr.State = fields[5];

                            address.Add(addr);
                        });
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
        public List<CardEntitie> ListCard()
        {
            var card = new List<CardEntitie>();
            try
            {
                string[] cardList = File.ReadAllLines(pathCard, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(pathAddress))
                {
                    cardList
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var cardClient = new CardEntitie();
                            cardClient.CardNumber = long.Parse(fields[0]);
                            cardClient.Name = fields[1];
                            cardClient.DateTime = DateTime.Parse(fields[2]);
                            cardClient.CVV = int.Parse(fields[3]);
                            cardClient.ClientId = fields[4];

                            card.Add(cardClient);
                        });
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
        public string SignUp(SignUpModel client)
        {
            var clientDB = new ClientDB();
            var listClient = clientDB.ListClient();
            var address = File.ReadAllLines(pathAddress, Encoding.UTF8);
            var count = 0;
            string msg = null;
            try
            {
                var lines = new List<string>();
                var linesAddress = new List<string>();

                foreach (var item in listClient)
                {
                    if (item.Email == client.Email)
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    var aux = new string[]
                    {
                        client.Cpf.ToString(),
                        client.FullName.ToString(),
                        client.Phone.ToString(), client.Email.ToString(),
                        client.BirthDate.ToString(),
                        client.Password.ToString(),
                        address.Length.ToString(),
                        client.SessionId.ToString()
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
                    msg = "\nUsuário cadastrado com sucesso";
                }
                else
                {
                    msg = "\nJá existe uma conta com esse e-mail";
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

            return msg;
        }

        //CreditCard
        public void AddCard(string session, CardModel card)
        {
            var clientDB = new ClientDB();
            var listClient = clientDB.ListClient();
            var listCard = clientDB.ListCard();
            var count = 0;
            var lines = new List<string>();

            try
            {
                listClient.ForEach(p =>
                {
                    if (p.SessionId != 0 && p.Cpf == session)
                    {
                        foreach (var item in listCard)
                        {
                            if (item.CardNumber == card.CardNumber)
                            {
                                count++; //Acrescenta um se o cartão já estiver cadastrado
                            }
                        }
                        if (count == 0)
                        {
                            var aux = new string[] { card.CardNumber.ToString(), card.Name, card.ShelfLife.ToString(), card.CVV.ToString(), p.Cpf.ToString() };
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
                });
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
        }
    }
}
