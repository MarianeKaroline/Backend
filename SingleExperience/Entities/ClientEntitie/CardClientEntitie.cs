using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientsEntities
{
    class CardClientEntitie
    {
        public long CardNumber { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public int CVV { get; set; }
        public string ClientId { get; set; }
    }
}
