using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class AddressClientEntitie
    {
        public int AddressId { get; set; }
        public int CEP { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
