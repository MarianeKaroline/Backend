﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ClientEntities
{
    class AddressClientEntitie
    {
        public int AddressId { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
