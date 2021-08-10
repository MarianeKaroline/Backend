using SingleExperience.Services.CartServices.Models;
using SingleExperience.Entities.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SingleExperience.Enums;
using System.Text;

namespace SingleExperience.Services.CartServices
{
    class CartService
    {
        private ProductDB productDB = new ProductDB();
        private CartDB cartDB = new CartDB();

        //Listar produtos no carrinho
        public List<ProductCartModel> ItemCart(SessionModel parameters, StatusProductEnum status)
        {
            var prod = new List<ProductCartModel>();

            if (parameters.Session.Length == 11)
            {
                var getCart = cartDB.GetCart(parameters.Session);
                var itensCart = cartDB.ListItens(getCart.CartId);

                try
                {
                    prod = itensCart
                        .Where(i => i.StatusId == Convert.ToInt32(status))
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            Name = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Name,
                            CategoryId = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).CategoryId,
                            StatusId = j.StatusId,
                            Amount = j.Amount,
                            Price = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Price
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
                        .Where(i => i.StatusId == Convert.ToInt32(status))
                        .Select(j => new ProductCartModel()
                        {
                            ProductId = j.ProductId,
                            StatusId = j.StatusId,
                            Amount = j.Amount
                        })
                        .ToList();
            }
            return prod;
        }

        //Total do Carrinho
        public TotalCartModel TotalCart(SessionModel parameters)
        {
            var itens = ItemCart(parameters, StatusProductEnum.Ativo);
            var total = new TotalCartModel();

            if (parameters.Session.Length == 11)
            {
                total.TotalAmount = itens
                    .Where(item => item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                    .Sum(item => item.Amount);
                total.TotalPrice = itens
                    .Where(item => item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
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
                    total.TotalPrice = parameters.CartMemory.Sum(item => productDB.ListProducts().FirstOrDefault(i => i.ProductId == item.ProductId).Price * item.Amount);
                }
            }

            return total;
        }

        //Remove um item do carrinho
        public void RemoveItem(int productId, string session, SessionModel parameters)
        {
            var getCart = cartDB.GetCart(session);
            var listItens = cartDB.ListItens(getCart.CartId);
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


        private ClientDB clientDB = new ClientDB();
        //Ver produtos antes da compra e depois
        public PreviewBoughtModel PreviewBoughts(SessionModel parameters, BuyModel bought, int addressId)
        {
            var preview = new PreviewBoughtModel();
            var client = clientDB.GetClient(bought.Session);
            var address = clientDB.ListAddress(parameters.Session);
            var card = clientDB.ListCard(bought.Session);
            var cart = cartDB.GetCart(bought.Session);
            var itens = cartDB.ListItens(cart.CartId);
            var listProducts = new List<ProductCartModel>();

            //Pega alguns atributos do cliente
            preview.FullName = client.FullName;
            preview.Phone = client.Phone;

            //Pega alguns atributos do endereço
            var aux = address
                .FirstOrDefault(i => i.AddressId == addressId);

            preview.Cep = aux.Cep;
            preview.Street = aux.Street;
            preview.Number = aux.Number;
            preview.City = aux.City;
            preview.State = aux.State;

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

        //Depois que confirma a compra, chama os métodos para alterar os status do carrinho
        public bool Buy(List<BuyProductModel> products, string session)
        {
            var buy = false;

            products.ForEach(i =>
            {
                cartDB.EditStatusProduct(i.ProductId, session, i.Status);
                buy = true;
            });
            return buy;
        }
    }
}
