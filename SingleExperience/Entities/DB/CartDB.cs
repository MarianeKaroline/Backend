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
        private string header = "";

        public CartDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
            pathItens = CurrentDirectory + @"..\..\..\..\\Database\ItensCart.csv";
            header = ReadItens()[0];
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
        //Criar carrinho e adiciona produtos
        public void AddItemCart(ParametersModel parameters, CartModel cartModel)
        {
            var client = new ClientDB();
            var cart = new CartDB();
            var ipComputer = client.ClientId();
            var currentCart = cart.GetCart(parameters.Session);
            var listItensCart = ListItens(parameters.Session);
            var linesCart = new List<string>();
            var linesItens = new List<string>();
            var aux = 0;
            var sum = 1;            

            try
            {
                //Criar Carrinho
                if (currentCart == null)
                {
                    currentCart = new CartEntitie();

                    currentCart.UserId = parameters.Session;
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

                if (parameters.CartMemory.Count > 0)
                {
                    if (aux == 0)
                    {
                        PassItens(parameters);
                    }
                }
                else
                {
                    listItensCart.ForEach(j =>
                    {
                        if (j.ProductId == cartModel.ProductId && j.StatusId != Convert.ToInt32(StatusProductEnum.Ativo))
                        {
                            EditStatusProduct(cartModel.ProductId, cartModel.UserId, StatusProductEnum.Ativo);
                            aux++;
                        }
                        else if (j.ProductId == cartModel.ProductId)
                        {
                            sum += j.Amount;
                            EditAmount(cartModel.ProductId, cartModel.UserId, sum);
                            aux++;
                        }
                    });

                    //Criar item na tabela
                    if (aux == 0)
                    {
                        var auxItens = new String[]
                        {
                            ReadItens().Length.ToString(),
                            cartModel.ProductId.ToString(),
                            cartModel.UserId.ToString(),
                            cartModel.Name.ToString(),
                            cartModel.CategoryId.ToString(),
                            sum.ToString(),
                            cartModel.StatusId.ToString(),
                            cartModel.Price.ToString(),
                            cartModel.UserId.ToString()
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

        //Passa os itens da memória para o banco
        public void PassItens(ParametersModel parameters)
        {
            var listItensCart = ListItens(parameters.Session);
            var linesItens = new List<string>();
            var aux = 0;
            var sum = 1;

            if (listItensCart.Count() > 0)
            {
                listItensCart.ForEach(j =>
                {
                    parameters.CartMemory.ForEach(i =>
                    {
                        if (j.ProductId == i.ProductId && j.StatusId != Convert.ToInt32(StatusProductEnum.Ativo))
                        {
                            EditStatusProduct(j.ProductId, j.UserId, StatusProductEnum.Ativo);
                            aux++;
                        }
                        else if (j.ProductId == i.ProductId)
                        {
                            sum += j.Amount;
                            EditAmount(j.ProductId, j.UserId, sum);
                            aux++;
                        }
                    });
                });
            }
            if (aux == 0)
            {
                parameters.CartMemory.ForEach(i =>
                {
                    var auxItens = new String[]
                    {
                        ReadItens().Length.ToString(),
                        i.ProductId.ToString(),
                        parameters.Session,
                        i.Name.ToString(),
                        i.CategoryId.ToString(),
                        sum.ToString(),
                        i.StatusId.ToString(),
                        i.Price.ToString(),
                        i.UserId.ToString()
                    };

                    linesItens.Add(String.Join(",", auxItens));
                });

                using (StreamWriter writer = File.AppendText(pathItens))
                {
                    linesItens.ForEach(i =>
                    {
                        writer.WriteLine(i);
                    });
                }
            }
        }

        /*Memória*/
        //Coloca produtos na memória quando usuário não estiver logado
        public List<ItemEntitie> AddItensMemory(CartModel cart, List<ItemEntitie> cartMemory)
        {
            if (cartMemory == null)
            {
                cartMemory = new List<ItemEntitie>();
            }
            var sum = 1;

            if (cart.ProductId != 0)
            {

                var aux = cartMemory
                        .Where(i => i.ProductId == cart.ProductId)
                        .FirstOrDefault();

                if (aux == null)
                {
                    var item = new ItemEntitie()
                    {
                        ProductCartId = 1,
                        ProductId = cart.ProductId,
                        UserId = cart.UserId,
                        Name = cart.Name,
                        CategoryId = cart.CategoryId,
                        Amount = sum,
                        StatusId = cart.StatusId,
                        Price = cart.Price,
                        IpComputer = cart.UserId,
                    };
                    cartMemory.Add(item);
                }
                else
                {
                    cartMemory.ForEach(i =>
                    {
                        i.ProductCartId = cartMemory.Count();
                        i.ProductId = cart.ProductId;
                        i.UserId = cart.UserId;
                        i.Name = cart.Name;
                        i.CategoryId = cart.CategoryId;
                        i.Amount += sum;
                        i.StatusId = cart.StatusId;
                        i.Price = cart.Price;
                        i.IpComputer = cart.UserId;
                    });
                }
            }

            return cartMemory;
        }
    }
}
