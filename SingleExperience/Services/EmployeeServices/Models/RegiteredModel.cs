using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.EmployeeServices.Models
{
    class RegiteredModel
    {
        public string Cpf { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool AccessInventory { get; set; }
        public bool RegisterEmployee { get; set; }
    }
}
