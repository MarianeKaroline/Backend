using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class SignUpModel
    {
        public string Cpf { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public bool Employee { get; set; }
        public bool AccessInventory { get; set; }
        public bool AccessRegister { get; set; }
    }
}
