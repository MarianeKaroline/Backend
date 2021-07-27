using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Entities.EmployesEntities
{
    class EmployeeEntitie
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public int CPF { get; set; }
        public int BirthDate { get; set; }
        public int Phone { get; set; }
        public string Password { get; set; }
        public bool accessInventory { get; set; }
    }
}
