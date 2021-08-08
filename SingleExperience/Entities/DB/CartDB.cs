using SingleExperience.Entities.CartEntities;
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

        public string[] ReadCart()
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        /* Lê Arquivo CSV */
        //Cart
        public CartEntitie GetCart(string userId)
        {
            var cart = new CartEntitie();
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    //Irá procurar o carrinho pelo userId
                    cart = ReadCart()
                        .Skip(1)
                        .Select(p => new CartEntitie
                        {
                            CartId = int.Parse(p.Split(',')[0]),
                            Cpf = p.Split(',')[1],
                            DateCreated = DateTime.Parse(p.Split(',')[2]),
                        })
                        .FirstOrDefault(p => p.Cpf == userId);
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
        public List<ItemEntitie> ListItens(int cartId)
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
                            CartId = int.Parse(i.Split(',')[2]),
                            Name = i.Split(',')[3],
                            CategoryId = int.Parse(i.Split(',')[4]),
                            Amount = int.Parse(i.Split(',')[5]),
                            StatusId = int.Parse(i.Split(',')[6]),
                            Price = double.Parse(i.Split(',')[7])
                        })
                        .Where(i => i.CartId == cartId)
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
            var cart = new CartDB(); //Instanciar lá em cima
            var ipComputer = client.ClientId();
            var currentCart = cart.GetCart(parameters.Session);
            var listItensCart = ListItens(currentCart.CartId);
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
                    currentCart.Cpf = parameters.Session;
                    currentCart.DateCreated = DateTime.Now;

                    var auxCart = new string[]
                    {
                        ReadCart().Length.ToString(),
                        currentCart.Cpf,
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
                            ReadCart().Length.ToString(),
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
            var cart = GetCart(session);
            var listItens = ListItens(cart.CartId);
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
                            p.CartId.ToString(),
                            p.Name,
                            p.CategoryId.ToString(),
                            p.Amount.ToString(),
                            p.StatusId.ToString(),
                            p.Price.ToString()
                        };

                        //Atualiza a linha que contém o produtoId
                        if (p.ProductId == productId)
                        {
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.CartId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                sub.ToString(),
                                p.StatusId.ToString(),
                                p.Price.ToString()
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
            var cart = GetCart(session);
            var cartDB = new CartDB();
            var listItens = cartDB.ListItens(cart.CartId);
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
                            p.CartId.ToString(),
                            p.Name,
                            p.CategoryId.ToString(),
                            p.Amount.ToString(),
                            p.StatusId.ToString(),
                            p.Price.ToString()
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
                                p.CartId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                auxAmount.ToString(),
                                Convert.ToInt32(status).ToString(),
                                p.Price.ToString()
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
            var cart = new CartDB();
            var currentCart = cart.GetCart(parameters.Session);
            var linesCart = new List<string>();

            //Se carrinho está null ele cria um para o cliente
            if (currentCart == null)
            {
                currentCart = new CartEntitie();
                currentCart.Cpf = parameters.Session;
                currentCart.DateCreated = DateTime.Now;

                var auxCart = new string[]
                {
                    ReadCart().Length.ToString(),
                    currentCart.Cpf,
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

                currentCart = cart.GetCart(parameters.Session);
            }

            var listItensCart = ListItens(currentCart.CartId);
            var linesItens = new List<string>();
            var aux = 0;

            //Verifica se o cliente já possui esse produto no carrinho
            if (listItensCart.Count() > 0)
            {
                listItensCart.ForEach(j =>
                {
                    parameters.CartMemory.ForEach(i =>
                    {
                        if (j.ProductId == i.ProductId && j.StatusId != Convert.ToInt32(StatusProductEnum.Ativo))
                        {
                            EditStatusProduct(j.ProductId, parameters.Session, StatusProductEnum.Ativo);
                            EditAmount(j.ProductId, parameters.Session, i.Amount);
                            aux++;
                        }
                        else if (j.ProductId == i.ProductId)
                        {
                            EditAmount(j.ProductId, parameters.Session, i.Amount+1);
                            aux++;
                        }
                    });
                });
            }

            //Passa o produto para o carrinho
            if (aux == 0)
            {
                parameters.CartMemory.ForEach(i =>
                {
                    var auxItens = new String[]
                    {
                        ReadItens().Length.ToString(),
                        i.ProductId.ToString(),
                        currentCart.CartId.ToString(),
                        i.Name.ToString(),
                        i.CategoryId.ToString(),
                        i.Amount.ToString(),
                        i.StatusId.ToString(),
                        i.Price.ToString()
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
                        Name = cart.Name,
                        CategoryId = cart.CategoryId,
                        Amount = sum,
                        StatusId = cart.StatusId,
                        Price = cart.Price
                    };
                    cartMemory.Add(item);
                }
                else
                {
                    cartMemory.ForEach(i =>
                    {
                        i.ProductCartId = cartMemory.Count();
                        i.ProductId = cart.ProductId;
                        i.Name = cart.Name;
                        i.CategoryId = cart.CategoryId;
                        i.Amount += sum;
                        i.StatusId = cart.StatusId;
                        i.Price = cart.Price;
                    });
                }
            }

            return cartMemory;
        }
    }
}
