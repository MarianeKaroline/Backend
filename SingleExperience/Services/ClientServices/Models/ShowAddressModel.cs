using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class ShowAddressModel
    {
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public int AddressId { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
