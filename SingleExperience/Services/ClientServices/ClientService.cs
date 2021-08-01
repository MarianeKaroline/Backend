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

namespace SingleExperience.Services.ClientServices
{
    class ClientService
    {
        private string CurrentDirectory = null;
        private string path = null;
        private string pathAddress = null;
        private string pathCard = null;
        public ClientService()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Client.csv";
            pathAddress = CurrentDirectory + @"..\..\..\..\\Database\Address.csv";
            pathCard = CurrentDirectory + @"..\..\..\..\\Database\Card.csv";
        }

        //Pega o ip do computador, para verificar o usuário
        public string ClientId()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string session = "";

            //Pega o endereço do Computador
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    session = ip.ToString().Replace(".", "");
                }
            }
            return session;
        }

        //Lê o arquivo csv client
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

        //Lê o arquivo csv address
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

        //Lê o arquivo csv card
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

        //Cadastro Cliente
        public string SignUp(SignUpModel client)
        {
            var listClient = ListClient();
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

        //Login
        public string SignIn(SignInModel signIn)
        {
            var listClient = ListClient();
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
        public long SignOut()
        {
            return 0;
        }

        //Puxa o nome do cliente
        public string ClientName(string session)
        {
            var client = ListClient();
            string name = null;

            client.ForEach(p =>
            {
                if (p.Cpf == session)
                {
                    name = p.FullName;
                    var lines = new List<string>();
                }
            });

            return name;
        }

        //cadastra Cartão de Crédito
        public void AddCard(string session, CardModel card)
        {
            var client = ListClient();
            var listCard = ListCard();
            var count = 0;
            var lines = new List<string>();

            try
            {
                client.ForEach(p =>
                {
                    if (p.SessionId != 0 && p.Cpf == session)
                    {
                        foreach (var item in listCard)
                        {
                            if (item.CardNumber == card.CardNumber)
                            {
                                count++;
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

        //Verifica se está registrado
        public bool Registered(string session)
        {
            var client = ListClient();
            var registered = false;
            foreach (var item in client)
            {
                if (item.Cpf != "")
                {
                    if (item.Cpf == session && item.Cpf != "" && item.Cpf.ToString().Length == 11)
                    {
                        registered = true;
                    }
                }
            }
            return registered;
        }

        //Verifica se já possui cartão
        public bool HasCard(string session)
        {
            var hasCard = false;
            var listClient = ListClient();
            var listCard = ListCard();
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

        //Altera DateTime do ListCard
        public List<ShowCard> ListCardClient(string session)
        {
            var listClient = ListClient();
            var listCard = ListCard();
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
