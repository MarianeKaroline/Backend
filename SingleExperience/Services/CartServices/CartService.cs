using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SingleExperience.Entities.CartEntities;
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
        private string pathProducts = null;
        private string header = null;

        public CartService()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
            pathItens = CurrentDirectory + @"..\..\..\..\\Database\ItensCart.csv";
            pathProducts = CurrentDirectory + @"..\..\..\..\\Database\Products.csv";
            header = "";
        }

        //Lê arquivo csv carrinho
        public CartEntitie GetCart(string userId)
        {
            var cart = new CartEntitie();
            try
            {
                string[] carts = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    header = carts[0];

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

        //Lê o arquivo csv itens no carrinho
        public List<ItemEntitie> ListItens()
        {
            var itens = new List<ItemEntitie>();
            try
            {
                string[] itensCart = File.ReadAllLines(pathItens, Encoding.UTF8);
                header = itensCart[0];
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

        public List<ProductEntitie> ListProducts()
        {
            var CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var prod = new List<ProductEntitie>();

            try
            {
                string[] products = File.ReadAllLines(pathProducts, Encoding.UTF8);
                header = products[0];

                using (StreamReader sr = File.OpenText(pathProducts))
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

        //Adiciona no carrinho
        public void AddCart(CartModel cart)
        {
            var currentCart = GetCart(cart.UserId);
            var listItensCart = ListItens();
            var linesCart = new List<string>();
            var linesItens = new List<string>();
            var aux = 0;
            var sum = 1;

            try
            {
                listItensCart.ForEach(i =>
                {
                    if (i.ProductId == cart.ProductId && i.UserId == cart.UserId && i.StatusId == Convert.ToInt32(StatusProductEnum.Inativo))
                    {
                        EditCart(cart, StatusProductEnum.Ativo);
                        aux++;
                    }
                    else if (i.ProductId == cart.ProductId && i.UserId == cart.UserId)
                    {
                        sum += i.Amount;
                        EditAmount(cart, sum);
                        aux++;
                    }
                });

                //Create Cart
                if (currentCart == null)
                {
                    currentCart = new CartEntitie();

                    currentCart.UserId = cart.UserId;
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

        public void EditCart(CartModel cart, StatusProductEnum status)
        {
            var lines = new List<string>();

            var listItens = ListItens();

            if (File.Exists(pathItens))
            {
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
                    lines.Add(header);
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

                        if (p.ProductId == cart.ProductId && p.UserId == cart.UserId && p.StatusId != Convert.ToInt32(status))
                        {
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.UserId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                p.Amount.ToString(),
                                status.ToString(),
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

        public void EditAmount(CartModel cart, int sum)
        {
            var lines = new List<string>();

            var listItens = ListItens();

            if (File.Exists(pathItens))
            {
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
                    lines.Add(header);
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

                        if (p.ProductId == cart.ProductId && p.UserId == cart.UserId)
                        {
                            aux = new string[]
                            {
                                p.ProductCartId.ToString(),
                                p.ProductId.ToString(),
                                p.UserId.ToString(),
                                p.Name,
                                p.CategoryId.ToString(),
                                sum.ToString(),
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

        public void EditUserId(string session)
        {
            var linesCart = new List<string>();
            var linesItens = new List<string>();

            if (ListItens() != null && ListItens().Count != 0)
            {
                ListItens()
                    .Where(p => p.IpComputer == p.UserId)
                    .ToList()
                    .ForEach(i =>
                    {
                        var currentCart = GetCart(i.UserId);
                        linesCart.Add(header);
                        using (StreamWriter writer = new StreamWriter(path))
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
                        }
                    });
                File.WriteAllLines(path, linesCart);

                ListItens()
                    .ForEach(i =>
                    {
                        using (StreamWriter writer = new StreamWriter(pathItens))
                        {
                            linesItens.Add(header);
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

                            if (i.IpComputer == i.UserId)
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
                File.WriteAllLines(pathItens, linesItens);
            }
        }

        //Limpar carrinho do usuário
        public void RemoveAllCart(int productId, string session, StatusProductEnum status)
        {
            var lines = new List<string>();

            var listItens = ListItens();

            if (File.Exists(pathItens))
            {
                using (StreamWriter writer = new StreamWriter(pathItens))
                {
                    lines.Add(header);
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
                                p.Amount.ToString(),
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

        //Listar produtos no carrinho
        public List<ProductCartModel> ItemCart(string session)
        {
            var prod = new List<ProductCartModel>();
            var itensCart = ListItens();

            try
            {
                itensCart
                    .ToList()
                    .ForEach(j =>
                    {
                        if (j.UserId == session)
                        {
                            if (j.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                            {
                                var prodCart = new ProductCartModel();
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
        public TotalCartModel TotalCart(string session)
        {
            var itens = ItemCart(session);
            var total = new TotalCartModel();

            try
            {
                var totalItem = itens
                    .Select(i => new
                    {
                        i.Amount
                    })
                    .ToList();
                total.TotalAmount = itens
                    .Where(item => item.UserId == session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                    .Sum(item => item.Amount);
                totalItem.ForEach(p =>
                {
                    total.TotalPrice = itens
                        .Where(item => item.UserId == session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                        .Sum(item => item.Price * p.Amount);
                });
            }
            catch (Exception)
            {

                throw;
            }

            return total;
        }

        //Remove um item do carrinho
        public void RemoveItem(int productId, string session)
        {
            string[] products = File.ReadAllLines(pathItens, Encoding.UTF8);
            var count = 0;
            string[] aux = new string[products.Length];
            Console.WriteLine($"{Convert.ToInt32(StatusProductEnum.Inativo)}");

            for (int i = 0; i < products.Length; i++)
            {
                string[] field = products[i].Split(',');
                aux[i] = products[i];
                if (i != 0)
                {
                    if (products[i].Contains($",{productId},") && count == 0 && field[6] == "1")
                    {
                        aux[i] = products[i].Replace(",4002,", $",{Convert.ToInt32(StatusProductEnum.Inativo)},");
                        count++;
                    }
                    else if (products[i].Contains($",{productId},") && count == 0)
                    {
                        aux[i] = products[i].Replace($",{field[6]},{field[7]},", $",{Convert.ToInt32(field[6]) - 1},{field[7]},");
                        count++;
                    }
                }
            }

            File.WriteAllLines(pathItens, aux);
        }

        //Ver produto antes de comprar
        public List<PreviewBoughtModel> PreviewBoughts(string session, MethodPaymentEnum method, string lastNumbers)
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
            string cpf = "";

            var preview = new PreviewBoughtModel();

            client.ToList().ForEach(p =>
            {
                if (p.Contains($",{session},"))
                {
                    string[] field = p.Split(',');

                    cpf = field[0];
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

                    preview.Cep = field[1];
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
                    if (p.Contains($"{lastNumbers},"))
                    {
                        if (p.Contains($",{cpf}"))
                        {
                            string[] field = p.Split(',');
                            preview.NumberCard = field[0];
                        }
                    }
                });
            }

            preview.Itens = ItemCart(session);
            list.Add(preview);
            return list;
        }

        public bool Buy(List<BuyProductModel> products, string session)
        {
            var buy = false;
            var lines = new List<string>();
            var listItens = ListProducts();

            products.ForEach(i =>
            {
                RemoveAllCart(i.ProductId, session, i.Status);

                try
                {
                    if (File.Exists(pathProducts))
                    {
                        using (StreamWriter writer = new StreamWriter(pathProducts))
                        {
                            lines.Add(header);
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

                                if (p.ProductId == i.ProductId)
                                {
                                    var j = p.Amount - i.Amount;
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
                        File.WriteAllLines(pathProducts, lines);
                    }
                    buy = true;
                }
                catch (IOException e)
                {
                    Console.WriteLine("Ocorreu um erro");
                    Console.WriteLine(e.Message);
                }
            });
            return buy;
        }
    }
}
