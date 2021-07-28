using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SingleExperience.Services.ClientServices
{
    class ClientService
    {
        public string ClientId()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipComputer = null;

            //Pega o endereço do Computador
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipComputer = ip.ToString();
                }
            }
            return ipComputer;
        }
    }
}
