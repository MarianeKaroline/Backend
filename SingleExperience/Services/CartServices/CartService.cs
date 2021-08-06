using SingleExperience.Services.CartServices.Models;
using SingleExperience.Entities.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SingleExperience.Enums;

namespace SingleExperience.Services.CartServices
{
    class CartService
    {
        private ProductDB productDB;
        private CartDB cartDB;
        private ClientDB clientDB;
        public CartService()
        {
            productDB = new ProductDB();
            cartDB = new CartDB();
            clientDB = new ClientDB();
        }

        //Listar produtos no carrinho
        public List<ProductCartModel> ItemCart(ParametersModel parameters, StatusProductEnum status)
        {
            var itensCart = cartDB.ListItens(parameters.Session);
            var prod = new List<ProductCartModel>();

            if (parameters.Session.Length == 11)
            {
                try
                {
                    prod = itensCart
                        .Where(i => i.UserId == parameters.Session && i.StatusId == Convert.ToInt32(status))
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = j.Name,
                            StatusId = j.StatusId,
                            CategoryId = j.CategoryId,
                            Price = j.Price,
                            Amount = j.Amount,
                            UserId = j.UserId
                        })
                        .ToList();
                }
                catch (IOException e)
                {
                    Console.WriteLine("Ocorreu um erro");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                prod = parameters.CartMemory
                        .Where(i => i.UserId == parameters.Session && i.StatusId == Convert.ToInt32(status))
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = j.Name,
                            StatusId = j.StatusId,
                            CategoryId = j.CategoryId,
                            Price = j.Price,
                            Amount = j.Amount,
                            UserId = j.UserId
                        })
                        .ToList();
            }
            return prod;
        }

        //Total do Carrinho
        public TotalCartModel TotalCart(ParametersModel parameters)
        {
            var itens = ItemCart(parameters, StatusProductEnum.Ativo);
            var total = new TotalCartModel();

            if (parameters.Session.Length == 11)
            {
                total.TotalAmount = itens
                    .Where(item => item.UserId == parameters.Session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                    .Sum(item => item.Amount);
                total.TotalPrice = itens
                    .Where(item => item.UserId == parameters.Session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                    .Sum(item => item.Price * item.Amount);
            }
            else
            {
                if (parameters.CartMemory.Count == 0)
                {
                    total.TotalAmount = 0;
                    total.TotalPrice = 0;
                }
                else
                {
                    total.TotalAmount = parameters.CartMemory.Sum(item => item.Amount);
                    total.TotalPrice = parameters.CartMemory.Sum(item => item.Price * item.Amount);
                }
            }


            return total;
        }

        //Remove um item do carrinho
        public void RemoveItem(int productId, string session, ParametersModel parameters)
        {
            var listItens = cartDB.ListItens(session);
            var sum = 0;
            var count = 0;

            if (session.Length == 11)
            {
                listItens
                    .Where(i => i.ProductId == productId)
                    .ToList()
                    .ForEach(p => 
                    {
                        if (p.Amount > 1 && count == 0)
                        {
                            sum = p.Amount - 1;
                            cartDB.EditAmount(productId, session, sum);
                            count++;
                        }
                        else if (p.Amount == 1)
                        {
                            cartDB.EditStatusProduct(productId, session, StatusProductEnum.Inativo);
                        }
                    });          
            }
            else
            {
                var aux = 0;
                parameters.CartMemory.ForEach(i =>
                {
                    if (i.ProductId == productId && i.Amount > 1)
                    {
                        i.Amount -= 1;
                    }
                    else if (i.ProductId == productId && i.Amount == 1)
                    {
                        aux++;
                    }
                });

                if (aux > 0)
                {
                    parameters.CartMemory.RemoveAll(x => x.ProductId == productId);
                }

            }

        }

        //Ver produtos antes da compra e depois
        public PreviewBoughtModel PreviewBoughts(ParametersModel parameters, BoughtModel bought)
        {
            var preview = new PreviewBoughtModel();
            var client = clientDB.GetClient(bought.Session);
            var address = clientDB.ListAddress(client.AddressId);
            var card = clientDB.ListCard(bought.Session);
            var itens = cartDB.ListItens(bought.Session);
            var listProducts = new List<ProductCartModel>();

            //Pega alguns atributos do cliente
            preview.FullName = client.FullName;
            preview.Phone = client.Phone;

            //Pega alguns atributos do endereço
            address
                .ForEach(i =>
                {
                    preview.Cep = i.Cep;
                    preview.Street = i.Street;
                    preview.Number = i.Number;
                    preview.City = i.City;
                    preview.State = i.State;
                });

            preview.Method = bought.Method;

            if (bought.Method == PaymentMethodEnum.CreditCard)
            {
                card
                    .Where(i => i.CardNumber.ToString().Contains(bought.Confirmation))
                    .ToList()
                    .ForEach(i =>
                    {
                        preview.NumberCard = i.CardNumber.ToString();
                    });
            }
            else if (bought.Method == PaymentMethodEnum.BankSlip)
            {
                var a = bought.Confirmation.Length;
                preview.Code = bought.Confirmation;
            }
            else
            {
                preview.Pix = bought.Confirmation;
            }

            if (bought.Ids.Count > 0)
            {
                bought.Ids.ForEach(i =>
                {
                    listProducts.Add(ItemCart(parameters, bought.Status)
                                    .Where(j => j.ProductId == i)
                                    .FirstOrDefault());
                });
                preview.Itens = listProducts;
            }
            else
            {
                preview.Itens = ItemCart(parameters, bought.Status);
            }

            return preview;
        }

        //Depois que confirma a compra, chama os métodos para alterar os status e diminuir a quantidade
        public bool Buy(List<BuyProductModel> products, string session)
        {
            var buy = false;

            products.ForEach(i =>
            {
                cartDB.EditStatusProduct(i.ProductId, session, i.Status);
                buy = productDB.EditAmount(i.ProductId, i.Amount);                
            });
            return buy;
        }
    }
}
