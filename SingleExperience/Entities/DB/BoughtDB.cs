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
        private CartDB cartDB = null;
        private ClientDB clientDB = null;

        public BoughtDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Bought.csv";
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
                    //Irá procurar o carrinho pelo userId
                    boughtEntitie = boughts
                        .Skip(1)
                        .Select(p => new BoughtEntitie
                        {
                            BoughtId = int.Parse(p.Split(',')[0]),
                            ProductId = int.Parse(p.Split(',')[1]),
                            Cpf = p.Split(',')[2],
                            Name = p.Split(',')[3],
                            CategoryId = int.Parse(p.Split(',')[4]),
                            Amount = int.Parse(p.Split(',')[5]),
                            StatusId = int.Parse(p.Split(',')[6]),
                            Price = double.Parse(p.Split(',')[7]),
                            TotalPrice = double.Parse(p.Split(',')[8]),
                            AddressId = int.Parse(p.Split(',')[9]),
                            PaymentId = int.Parse(p.Split(',')[10]),
                            NumberPayment = p.Split(',')[11],
                            DateBought = DateTime.Parse(p.Split(',')[12])
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

        public void Add(ParametersModel parameters, PaymentMethodEnum payment, List<BuyProductModel> buyProducts, string lastNumbers, double totalPrice)
        {
            var listBought = List(parameters.Session);
            var listItens = new List<ItemEntitie>();
            var linesBought = new List<string>();
            var address = 0;
            string numberCard = "";
            var card = new ClientDB();

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
                address = clientDB.GetClient(parameters.Session).AddressId;

                buyProducts.ForEach(j =>
                {
                    listItens.Add(cartDB.ListItens(parameters.Session)
                        .Where(i => 
                            i.StatusId == Convert.ToInt32(StatusProductEnum.Comprado) && 
                            i.ProductId == j.ProductId)
                        .FirstOrDefault());
                });

                listItens.ForEach(i =>
                {
                    var aux = new string[]
                    {
                        (listBought.Count() + 1).ToString(),
                        i.ProductId.ToString(),
                        i.Cpf,
                        i.Name,
                        i.CategoryId.ToString(),
                        i.Amount.ToString(),
                        i.StatusId.ToString(),
                        i.Price.ToString(),
                        totalPrice.ToString(),
                        address.ToString(),
                        Convert.ToInt32(payment).ToString(),
                        numberCard.ToString(),
                        DateTime.Now.ToString("dd/MMM/yyyy")
                    };

                    linesBought.Add(String.Join(",", aux));
                });         

                using (StreamWriter writer = File.AppendText(path))
                {
                    linesBought.ForEach(j =>
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
