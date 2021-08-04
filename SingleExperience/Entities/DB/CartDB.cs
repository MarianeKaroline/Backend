using SingleExperience.Entities.ProductEntities.CartEntities;
using SingleExperience.Entities.ProductEntities.Enums;
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
        public string header = "";

        public CartDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
            pathItens = CurrentDirectory + @"..\..\..\..\\Database\ItensCart.csv";
        }

        //Lê Todas as linhas quando pedir
        //Preciso pra que quando eu tiver que atualizar o arquivo csv, eu tenha todas as linhas da tabela
        public string[] ReadItens()
        {
            return File.ReadAllLines(pathItens, Encoding.UTF8);
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

        //Itens Cart
        public List<ItemEntitie> ListItens(string userId)
        {
            var itens = new List<ItemEntitie>();
            try
            {
                header = ReadItens()[0];
                using (StreamReader sr = File.OpenText(pathItens))
                {
                    itens = ReadItens()
                        .Skip(1)
                        .Select(i => new ItemEntitie
                        {
                            ProductCartId = int.Parse(i.Split(',')[0]),
                            ProductId = int.Parse(i.Split(',')[1]),
                            UserId = i.Split(',')[2],
                            Name = i.Split(',')[3],
                            CategoryId = int.Parse(i.Split(',')[4]),
                            Amount = int.Parse(i.Split(',')[5]),
                            StatusId = int.Parse(i.Split(',')[6]),
                            Price = double.Parse(i.Split(',')[7]),
                            IpComputer = i.Split(',')[8]
                        })
                        .Where(i => i.UserId == userId)
                        .ToList();

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
            var listItensCart = ListItens(cart.UserId);
            var linesCart = new List<string>();
            var linesItens = new List<string>();
            var aux = 0;
            var sum = 1;

            try
            {
                listItensCart.ForEach(i =>
                {
                    if (i.ProductId == cart.ProductId && i.StatusId != Convert.ToInt32(StatusProductEnum.Ativo))
                    {
                        EditStatusProduct(cart.ProductId, cart.UserId, StatusProductEnum.Ativo);
                        aux++;
                    }
                    else if (i.ProductId == cart.ProductId)
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
                        ReadItens().Length.ToString(),
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
            var listItens = ListItens(session);
            var lines = new List<string>();

            if (File.Exists(pathItens))
            {
                lines.Add(header);
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
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

                        //Atualiza a linha que contém o produtoId
                        if (p.ProductId == productId)
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
            var listItens = cartDB.ListItens(session);
            var lines = new List<string>();
            var auxAmount = 0;

            

            if (File.Exists(pathItens))
            {
                lines.Add(header);
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
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

                        if (p.ProductId == productId)
                        {
                            if (status == StatusProductEnum.Ativo)
                            {
                                auxAmount = 1;
                            }
                            else
                            {
                                auxAmount = p.Amount;
                            }
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.UserId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                auxAmount.ToString(),
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
            var linesItens = new string[ReadItens().Length];

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

            //Atualiza os itens para a sessão do usuário logado
            for (int i = 0; i < ReadItens().Length; i++)
            {
                linesItens[i] = ReadItens()[i];
                if (ReadItens()[i].Contains($",{ipComputer},") && ReadItens()[i].Contains($",{ipComputer}"))
                {
                    var aux = ReadItens()[i].Split(',');

                    var auxItens = new string[]
                    {
                        aux[0],
                        aux[1],
                        session,
                        aux[3],
                        aux[4],
                        aux[5],
                        aux[6],
                        aux[7],
                        aux[8]
                    };
                    linesItens[i] = String.Join(',', auxItens);
                }
            }

            File.WriteAllLines(pathItens, linesItens);
        }
    }
}
