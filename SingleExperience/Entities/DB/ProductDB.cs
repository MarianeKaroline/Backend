using SingleExperience.Entities.ProductEntities;
using SingleExperience.Services.ProductServices.Models;
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
        private string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Products.csv";
        private string[] products = File.ReadAllLines(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Products.csv", Encoding.UTF8);
        public string header;

        public ProductDB()
        {
            header = products[0];
        }

        //Lê o arquivo CSV Produtos
        public List<ProductEntitie> ListProducts()
        {
            return products
                .Skip(1)
                .Select(i => new ProductEntitie()
                {
                    ProductId = int.Parse(i.Split(',')[0]),
                    Name = i.Split(',')[1],
                    Price = double.Parse(i.Split(',')[2]),
                    Detail = i.Split(',')[3],
                    Amount = int.Parse(i.Split(',')[4]),
                    CategoryId = int.Parse(i.Split(',')[5]),
                    Ranking = int.Parse(i.Split(',')[6]),
                    Available = bool.Parse(i.Split(',')[7]),
                    Rating = float.Parse(i.Split(',')[8])
                })
                .ToList();
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
                    lines.Add(header);
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        listItens.ForEach(p =>
                        {
                            var aux = new string[]
                            {
                                    p.ProductId.ToString(),
                                    p.Name,
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

        //Deixando o produto disponivel ou indisponivel
        public bool EditAvailable(int productId, bool available)
        {
            var listItens = ListProducts();
            var lines = new List<string>();
            var buy = false;
            try
            {
                if (File.Exists(path))
                {
                    lines.Add(header);
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        listItens.ForEach(p =>
                        {
                            var aux = new string[]
                            {
                                    p.ProductId.ToString(),
                                    p.Name,
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
                                aux = new string[]
                                {
                                        p.ProductId.ToString(),
                                        p.Name.ToString(),
                                        p.Price.ToString(),
                                        p.Detail.ToString(),
                                        p.Amount.ToString(),
                                        p.CategoryId.ToString(),
                                        p.Ranking.ToString(),
                                        available.ToString(),
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

        public void AddNewProducts(AddNewProductModel newProduct)
        {
            var product = File.ReadAllLines(path, Encoding.UTF8);
            var linesAddress = new List<string>();
            try
            {
                var auxAddress = new string[]
                {
                    product.Length.ToString(),
                    newProduct.Name,
                    newProduct.Price.ToString(),
                    newProduct.Detail,
                    newProduct.Amount.ToString(),
                    newProduct.CategoryId.ToString(),
                    newProduct.Ranking.ToString(),
                    newProduct.Available.ToString(),
                    newProduct.Rating.ToString()
                };
                linesAddress.Add(String.Join(",", auxAddress));


                using (StreamWriter sw = File.AppendText(path))
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

        }
    }
}
