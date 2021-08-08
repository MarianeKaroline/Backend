using SingleExperience.Entities.CartEntities;
using SingleExperience.Entities.ClientEntities;
using SingleExperience.Enums;
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
        private string CurrentDirectory = null;
        private string path = null;
        private string pathProducts = null;
        private CartDB cartDB = null;
        private ClientDB clientDB = null;

        public BoughtDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Bought.csv";
            pathProducts = CurrentDirectory + @"..\..\..\..\\Database\ProductBought.csv";
            cartDB = new CartDB();
            clientDB = new ClientDB();
        }

        public List<BoughtEntitie> List(string userId)
        {
            var boughtEntitie = new List<BoughtEntitie>();
            try
            {
                string[] boughts = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    //Irá procurar a compra pelo cpf do cliente
                    boughtEntitie = boughts
                        .Skip(1)
                        .Select(p => new BoughtEntitie
                        {
                            BoughtId = int.Parse(p.Split(',')[0]),
                            TotalPrice = double.Parse(p.Split(',')[1]),
                            AddressId = int.Parse(p.Split(',')[2]),
                            PaymentId = int.Parse(p.Split(',')[3]),
                            CodeBought = p.Split(',')[4],
                            Cpf = p.Split(',')[5],
                            DateBought = DateTime.Parse(p.Split(',')[6])
                        })
                        .Where(p => p.Cpf == userId)
                        .ToList();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return boughtEntitie;
        }

        //Lista os produtos comprados
        public List<ProductBoughtEntitie> ListProductBought(int boughtId)
        {
            var productBoughtEntitie = new List<ProductBoughtEntitie>();
            try
            {
                string[] productBoughts = File.ReadAllLines(pathProducts, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    //Irá procurar o os produtos da compra pela compra id
                    productBoughtEntitie = productBoughts
                        .Skip(1)
                        .Select(p => new ProductBoughtEntitie
                        {
                            ProductBoughtId = int.Parse(p.Split(',')[0]),
                            ProductId = int.Parse(p.Split(',')[1]),
                            Name = p.Split(',')[2],
                            CategoryId = int.Parse(p.Split(',')[3]),
                            Amount = int.Parse(p.Split(',')[4]),
                            StatusId = int.Parse(p.Split(',')[5]),
                            Price = double.Parse(p.Split(',')[6]),                            
                            BoughtId = int.Parse(p.Split(',')[7]),
                        })
                        .Where(p => p.BoughtId == boughtId)
                        .ToList();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return productBoughtEntitie;
        }

        public void Add(ParametersModel parameters, PaymentMethodEnum payment, List<BuyProductModel> buyProducts, string lastNumbers, double totalPrice, int addressId)
        {
            string[] productBoughts = File.ReadAllLines(pathProducts, Encoding.UTF8);
            var listBought = List(parameters.Session);
            var getCart = cartDB.GetCart(parameters.Session);

            var listItens = new List<ItemEntitie>();
            var linesItens = new List<string>();
            var linesBought = new List<string>();
            string numberCard = "";
            var card = new ClientDB();
            var data = DateTime.Now.ToString("G");

            if (payment == PaymentMethodEnum.CreditCard)
            {
                numberCard = card.ListCard(parameters.Session)
                    .Where(p => p.CardNumber
                        .ToString()
                        .Contains(lastNumbers))
                    .FirstOrDefault()
                    .CardNumber
                    .ToString();
            }
            else
            {
                numberCard = lastNumbers;
            }

            try
            {            
                //Adiciona compra no csv
                var auxBought = new string[]
                {
                    (listBought.Count() + 1).ToString(),
                    totalPrice.ToString(),
                    addressId.ToString(),
                    Convert.ToInt32(payment).ToString(),
                    numberCard,
                    parameters.Session,
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
                buyProducts.ForEach(j =>
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
                        productBoughts.Length.ToString(),
                        i.ProductId.ToString(),
                        i.Name,
                        i.CategoryId.ToString(),
                        i.Amount.ToString(),
                        i.StatusId.ToString(),
                        i.Price.ToString(),
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
    }
}
