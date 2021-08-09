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
    class EmployeeService
    {
        private BoughtDB boughtDB;
        private CartDB cartDB;
        private ClientDB clientDB;
        private EmployeeDB employeeDB;

        public EmployeeService()
        {
            boughtDB = new BoughtDB();
            cartDB = new CartDB();
            clientDB = new ClientDB();
            employeeDB = new EmployeeDB();
        }

        public List<BoughtModel> Bought()
        {
            var listProducts = new List<BoughtModel>();
            var listBought = boughtDB.ListAll();

            listBought.ForEach(i =>
            {
                var client = clientDB.GetClient(i.Cpf);
                var address = clientDB.ListAddress(i.Cpf);
                var card = clientDB.ListCard(i.Cpf);
                var cart = cartDB.GetCart(i.Cpf);
                var itens = cartDB.ListItens(cart.CartId);
                var boughtModel = new BoughtModel();
                boughtModel.Itens = new List<ProductBought>();

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
                    var product = new ProductBought();

                    product.ProductId = j.ProductId;
                    product.ProductName = j.Name;
                    product.CategoryId = j.CategoryId;
                    product.Amount = j.Amount;
                    product.StatusId = j.StatusId;
                    product.Price = j.Price;
                    product.BoughtId = j.BoughtId;

                    boughtModel.Itens.Add(product);
                });

                listProducts.Add(boughtModel);
            });

            return listProducts;
        }

        //Login
        public string SignIn(SignInEmployeeModel signIn)
        {
            var numberSession = Guid.NewGuid();
            var employee = employeeDB.GetEmployee(signIn.Email);
            string session = "";

            if (employee != null)
            {
                if (employee.Password == signIn.Password)
                {
                    session = numberSession + employee.Cpf;
                }
            }

            return session;
        }

        public string SignOut()
        {
            return clientDB.ClientId();
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
                        AccessInventory = i.AccessInventory,
                        RegisterEmployee = i.RegisterEmployee
                    })
                    .ToList();
        }
    }
}
