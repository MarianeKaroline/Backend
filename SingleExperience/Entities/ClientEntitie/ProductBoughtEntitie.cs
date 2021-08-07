using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class ProductBoughtEntitie
    {
        public int ProductBoughtId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Amount { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }
        public string Cpf { get; set; }
        public int BoughtId { get; set; }
    }
}
