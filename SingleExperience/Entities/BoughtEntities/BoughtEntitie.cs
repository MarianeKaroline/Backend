using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.BoughtEntities
{
    class BoughtEntitie
    {
        public int BoughtId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}
