using SingleExperience.Entities.DB;
using SingleExperience.Entities.EmployesEntities;
using SingleExperience.Entities.Enums;
using SingleExperience.Enums;
using SingleExperience.Services.BoughtServices.Models;
using SingleExperience.Services.EmployeeServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleExperience.Services.EmployeeServices
{
    class EmployeeService : EnjoyerDB
    {
        private BoughtDB boughtDB = new BoughtDB();
        private CartDB cartDB = new CartDB();
        private ClientDB clientDB = new ClientDB();
        private ProductDB productDB = new ProductDB();
        private EmployeeDB employeeDB = new EmployeeDB();

        public List<BoughtModel> Bought()
        {
            var listProducts = new List<BoughtModel>();
            var listBought = boughtDB.ListAll();

            listBought.ForEach(i =>
            {
                var client = clientDB.GetEnjoyer(i.Cpf);
                var address = clientDB.ListAddress(i.Cpf);
                var card = clientDB.ListCard(i.Cpf);
                var cart = cartDB.GetCart(i.Cpf);
                var itens = cartDB.ListItens(cart.CartId);
                var boughtModel = new BoughtModel();
                boughtModel.Itens = new List<ProductBoughtModel>();

                boughtModel.ClientName = client.FullName;
                var aux = address
                .FirstOrDefault(j => j.AddressId == i.AddressId);

                boughtModel.Cep = aux.Cep;
                boughtModel.Street = aux.Street;
                boughtModel.Number = aux.Number;
                boughtModel.City = aux.City;
                boughtModel.State = aux.State;

                boughtModel.BoughtId = i.BoughtId;
                boughtModel.paymentMethod = (PaymentMethodEnum)i.PaymentId;

                if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.CreditCard))
                {
                    card
                    .Where(j => j.CardNumber.ToString().Contains(i.CodeBought))
                    .ToList()
                    .ForEach(k =>
                    {
                        boughtModel.NumberCard = k.CardNumber.ToString();
                    });
                }
                else if (i.PaymentId == Convert.ToInt32(PaymentMethodEnum.BankSlip))
                {
                    boughtModel.Code = i.CodeBought;
                }
                else
                {
                    boughtModel.Pix = i.CodeBought;
                }
                boughtModel.TotalPrice = i.TotalPrice;
                boughtModel.DateBought = i.DateBought;
                boughtModel.StatusId = i.StatusId;

                boughtDB.ListProductBought(i.BoughtId)
                .ToList()
                .ForEach(j =>
                {
                    var product = new ProductBoughtModel();

                    product.ProductId = j.ProductId;
                    product.ProductName = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Name;
                    product.Amount = j.Amount;
                    product.Price = productDB.ListProducts().FirstOrDefault(i => i.ProductId == j.ProductId).Price;
                    product.BoughtId = j.BoughtId;

                    boughtModel.Itens.Add(product);
                });

                listProducts.Add(boughtModel);
            });

            return listProducts;
        }

        public List<BoughtModel> BoughtPendent(StatusBoughtEnum status)
        {
            return Bought().Where(i => i.StatusId == Convert.ToInt32(status)).ToList();
        }

        public List<RegiteredModel> listEmployee()
        {
            return employeeDB.List()
                     .Select(i => new RegiteredModel()
                     {
                         Cpf = i.Cpf,
                         FullName = i.FullName,
                         Email = i.Email,
                         AccessInventory = employeeDB.Access(i.Cpf).AccessInventory,
                         RegisterEmployee = employeeDB.Access(i.Cpf).AccessRegister
                     })
                     .ToList();
        }
    }
}
