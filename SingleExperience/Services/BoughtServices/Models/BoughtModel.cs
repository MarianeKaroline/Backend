using SingleExperience.Enums;
using SingleExperience.Services.CartServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.BoughtServices.Models
{
    class BoughtModel
    {
        public int BoughtId { get; set; }
        public string ClientName { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }        
        public PaymentMethodEnum paymentMethod { get; set; }        
        public string NumberCard { get; set; }
        public string Code { get; set; }
        public string Pix { get; set; }
        public List<ProductBought> Itens { get; set; }
        public double TotalPrice { get; set; }
        public DateTime DateBought { get; set; }
    }
}
