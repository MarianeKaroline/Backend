using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class BoughtEntitie
    {
        public int BoughtId { get; set; }        
        public double TotalPrice { get; set; }
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        public string CodeBought { get; set; }
        public string Cpf { get; set; }
        public DateTime DateBought { get; set; }
    }
}
