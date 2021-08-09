using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class ProductBoughtEntitie
    {
        public int ProductBoughtId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int BoughtId { get; set; }
    }
}
