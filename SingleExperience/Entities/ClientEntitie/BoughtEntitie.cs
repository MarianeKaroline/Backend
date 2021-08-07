using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class BoughtEntitie
    {
        public int BoughtId { get; set; }
        public int ProductId { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        public string NumberPayment { get; set; }
        public DateTime DateBought { get; set; }
    }
}
