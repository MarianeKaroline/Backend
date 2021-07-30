using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Views
{
    class PaymentMethodView
    {
        public int j = 41;
        public void Methods(long session)
        {
            Console.Clear();

            Console.WriteLine("\nCarrinho > Informações pessoais > Método de pagamento\n");

            Console.WriteLine("1. Cartão de Crédito");
            Console.WriteLine("2. Boleto");
            Console.WriteLine("3. Pix");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    CreditCard(session);
                    break;
                case 2:
                    Bullet();
                    break;
                case 3:

                    break;
                default:
                    break;
            }
        }

        public void CreditCard(long session)
        {
            ClientService client = new ClientService();
            CardModel card = new CardModel();

            Console.WriteLine($"\n+{new string('-', j)}+\n");
            Console.Write("Número do cartão: ");
            card.CardNumber = long.Parse(Console.ReadLine());
            Console.Write("Nome no cartão: ");
            card.Name = Console.ReadLine();
            Console.Write("Data de expiração(01/2021): ");
            card.ShelfLife = DateTime.Parse(Console.ReadLine());
            Console.Write("Código de segurança(CVV): ");
            card.CVV = int.Parse(Console.ReadLine());

            client.AddCard(session, card);
        }

        public void Bullet()
        {

        }

        public void Pix()
        {
            Console.WriteLine($"\n+{new string('-', j)}+\n");
            Console.WriteLine("Escolha uma chave");
            Console.WriteLine("1. CPF");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. E-mail");
            Console.WriteLine("4. Chave aleatória");
            int op = int.Parse(Console.ReadLine());

            switch (op)
            {
                case 1:
                    Console.Write("Digite o CPF: ");

                    break;
                case 2:
                    Console.Write("Digite o número de celular: ");

                    break;
                case 3:
                    Console.Write("Digite o e-mail: ");

                    break;
                case 4:
                    Console.Write("Digite o chave aleatória: ");

                    break;
                default:
                    break;
            }
        }
    }
}
