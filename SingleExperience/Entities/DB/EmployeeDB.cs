using SingleExperience.Entities.EmployesEntities;
using SingleExperience.Services.EmployeeServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class EmployeeDB
    {
        private string CurrentDirectory = null;
        private string path = null;

        public EmployeeDB()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Employee.csv";
        }

        public string[] EmployeeList()
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        //Lista todos os funcionário cadastrados no sistema
        public List<EmployeeEntitie> List()
        {
            var employee = new List<EmployeeEntitie>();
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    employee = EmployeeList()
                        .Skip(1)
                        .Select(i => new EmployeeEntitie
                        {
                            Cpf = i.Split(',')[0],
                            FullName = i.Split(',')[1],
                            Email = i.Split(',')[2],
                            Password = i.Split(',')[3],
                            AccessInventory = bool.Parse(i.Split(',')[4]),
                            RegisterEmployee = bool.Parse(i.Split(',')[5]),
                        })
                        .ToList();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return employee;
        }

        //Pega apenas um funcionário pelo cpf
        public EmployeeEntitie GetEmployee(string cpf)
        {
            var employee = new EmployeeEntitie();
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    employee = EmployeeList()
                        .Skip(1)
                        .Select(i => new EmployeeEntitie
                        {
                            Cpf = i.Split(',')[0],
                            FullName = i.Split(',')[1],
                            Email = i.Split(',')[2],
                            Password = i.Split(',')[3],
                            AccessInventory = bool.Parse(i.Split(',')[4]),
                            RegisterEmployee = bool.Parse(i.Split(',')[5]),
                        })
                        .FirstOrDefault(i => i.Cpf == cpf || i.Email == cpf);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return employee;
        }

        //Cadastra funcionário
        public bool Register(SignUpEmployeeModel employee)
        {
            var existClient = GetEmployee(employee.Cpf);
            var signUp = false;

            try
            {
                var lines = new List<string>();

                if (existClient == null)
                {
                    var aux = new string[]
                    {
                        employee.Cpf,
                        employee.FullName.ToString(),
                        employee.Email.ToString(),
                        employee.Password.ToString(),
                        employee.AccessInventory.ToString(),
                        employee.RegisterEmployee.ToString()
                    };

                    lines.Add(String.Join(",", aux));


                    using (StreamWriter sw = File.AppendText(path))
                    {
                        lines.ForEach(p =>
                        {
                            sw.WriteLine(p);
                        });
                    }

                    signUp = true;
                }
                else
                {
                    signUp = false;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

            return signUp;
        }
    }
}
