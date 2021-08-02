using SingleExperience.Entities.CartEntities;
using SingleExperience.Entities.Enums;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class CartDB
    {
        private string CurrentDirectory = null;
        private string path = null;
        private string pathItens = null;

        public CartDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
            pathItens = CurrentDirectory + @"..\..\..\..\\Database\ItensCart.csv";
        }

        /* Lê Arquivo CSV */
        //Cart
        public CartEntitie GetCart(string userId)
        {
            var cart = new CartEntitie();
            try
            {
                string[] carts = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    //Irá procurar o carrinho pelo userId
                    cart = carts
                        .Skip(1)
                        .Select(p => new CartEntitie
                        {
                            UserId = p.Split(',')[0],
                            DateCreated = DateTime.Parse(p.Split(',')[1]),
                        })
                        .FirstOrDefault(p => p.UserId == userId);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return cart;
        }

        //Pega o header do CSV
        public string GetHeader()
        {
            string[] carts = File.ReadAllLines(pathItens, Encoding.UTF8);
            return carts[0];
        }

        //Itens Cart
        public List<ItemEntitie> ListItens()
        {
            var itens = new List<ItemEntitie>();
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

                            var item = new ItemEntitie();
                            item.ProductCartId = int.Parse(fields[0]);
                            item.ProductId = int.Parse(fields[1]);
                            item.UserId = fields[2];
                            item.Name = fields[3];
                            item.CategoryId = int.Parse(fields[4]);
                            item.Amount = int.Parse(fields[5]);
                            item.StatusId = int.Parse(fields[6]);
                            item.Price = double.Parse(fields[7]);
                            item.IpComputer = fields[8];

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

        /* Editar Arquivo CSV */
        //Cria itens do carrinho, porém o carrinho só irá ser criado quando o usuário estiver logado
        public void AddItensCart(CartModel cart)
        {
            var cartDB = new CartDB();
            var listItensCart = cartDB.ListItens();
            var linesCart = new List<string>();
            var linesItens = new List<string>();
            var aux = 0;
            var sum = 1;

            try
            {
                listItensCart.ForEach(i =>
                {
                    if (i.ProductId == cart.ProductId && i.UserId == cart.UserId && i.StatusId != Convert.ToInt32(StatusProductEnum.Ativo))
                    {
                        EditStatusProduct(cart.ProductId, cart.UserId, StatusProductEnum.Ativo);
                        aux++;
                    }
                    else if (i.ProductId == cart.ProductId && i.UserId == cart.UserId)
                    {
                        sum += i.Amount;
                        EditAmount(cart.ProductId, cart.UserId, sum);
                        aux++;
                    }
                });

                //Create item Cart
                if (aux == 0)
                {
                    var auxItens = new String[]
                    {
                        (listItensCart.Count + 1).ToString(),
                        cart.ProductId.ToString(),
                        cart.UserId.ToString(),
                        cart.Name.ToString(),
                        cart.CategoryId.ToString(),
                        sum.ToString(),
                        cart.StatusId.ToString(),
                        cart.Price.ToString(),
                        cart.UserId.ToString()
                    };

                    linesItens.Add(String.Join(",", auxItens));

                    using (StreamWriter writer = File.AppendText(pathItens))
                    {
                        linesItens.ForEach(i =>
                        {
                            writer.WriteLine(i);
                        });
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
        }

        //Edita a quantidade do item, caso o usuário adiciona o produto mais uma vez no carrinho
        public void EditAmount(int productId, string session, int sub)
        {
            var cartDB = new CartDB();
            var listItens = cartDB.ListItens();
            var lines = new List<string>();

            if (File.Exists(pathItens))
            {
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
                    lines.Add(GetHeader());
                    listItens.ForEach(p =>
                    {
                        var aux = new string[]
                        {
                            p.ProductCartId.ToString(),
                            p.ProductId.ToString(),
                            p.UserId.ToString(),
                            p.Name,
                            p.CategoryId.ToString(),
                            p.Amount.ToString(),
                            p.StatusId.ToString(),
                            p.Price.ToString(),
                            p.IpComputer.ToString()
                        };

                        if (p.ProductId == productId && p.UserId == session)
                        {
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.UserId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                sub.ToString(),
                                p.StatusId.ToString(),
                                p.Price.ToString(),
                                p.IpComputer.ToString()
                            };
                        }
                        lines.Add(String.Join(",", aux));
                    });
                }
                File.WriteAllLines(pathItens, lines);
            }
        }

        //Edita o status do produto
        public void EditStatusProduct(int productId, string session, StatusProductEnum status)
        {
            var cartDB = new CartDB();
            var listItens = cartDB.ListItens();
            var lines = new List<string>();


            if (File.Exists(pathItens))
            {
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
                    lines.Add(GetHeader());
                    listItens.ForEach(p =>
                    {
                        var aux = new string[]
                        {
                            p.ProductCartId.ToString(),
                            p.ProductId.ToString(),
                            p.UserId.ToString(),
                            p.Name,
                            p.CategoryId.ToString(),
                            p.Amount.ToString(),
                            p.StatusId.ToString(),
                            p.Price.ToString(),
                            p.IpComputer.ToString()
                        };

                        if (p.ProductId == productId && p.UserId == session)
                        {
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.UserId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                1.ToString(),
                                Convert.ToInt32(status).ToString(),
                                p.Price.ToString(),
                                p.IpComputer.ToString()
                            };
                        }
                        lines.Add(String.Join(",", aux));
                    });
                }
                File.WriteAllLines(pathItens, lines);
            }
        }

        //Criar carrinho e edita produtos quando o usuário logar
        public void EditUserId(string session)
        {
            var client = new ClientDB();
            var cart = new CartDB();
            var ipComputer = client.ClientId();
            var currentCart = cart.GetCart(session);
            var linesCart = new List<string>();
            var linesItens = new List<string>();

            //Create Cart
            if (currentCart == null)
            {
                currentCart = new CartEntitie();

                currentCart.UserId = session;
                currentCart.DateCreated = DateTime.Now;

                var auxCart = new string[]
                {
                        currentCart.UserId,
                        currentCart.DateCreated.ToString()
                };

                linesCart.Add(String.Join(",", auxCart));

                using (StreamWriter writer = File.AppendText(path))
                {
                    linesCart.ForEach(i =>
                    {
                        writer.WriteLine(i);
                    });
                }
            }

            if (cart.ListItens() != null && cart.ListItens().Count != 0)
            {
                linesItens.Add(GetHeader());
                cart.ListItens()
                    .ForEach(i =>
                    {
                        using (StreamWriter writer = new StreamWriter(pathItens))
                        {
                            var auxItens = new string[]
                            {
                                i.ProductCartId.ToString(),
                                i.ProductId.ToString(),
                                i.UserId,
                                i.Name,
                                i.CategoryId.ToString(),
                                i.Amount.ToString(),
                                i.StatusId.ToString(),
                                i.Price.ToString(),
                                i.IpComputer,
                            };

                            if (i.IpComputer == i.UserId && i.IpComputer == ipComputer)
                            {
                                auxItens = new string[]
                                {
                                    i.ProductCartId.ToString(),
                                    i.ProductId.ToString(),
                                    session,
                                    i.Name,
                                    i.CategoryId.ToString(),
                                    i.Amount.ToString(),
                                    i.StatusId.ToString(),
                                    i.Price.ToString(),
                                    i.IpComputer,
                                };
                            }
                            linesItens.Add(String.Join(",", auxItens));
                        }
                    });
                if (linesItens.Count != 0)
                {
                    File.WriteAllLines(pathItens, linesItens);
                }
            }
        }
    }
}
