using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.EmployesEntities
{
    class EmployeeEntitie
    {
        public int Cpf { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AccessInventory { get; set; }
        public bool RegisterEmployee { get; set; }
    }
}
