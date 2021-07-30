using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientsEntities
{
    class CardClientEntitie
    {
        public int CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public int CVV { get; set; }
        public long ClientId { get; set; }
    }
}
