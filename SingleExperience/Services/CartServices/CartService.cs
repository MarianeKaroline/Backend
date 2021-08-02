using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SingleExperience.Services.ProductServices.Models.CartModels;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Entities.Enums;
using SingleExperience.Entities.DB;

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
        public List<ProductCartModel> ItemCart(string session, StatusProductEnum status)
        {
            var itensCart = cartDB.ListItens();
            var prod = new List<ProductCartModel>();

            try
            {
                itensCart
                    .Where(i => i.UserId == session && i.StatusId == Convert.ToInt32(status))
                    .ToList()
                    .ForEach(j =>
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
            var itens = ItemCart(session, StatusProductEnum.Ativo);
            var total = new TotalCartModel();

            total.TotalAmount = itens
                .Where(item => item.UserId == session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                .Sum(item => item.Amount);
            total.TotalPrice = itens
                .Where(item => item.UserId == session && item.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                .Sum(item => item.Price * item.Amount);

            return total;
        }

        //Remove um item do carrinho
        public void RemoveItem(int productId, string session)
        {
            var listItens = cartDB.ListItens();
            var sum = 0;
            var count = 0;

            listItens
                .Where(i => i.UserId == session && i.ProductId == productId)
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

        //Ver produtos antes da compra e depois
        public List<PreviewBoughtModel> PreviewBoughts(string session, PaymentMethodEnum method, string confirmation, StatusProductEnum status)
        {
            var list = new List<PreviewBoughtModel>();
            var preview = new PreviewBoughtModel();
            var client = clientDB.GetClient(session);
            var address = clientDB.ListAddress(client.AddressId);
            var card = clientDB.ListCard(session);
            var itens = cartDB.ListItens();

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

            preview.Method = method;
            if (method == PaymentMethodEnum.CreditCard) //Só ira adicionar o número do cartão se o método for cartão
            {
                card.ForEach(i =>
                {
                    var teste = i.CardNumber.ToString().Substring(12);
                    if (teste == confirmation)
                    {
                        preview.NumberCard = i.CardNumber.ToString();
                    }
                });
            }
            else if (method == PaymentMethodEnum.BankSlip)
            {
                preview.Code = confirmation;
            }
            else
            {
                preview.Pix = confirmation;
            }

            preview.Itens = ItemCart(session, status);
            list.Add(preview);
            return list;
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
