using SingleExperience.Entities.CartEntities;
using SingleExperience.Entities.ClientEntities;
using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class BoughtDB
    {
        private string CurrentDirectory;
        private string path;
        private string pathProducts;
        private CartDB cartDB = new CartDB();
        private ClientDB clientDB = new ClientDB();
        private string header;

        public BoughtDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Bought.csv";
            pathProducts = CurrentDirectory + @"..\..\..\..\\Database\ProductBought.csv";
            header = ReadBought()[0];
        }

        public string[] ReadBought()
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        public string[] ReadProductBought()
        {
            return File.ReadAllLines(pathProducts, Encoding.UTF8);
        }

        //Lista todas as compras para o employee
        public List<BoughtEntitie> ListAll()
        {
            return ReadBought()
                .Skip(1)
                .Select(p => new BoughtEntitie
                {
                    BoughtId = int.Parse(p.Split(',')[0]),
                    TotalPrice = double.Parse(p.Split(',')[1]),
                    AddressId = int.Parse(p.Split(',')[2]),
                    PaymentId = int.Parse(p.Split(',')[3]),
                    CodeBought = p.Split(',')[4],
                    Cpf = p.Split(',')[5],
                    StatusId = int.Parse(p.Split(',')[6]),
                    DateBought = DateTime.Parse(p.Split(',')[7])
                })
                .ToList();
        }

        //Lista apenas as compras do usuário
        public List<BoughtEntitie> List(string userId)
        {
            //Irá procurar a compra pelo cpf do cliente
            return ReadBought()
                .Skip(1)
                .Select(p => new BoughtEntitie
                {
                    BoughtId = int.Parse(p.Split(',')[0]),
                    TotalPrice = double.Parse(p.Split(',')[1]),
                    AddressId = int.Parse(p.Split(',')[2]),
                    PaymentId = int.Parse(p.Split(',')[3]),
                    CodeBought = p.Split(',')[4],
                    Cpf = p.Split(',')[5],
                    StatusId = int.Parse(p.Split(',')[6]),
                    DateBought = DateTime.Parse(p.Split(',')[7])
                })
                .Where(p => p.Cpf == userId)
                .ToList();
        }

        //Lista os produtos comprados
        public List<ProductBoughtEntitie> ListProductBought(int boughtId)
        {
            //Irá procurar o os produtos da compra pela compra id
            return ReadProductBought()
                .Skip(1)
                .Select(p => new ProductBoughtEntitie
                {
                    ProductBoughtId = int.Parse(p.Split(',')[0]),
                    ProductId = int.Parse(p.Split(',')[1]),
                    Amount = int.Parse(p.Split(',')[2]),
                    BoughtId = int.Parse(p.Split(',')[3]),
                })
                .Where(p => p.BoughtId == boughtId)
                .ToList();
        }

        //Adiciona os produtos nas tabelas de compras
        public void AddBought(SessionModel parameters, AddBoughtModel addBought)
        {
            var listBought = List(parameters.Session);
            var getCart = cartDB.GetCart(parameters.Session);
            var listItens = new List<ItemEntitie>();
            var linesItens = new List<string>();
            var linesBought = new List<string>();
            var data = DateTime.Now.ToString("G");
            string codeBought = ""; //Esse code Bought é o número do cartão, ou o gid gerado para boleto e pix
            int statusBought = 0;

            //Verifica se o pagamento foi feito com boleto, para transformar o status do produto em Pagamento Pendente
            if (addBought.Payment == PaymentMethodEnum.BankSlip)
            {
                statusBought = Convert.ToInt32(StatusBoughtEnum.PagamentoPendente);
            }
            else
            {
                statusBought = Convert.ToInt32(StatusBoughtEnum.ConfirmacaoPendente);
            }


            if (addBought.Payment == PaymentMethodEnum.CreditCard)
            {
                codeBought = clientDB.ListCard(parameters.Session)
                    .Where(p => p.CardNumber
                        .ToString()
                        .Contains(addBought.CodeConfirmation))
                    .FirstOrDefault()
                    .CardNumber
                    .ToString();
            }
            else
            {
                codeBought = addBought.CodeConfirmation;
            }

            try
            {
                //Adiciona compra no csv
                var auxBought = new string[]
                {
                    (listBought.Count() + 1).ToString(),
                    addBought.TotalPrice.ToString(),
                    addBought.AddressId.ToString(),
                    Convert.ToInt32(addBought.Payment).ToString(),
                    codeBought,
                    parameters.Session,
                    statusBought.ToString(),
                    data
                };

                linesBought.Add(String.Join(",", auxBought));

                using (StreamWriter writer = File.AppendText(path))
                {
                    linesBought.ForEach(j =>
                    {
                        writer.WriteLine(j);
                    });
                }


                //Pega os dados do último produto comprado
                addBought.BuyProducts.ForEach(j =>
                {
                    listItens.Add(cartDB.ListItens(getCart.CartId)
                        .Where(i =>
                            i.StatusId == Convert.ToInt32(StatusProductEnum.Comprado) &&
                            i.ProductId == j.ProductId)
                        .FirstOrDefault());
                });

                //Adiciona no csv o último produto comprado
                listItens.ForEach(i =>
                {
                    var aux = new string[]
                    {
                        ReadProductBought().Length.ToString(),
                        i.ProductId.ToString(),
                        i.Amount.ToString(),
                        (listBought.Count() + 1).ToString()
                    };

                    linesItens.Add(String.Join(",", aux));
                });

                using (StreamWriter writer = File.AppendText(pathProducts))
                {
                    linesItens.ForEach(j =>
                    {
                        writer.WriteLine(j);
                    });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
        }

        //Atualiza o status da compra
        public void UpdateStatus(int boughtId, StatusBoughtEnum status)
        {
            var allBought = ListAll();
            var lines = new List<string>();

            if (File.Exists(path))
            {
                lines.Add(header);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    allBought.ForEach(p =>
                    {
                        var aux = new string[]
                        {
                            p.BoughtId.ToString(),
                            p.TotalPrice.ToString(),
                            p.AddressId.ToString(),
                            p.PaymentId.ToString(),
                            p.CodeBought.ToString(),
                            p.Cpf.ToString(),
                            p.StatusId.ToString(),
                            p.DateBought.ToString()
                        };

                        //Atualiza a linha que contém o produtoId
                        if (p.BoughtId == boughtId)
                        {
                            aux = new string[]
                            {
                                p.BoughtId.ToString(),
                                p.TotalPrice.ToString(),
                                p.AddressId.ToString(),
                                p.PaymentId.ToString(),
                                p.CodeBought.ToString(),
                                p.Cpf.ToString(),
                                Convert.ToInt32(status).ToString(),
                                p.DateBought.ToString()
                            };
                        }
                        lines.Add(String.Join(",", aux));
                    });
                }
                File.WriteAllLines(path, lines);
            }
        }
    }
}
