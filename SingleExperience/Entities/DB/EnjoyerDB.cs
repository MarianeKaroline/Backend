using SingleExperience.Entities.ClientEntities;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace SingleExperience.Entities.DB
{
    class EnjoyerDB
    {
        private string[] listEnjoyer = File.ReadAllLines(@"C:\Users\mariane.santos\Desktop\Backend\SingleExperience\Database\Enjoyer.csv", Encoding.UTF8);

        public string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string session = "";

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    session = ip.ToString().Replace(".", "");
                }
            }
            return session;
        }

        public EnjoyerEntitie SignIn(SignInModel signIn)
        {
            var client = GetEnjoyer(signIn.Email);
            EnjoyerEntitie session = null;

            if (client != null)
            {
                if (client.Password == signIn.Password)
                {
                    session = client;
                }
            }

            return session;
        }

        //Sair
        public string SignOut()
        {
            return GetIP();
        }

        //Lista todos os users cadastrados no sistema
        public List<EnjoyerEntitie> ListEnjoyer()
        {
            listEnjoyer = File.ReadAllLines(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Enjoyer.csv", Encoding.UTF8);

            return listEnjoyer
                .Skip(1)
                .Select(i => new EnjoyerEntitie
                {
                    Cpf = i.Split(',')[0],
                    FullName = i.Split(',')[1],
                    Phone = i.Split(',')[2],
                    Email = i.Split(',')[3],
                    BirthDate = DateTime.Parse(i.Split(',')[4]),
                    Password = i.Split(',')[5],
                    Employee = bool.Parse(i.Split(',')[6])
                })
                .ToList();
        }

        public EnjoyerEntitie GetEnjoyer(string cpf)
        {
            return ListEnjoyer()
                .FirstOrDefault(i => i.Cpf == cpf || i.Email == cpf);
        }

        public void SignUp(SignUpModel enjoyer)
        {
            try
            {
                var aux = new string[]
                {
                    enjoyer.Cpf,
                    enjoyer.FullName.ToString(),
                    enjoyer.Phone.ToString(),
                    enjoyer.Email.ToString(),
                    enjoyer.BirthDate.ToString(),
                    enjoyer.Password.ToString(),
                    enjoyer.Employee.ToString(),
                };


                using (StreamWriter sw = File.AppendText(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"..\..\..\..\\Database\Enjoyer.csv"))
                {
                    sw.WriteLine(String.Join(",", aux));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }

        }
    }
}
