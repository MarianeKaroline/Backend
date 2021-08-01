using SingleExperience.Entities.Enums;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using SingleExperience.Entities.DB;

namespace SingleExperience.Views
{
    class PaymentMethodView
    {
        public int j = 41;
        private PreviewBoughtView preview;
        private ClientService client;
        private ClientDB clientDB;

        public PaymentMethodView()
        {
            preview = new PreviewBoughtView();
            client = new ClientService();
            clientDB = new ClientDB();
        }

        public void Methods(string session)
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
                    Bullet(session);
                    break;
                case 3:
                    Pix(session);
                    break;
                default:
                    break;
            }
        }

        public void CreditCard(string session)
        {
            CardModel card = new CardModel();
            if (client.HasCard(session))
            {
                client.ShowCards(session)
                    .ForEach(p => 
                    {
                        Console.WriteLine($"\n(Crédito) com final {p.CardNumber.Substring(12, p.CardNumber.Length - 12)}        {p.Name}        {p.ShelfLife}\n");
                    });
                Console.Write("Escolher um desses cartões: (s/n) ");
                char opc = char.Parse(Console.ReadLine().ToLower());

                switch (opc)
                {
                    case 's':
                        Console.Write("Digite os últimos 4 números do cartão: ");
                        string op = Console.ReadLine();

                        preview.Bought(session, MethodPaymentEnum.CreditCard, op);
                        break;
                    case 'n':
                        Console.Write("Novo Cartão\n");
                        Console.WriteLine($"\n+{new string('-', j)}+\n");
                        Console.Write("Número do cartão: ");
                        card.CardNumber = long.Parse(Console.ReadLine());
                        Console.Write("Nome no cartão: ");
                        card.Name = Console.ReadLine();
                        Console.Write("Data de expiração(01/2021): ");
                        card.ShelfLife = DateTime.ParseExact(Console.ReadLine(), "MM/yyyy", CultureInfo.InvariantCulture);
                        Console.Write("Código de segurança(CVV): ");
                        card.CVV = int.Parse(Console.ReadLine());

                        clientDB.AddCard(session, card);
                        preview.Bought(session, MethodPaymentEnum.CreditCard, card.CardNumber.ToString().Substring(12, card.CardNumber.ToString().Length - 12));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine($"\n+{new string('-', j)}+\n");
                Console.Write("Número do cartão: ");
                card.CardNumber = long.Parse(Console.ReadLine());
                Console.Write("Nome no cartão: ");
                card.Name = Console.ReadLine();
                Console.Write("Data de expiração(01/2021): ");
                card.ShelfLife = DateTime.ParseExact(Console.ReadLine(), "MM/yyyy", CultureInfo.InvariantCulture);
                Console.Write("Código de segurança(CVV): ");
                card.CVV = int.Parse(Console.ReadLine());

                clientDB.AddCard(session, card);
                preview.Bought(session, MethodPaymentEnum.CreditCard, "");
            }
        }

        public void Bullet(string session)
        {
            preview.Bought(session, MethodPaymentEnum.Bullet, "");
        }

        public void Pix(string session)
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

            preview.Bought(session, MethodPaymentEnum.Pix, "");
        }
    }
}
