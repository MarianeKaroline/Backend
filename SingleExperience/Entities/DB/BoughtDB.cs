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
        private string[] boughts = null;
        private CartDB cartDB = null;
        private ClientDB clientDB = null;

        public BoughtDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Bought.csv";
            boughts = File.ReadAllLines(path, Encoding.UTF8);
            cartDB = new CartDB();
            clientDB = new ClientDB();
        }

        public List<BoughtEntitie> List(string userId)
        {
            var boughtEntitie = new List<BoughtEntitie>();
            try
            {
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
                            AddressId = int.Parse(p.Split(',')[8]),
                            PaymentId = int.Parse(p.Split(',')[9]),
                            CardId = int.Parse(p.Split(',')[10]),
                            DateBought = DateTime.Parse(p.Split(',')[11])
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

        public void Add(ParametersModel parameters, PaymentMethodEnum payment, List<BuyProductModel> buyProducts, string lastNumbers)
        {
            var listItens = new List<ItemEntitie>();
            var linesBought = new List<string>();
            var address = 0;

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
                        boughts.Length.ToString(),
                        i.ProductId.ToString(),
                        i.Cpf,
                        i.Name,
                        i.CategoryId.ToString(),
                        i.Amount.ToString(),
                        i.StatusId.ToString(),
                        i.Price.ToString(),
                        address.ToString(),
                        Convert.ToInt32(payment).ToString(),
                        lastNumbers,
                        DateTime.Now.ToString("dd MMM yyyy")
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
