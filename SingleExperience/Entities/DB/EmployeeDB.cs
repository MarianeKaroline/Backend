using SingleExperience.Entities.ClientEntities;
using SingleExperience.Entities.EmployesEntities;
using SingleExperience.Services.ClientServices.Models;
using SingleExperience.Services.EmployeeServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class EmployeeDB : EnjoyerDB
    {
        private string pathAccess = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\AccessEmployee.csv";

        //Lista todos os funcionário cadastrados no sistema
        public List<EnjoyerEntitie> List()
        {
            return ListEnjoyer()
                .Where(i => i.Employee == true)
                .ToList();
        }        

        //Lista o acesso do funcionário
        public AccessEmployeeEntitie Access(string cpf)
        {
            string[] listAccess = File.ReadAllLines(pathAccess, Encoding.UTF8);

            return listAccess
                .Skip(1)
                .Select(i => new AccessEmployeeEntitie
                {
                    Cpf = i.Split(',')[0],
                    AccessInventory = bool.Parse(i.Split(',')[1]),
                    AccessRegister = bool.Parse(i.Split(',')[2])
                })
                .FirstOrDefault(i => i.Cpf == cpf);
        }

        //Cadastra funcionário
        public bool Register(SignUpModel employee)
        {
            var existEmployee = GetEnjoyer(employee.Cpf);

            try
            {
                if (existEmployee == null)
                {
                    SignUp(employee);

                    var auxAccess = new string[]
                    {
                        employee.Cpf,
                        employee.AccessInventory.ToString(),
                        employee.AccessRegister.ToString()
                    };

                    using (StreamWriter sw = File.AppendText(pathAccess))
                    {
                        sw.WriteLine(String.Join(",", auxAccess));
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

            return existEmployee == null;
        }
    }
}
