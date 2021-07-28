using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SingleExperience.Entities.CartEntities;
using System.Net;
using System.Net.Sockets;

namespace SingleExperience.Services.CartServices
{
    class CartService
    {
        private int CartId = 0;
        private string CurrentDirectory = null;
        private string path = null;

        public CartService()
        {
            CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = CurrentDirectory + @"..\..\..\..\\Database\Cart.csv";
        }

        //Lê arquivo csv carrinho
        public List<CartEntitie> ListCart()
        {
            var prodCart = new List<CartEntitie>();
            try
            {
                string[] products = File.ReadAllLines(path, Encoding.UTF8);
                using (StreamReader sr = File.OpenText(path))
                {
                    products
                        .Skip(1)
                        .ToList()
                        .ForEach(p =>
                        {
                            string[] fields = p.Split(',');

                            var itens = new CartEntitie();
                            itens.CartId = int.Parse(fields[0]);
                            itens.ProductId = int.Parse(fields[1]);
                            itens.DateCreated = DateTime.Parse(fields[2]);

                            prodCart.Add(itens);
                        });
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(e.Message);
            }
            return prodCart;
        }

        //Adiciona no carrinho
        public int AddCart(int productId)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipComputer;
            CartId++;
            var count = 0;

            //Pega o endereço do Computador
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipComputer = ip.ToString();
                    Console.WriteLine($"{ipComputer}{count}");
                    Console.ReadKey();
                }
            }

            if (File.Exists(path))
            {
                string lines = $"{CartId},{productId},{DateTime.Now}";

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(lines);
                }

                var products = ListCart();
                count = products
                            .Skip(1)
                            .Count();
            }
            return count;
        }
    }
}
