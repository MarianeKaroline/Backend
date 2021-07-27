using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class ClientEntitie
    {
        public int ClientId { get; set; }
        public string FullName { get; set; }
        public int CPF { get; set; }
        public int Phone { get; set; }
        public string Password { get; set; }
        public int AddressId { get; set; }
    }
}
