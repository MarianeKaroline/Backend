using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SingleExperience.Entities.CartEntities;
using System.Net;
using System.Net.Sockets;
using SingleExperience.Entities.ProductsEntities;
using SingleExperience.Entities;
using SingleExperience.Services.ProductServices.Models.CartModels;

namespace SingleExperience.Services.CartServices
{
    class CartService
    {
        private int CartId = 0;
        private string CurrentDirectory = null;
        private string path = null;

        public CartService()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
        }

        //Lê arquivo csv carrinho
        public List<CartEntitie> ListCart()
        {
            var prodCart = new List<CartEntitie>();
            try
            {
                string[] products = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    products
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var itens = new CartEntitie();
                            itens.CartId = int.Parse(fields[0]);
                            itens.ProductId = int.Parse(fields[1]);
                            itens.UserId = fields[2];
                            itens.ProductStatus = int.Parse(fields[3]);
                            itens.DateCreated = DateTime.Parse(fields[4]);

                            prodCart.Add(itens);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prodCart;
        }

        //Adiciona no carrinho
        public int AddCart(int productId, string ipComputer)
        {
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            string[] product = File.ReadAllLines(pathProduct, Encoding.UTF8);
            CartId++;
            var countProducts = 0;

            if (File.Exists(path))
            {
                string lines = $"{CartId},{productId},{ipComputer},4002,{DateTime.Now}";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(lines);
                }
                countProducts = CountProducts(ipComputer);
            }

            if (File.Exists(pathProduct))
            {
                using (StreamWriter writer = new StreamWriter(pathProduct))
                {
                    for (int i = 0; i < product.Length; i++)
                    {
                        if (product[i].Contains(productId.ToString()))
                        {
                            string a = product[i];
                            product[i] = a.Replace(",4001,", ",4002,");
                        }
                    }
                }
                File.WriteAllLines(pathProduct, product);
            }

            return countProducts;
        }

        //Contar produtos no carrinho
        public int CountProducts(string ipComputer)
        {
            var count = 0;
            var products = ListCart();

            count = products
                .Where(p => p.UserId == ipComputer && p.ProductStatus == 4002)
                .Skip(0)
                .Count();
            return count;
        }

        //Limpar carrinho do usuário
        public void CleanAllCart(string ipComputer)
        {
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            string[] productInative = File.ReadAllLines(pathProduct, Encoding.UTF8);
            string[] product = File.ReadAllLines(path, Encoding.UTF8);

            if (File.Exists(pathProduct))
            {
                using (StreamWriter writer = new StreamWriter(pathProduct))
                {
                    for (int i = 0; i < productInative.Length; i++)
                    {
                        if (productInative[i].Contains(4002.ToString()))
                        {
                            string a = productInative[i];
                            productInative[i] = a.Replace(",4002,", ",4001,");
                        }
                    }
                }
                File.WriteAllLines(pathProduct, productInative);
            }

            if (File.Exists(path))
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < product.Length; i++)
                    {
                        if (product[i].Contains(4002.ToString()))
                        {
                            string a = product[i];
                            product[i] = a.Replace(",4002,", ",4001,");
                        }
                    }
                }
                File.WriteAllLines(path, productInative);
            }
        }

        //Listar produtos no carrinho
        public List<ProductsCartModel> ItemCard(string ipComputer)
        {
            var list = new List<ProductsCartModel>();
            var prod = new List<ProductsCartModel>();
            var aux = 0;
            var aux2 = 0;
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";

            try
            {
                var countId = 0;
                var countTotal = 0;
                var b = 0;
                var a = 0;
                string[] productCart = File.ReadAllLines(path, Encoding.UTF8);
                string[] product = File.ReadAllLines(pathProduct, Encoding.UTF8);
                string[] fieldsProdCart = null, fieldsProd = null;


                int count = CountProducts(ipComputer);

                for (int i = 1; i < product.Length; i++)
                {
                    fieldsProd = product[i].Split(',');
                    aux = int.Parse(fieldsProd[0]);
                    for (int j = 1; j < productCart.Length; j++)
                    {
                        fieldsProdCart = productCart[j].Split(',');
                        if (productCart[j].Contains(fieldsProd[0]) && product[i].Contains(",4002,"))
                        {
                            countTotal++;
                        }

                        if (aux == int.Parse(fieldsProdCart[1]))
                        {
                            countId++;
                            aux2++;
                        }
                        else
                        {
                            countId = 0;
                        }

                        if (fieldsProdCart[1] == fieldsProd[0] && countId < 2)
                        {
                            var prodCart = new ProductsCartModel();
                            prodCart.CartId = int.Parse(fieldsProdCart[0]);
                            prodCart.ProductId = int.Parse(fieldsProd[0]);
                            prodCart.UserId = fieldsProdCart[2];
                            prodCart.DateCreated = DateTime.Parse(fieldsProdCart[4]);
                            prodCart.Name = fieldsProd[1];
                            prodCart.CategoryId = int.Parse(fieldsProd[6]);
                            prodCart.Price = double.Parse(fieldsProd[2]);
                            prodCart.Amount = countId;
                            prodCart.TotalAmount = countTotal;
                            prodCart.TotalPrice = 0.0;

                            prod.Add(prodCart);
                        }
                        else
                        {
                            prod.ForEach(p => {
                                p.Amount = aux2;
                            });
                        }

                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prod;
        }
    }
}
