using SingleExperience.Services.ProductServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class ProductDB
    {
        private string CurrentDirectory = null;
        private string path = null;

        public ProductDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
        }

        //Lê o arquivo CSV Produtos
        public List<ProductEntitie> ListProducts()
        {
            var prod = new List<ProductEntitie>();

            try
            {
                string[] products = File.ReadAllLines(path, Encoding.UTF8);

                using (StreamReader sr = File.OpenText(path))
                {
                    products
                        .Skip(1)
                        .ToList()
                        .ForEach(item =>
                        {
                            string[] fields = item.Split(',');

                            var produto = new ProductEntitie();

                            produto.ProductId = int.Parse(fields[0]);
                            produto.Name = fields[1];
                            produto.Price = double.Parse(fields[2]);
                            produto.Detail = fields[3];
                            produto.Amount = int.Parse(fields[4]);
                            produto.CategoryId = int.Parse(fields[5]);
                            produto.Ranking = int.Parse(fields[6]);
                            produto.Available = bool.Parse(fields[7]);
                            produto.Rating = float.Parse(fields[8]);


                            prod.Add(produto);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prod;
        }

        //Pega o header do CSV
        public string GetHeader()
        {
            string[] carts = File.ReadAllLines(path, Encoding.UTF8);
            return carts[0];
        }

        //Quando o usuario compra um item, a quantidade do produto diminui
        public bool EditAmount(int productId, int amount)
        {
            var listItens = ListProducts();
            var lines = new List<string>();
            var buy = false;
            try
            {
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        lines.Add(GetHeader());
                        listItens.ForEach(p =>
                        {
                            var aux = new string[]
                            {
                                    p.ProductId.ToString(),
                                    p.Name.ToString(),
                                    p.Price.ToString(),
                                    p.Detail.ToString(),
                                    p.Amount.ToString(),
                                    p.CategoryId.ToString(),
                                    p.Ranking.ToString(),
                                    p.Available.ToString(),
                                    p.Rating.ToString()
                            };

                            if (p.ProductId == productId)
                            {
                                var j = p.Amount - amount;
                                aux = new string[]
                                {
                                        p.ProductId.ToString(),
                                        p.Name.ToString(),
                                        p.Price.ToString(),
                                        p.Detail.ToString(),
                                        j.ToString(),
                                        p.CategoryId.ToString(),
                                        p.Ranking.ToString(),
                                        p.Available.ToString(),
                                        p.Rating.ToString()
                                };
                            }
                            lines.Add(String.Join(",", aux));
                        });
                    }
                    File.WriteAllLines(path, lines);
                }
                buy = true;
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return buy;
        }
    }
}
