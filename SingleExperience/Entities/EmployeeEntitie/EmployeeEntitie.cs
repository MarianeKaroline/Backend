using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.ProductEntities.EmployesEntities
{
    class EmployeeEntitie
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public int Cpf { get; set; }
        public string Password { get; set; }
        public bool accessInventory { get; set; }
    }
}
