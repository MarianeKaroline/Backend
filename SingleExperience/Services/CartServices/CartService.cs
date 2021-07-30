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
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Entities.Enums;

namespace SingleExperience.Services.CartServices
{
    class CartService
    {
        private string CurrentDirectory = null;
        private string path = null;
        private string pathItens = null;

        public CartService()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
            pathItens = CurrentDirectory + @"..\..\..\..\\Database\ItensCart.csv";
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
                            itens.UserId = fields[2];
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

        //Lê o arquivo csv Status
        public List<StatusProductEntitie> ListStatus()
        {
            var statusCart = new List<StatusProductEntitie>();
            try
            {
                var pathStatus = CurrentDirectory + @"..\..\..\..\\Database\Status.csv";
                string[] status = File.ReadAllLines(pathStatus, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    status
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var itens = new StatusProductEntitie();
                            itens.StatusId = int.Parse(fields[0]);
                            itens.Description = fields[1];

                            statusCart.Add(itens);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return statusCart;
        }

        //Lê o arquivo csv itens no carrinho
        public List<ItensEntities> ListItens()
        {
            var itens = new List<ItensEntities>();
            try
            {
                string[] itensCart = File.ReadAllLines(pathItens, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(pathItens))
                {
                    itensCart
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var item = new ItensEntities();
                            item.ProductCartId = int.Parse(fields[0]);
                            item.CartId = int.Parse(fields[1]);
                            item.ProductId = int.Parse(fields[2]);
                            item.UserId = fields[3];
                            item.Name = fields[4];
                            item.CategoryId = int.Parse(fields[5]);
                            item.Amount = int.Parse(fields[6]);
                            item.StatusId = int.Parse(fields[7]);
                            item.Price = double.Parse(fields[8]);

                            itens.Add(item);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return itens;
        }

        //Adiciona no carrinho
        public void AddCart(int productId, string ipComputer)
        {
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            string[] product = File.ReadAllLines(pathProduct, Encoding.UTF8);
            string[] cart = File.ReadAllLines(path, Encoding.UTF8);
            string[] itensCart = File.ReadAllLines(pathItens, Encoding.UTF8);
            string[] aux1 = new string[itensCart.Length];
            var linesItens = "";
            var aux = "";
            var sum = 1;
            var count = 0;

            try
            {
                cart.ToList().ForEach(p =>
                {
                    if (p.Contains($",{ipComputer},"))
                    {
                        string[] field = p.Split(',');
                        aux = field[0];
                        count++;
                    }
                                        
                });

                itensCart.ToList().ForEach(p =>
                {
                    if (p.Contains($",{productId},"))
                    {
                        sum++;
                    }
                });
                for (int j = 0; j < product.Length; j++)
                {
                    var fields = product[j].Split(',');
                    if (j != 0)
                    {
                        if (count == 0 && fields[0] == productId.ToString())
                        {
                            string linesCart = $"{cart.Length},{ipComputer},{DateTime.Now}";

                            using (StreamWriter sw = File.AppendText(path))
                            {
                                sw.WriteLine(linesCart);
                            }
                        }
                        if (fields[0] == productId.ToString() && sum == 1)
                        {
                            linesItens = $"{itensCart.Length},{cart.Length},{productId},{ipComputer},{fields[1]},{fields[5]},{sum},{Convert.ToInt32(StatusProductEnums.Ativo)},{fields[2]}";

                            using (StreamWriter sw = File.AppendText(pathItens))
                            {
                                sw.WriteLine(linesItens);
                            }

                        }
                        else if (fields[0] == productId.ToString() && sum > 1)
                        {
                            for (int i = 0; i < itensCart.Length; i++)
                            {
                                aux1[i] = itensCart[i];
                                var help = aux1[i].Split(',');
                                if (itensCart[i].Contains($",{productId},") && itensCart[i].Contains($"{Convert.ToInt32(StatusProductEnums.Ativo)}"))
                                {
                                    aux1[i] = itensCart[i].Replace($",{help[6]},{help[7]}", $",{sum},{help[7]}");
                                    count++;
                                }
                                else if (itensCart[i].Contains($",{productId},"))
                                {
                                    aux1[i] = itensCart[i].Replace($",{help[6]},{help[7]}", $",{help[6]},{Convert.ToInt32(StatusProductEnums.Ativo)}");
                                    count++;
                                }
                            }
                            File.WriteAllLines(pathItens, aux1);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
        }

        //Limpar carrinho do usuário
        //Arrumar
        public void RemoveAllCart(string ipComputer)
        {
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\ItensCarts.csv";
            string[] productInative = File.ReadAllLines(pathProduct, Encoding.UTF8);

            if (File.Exists(pathProduct))
            {
                using (StreamWriter writer = new StreamWriter(pathProduct))
                {
                    for (int i = 0; i < productInative.Length; i++)
                    {
                        if (productInative[i].Contains($"{Convert.ToInt32(StatusProductEnums.Ativo)}"))
                        {
                            string a = productInative[i];
                            productInative[i] = a.Replace(",4002,", ",4001,");
                        }
                    }
                }
                File.WriteAllLines(pathProduct, productInative);
            }
        }

        //Listar produtos no carrinho
        public List<ProductsCartModel> ItemCart(string ipComputer)
        {
            var prod = new List<ProductsCartModel>();
            var pathProduct = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            var itensCart = ListItens();

            try
            {
                itensCart
                    .ToList()
                    .ForEach(j =>
                    {
                        if (j.UserId == ipComputer)
                        {
                            if (j.StatusId == Convert.ToInt32(StatusProductEnums.Ativo))
                            {
                                var prodCart = new ProductsCartModel();
                                prodCart.ProductId = j.ProductId;
                                prodCart.Name = j.Name;
                                prodCart.StatusId = j.StatusId;
                                prodCart.CategoryId = j.CategoryId;
                                prodCart.Price = j.Price;
                                prodCart.Amount = j.Amount;
                                prodCart.UserId = j.UserId;

                                prod.Add(prodCart);
                            }
                        }
                    });
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prod;
        }

        //Total do Carrinho
        public TotalCartModel TotalCart(string ipComputer)
        {
            var itens = ItemCart(ipComputer);
            var total = new TotalCartModel();

            try
            {
                total.TotalPrice = itens
                    .Where(item => item.UserId == ipComputer && item.StatusId == Convert.ToInt32(StatusProductEnums.Ativo))
                    .Sum(item => item.Price);
                total.TotalAmount = itens
                    .Where(item => item.UserId == ipComputer && item.StatusId == Convert.ToInt32(StatusProductEnums.Ativo))
                    .Sum(item => item.Amount);
            }
            catch (Exception)
            {

                throw;
            }

            return total;
        }

        //Remove um item do carrinho
        public void RemoveItem(int productId, string ipComputer)
        {
            string[] products = File.ReadAllLines(pathItens, Encoding.UTF8);
            var count = 0;
            string[] aux = new string[products.Length];
            Console.WriteLine($"{Convert.ToInt32(StatusProductEnums.Inativo)}");

            for (int i = 0; i < products.Length; i++)
            {
                string[] field = products[i].Split(',');
                aux[i] = products[i];
                if (i != 0)
                {
                    if (products[i].Contains($",{productId},") && count == 0 && field[6] == "1")
                    {
                        aux[i] = products[i].Replace(",4002,", $",{Convert.ToInt32(StatusProductEnums.Inativo)},");
                        count++;
                    }
                    else if (products[i].Contains($",{productId},") && count == 0)
                    {
                        aux[i] = products[i].Replace($",{field[6]},{field[7]},", $",{Convert.ToInt32(field[6])-1},{field[7]},");
                        count++;
                    }
                }
            }

            File.WriteAllLines(pathItens, aux);
        }

        public List<PreviewBoughtModel> PreviewBoughts(long session, Enum method)
        {
            var list = new List<PreviewBoughtModel>();
            var pathClient = CurrentDirectory + @"..\..\..\..\\Database\Client.csv";
            var pathAddress = CurrentDirectory + @"..\..\..\..\\Database\Address.csv";
            var pathCards = CurrentDirectory + @"..\..\..\..\\Database\Card.csv";
            var itens = ListItens();
            string[] client = File.ReadAllLines(pathClient, Encoding.UTF8);
            string[] address = File.ReadAllLines(pathAddress, Encoding.UTF8);
            string[] card = File.ReadAllLines(pathCards, Encoding.UTF8);
            int aux = 0;
            long cpf = 0;

            var preview = new PreviewBoughtModel();

            client.ToList().ForEach(p =>
            {
                if (p.Contains($",{session},"))
                {
                    string[] field = p.Split(',');

                    cpf = long.Parse(field[0]);
                    aux = int.Parse(field[6]);
                    preview.FullName = field[1];
                    preview.Phone = field[2];
                }
            });

            address.ToList().ForEach(p =>
            {
                if (p.Contains($"{aux},"))
                {
                    string[] field = p.Split(',');

                    preview.Street = field[2];
                    preview.Number = field[3];
                    preview.City = field[4];
                    preview.State = field[5];
                }
            });

            preview.Method = method;
            if (Convert.ToInt32(method) == 1)
            {
                card.ToList().ForEach(p =>
                {
                    if (p.Contains($",{cpf}"))
                    {
                        string[] field = p.Split(',');
                        preview.NumberCard = field[0];
                    }
                });
            }

            //itens.ForEach(p =>
            //{
            //    preview.ProductName.Add()

            //})
            return list;
        }
    }
}
