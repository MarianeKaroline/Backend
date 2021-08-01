﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SingleExperience.Entities.CartEntities;
using SingleExperience.Entities;
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
                    .ToList()
                    .ForEach(j =>
                    {
                        if (j.UserId == session)
                        {
                            if (j.StatusId == Convert.ToInt32(status))
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
            var itens = ItemCart(session, StatusProductEnum.Ativo);
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
            var listItens = cartDB.ListItens();
            var sum = 1;

            listItens.ForEach(p => 
            {
                if (p.Amount > 1)
                {
                    sum -= p.Amount;
                    cartDB.EditAmount(productId, session, sum);
                }
                else
                {
                    cartDB.EditStatusProduct(productId, session, StatusProductEnum.Inativo);
                }
            });            
        }

        //Ver produto antes de comprar
        public List<PreviewBoughtModel> PreviewBoughts(string session, MethodPaymentEnum method, string lastNumbers, StatusProductEnum status)
        {
            var list = new List<PreviewBoughtModel>();
            var preview = new PreviewBoughtModel();
            var client = clientDB.ListClient();
            var address = clientDB.ListAddress();
            var card = clientDB.ListCard();
            var itens = cartDB.ListItens();
            int aux = 0;

            //Pega alguns atributos do cliente
            client
                .Where(i => i.Cpf == session)
                .ToList()
                .ForEach(i =>
                {
                    aux = i.AddressId;
                    preview.FullName = i.FullName;
                    preview.Phone = i.Phone;
                });

            //Pega alguns atributos do endereço
            address
                .Where(i => i.AddressId == aux)
                .ToList()
                .ForEach(i =>
                {
                    preview.Cep = i.Cep;
                    preview.Street = i.Street;
                    preview.Number = i.Number;
                    preview.City = i.City;
                    preview.State = i.State;
                });

            preview.Method = method;
            if (Convert.ToInt32(method) == 1) //Só ira adicionar o número do cartão se o método for cartão
            {
                card
                    .Where(i => i.CardNumber.ToString().Substring(12, i.CardNumber.ToString().Length - 12) == lastNumbers && i.ClientId == session)
                    .ToList()
                    .ForEach(i =>
                    {
                        preview.NumberCard = i.CardNumber.ToString();
                    });
            }

            preview.Itens = ItemCart(session, status); //Arrumar aqui
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
