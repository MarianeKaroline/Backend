using SingleExperience.Entities.Enums;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
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
            var op = 0;
            var invalid = true;
            Console.Clear();

            Console.WriteLine("\nCarrinho > Informações pessoais > Método de pagamento\n");

            Console.WriteLine("1. Cartão de Crédito");
            Console.WriteLine("2. Boleto");
            Console.WriteLine("3. Pix");
            while (invalid)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (op)
            {
                case 1:
                    CreditCard(session);
                    break;
                case 2:
                    BankSlip(session);
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
            var op = "";
            char opc = '\0';
            var invalid = true;
            if (client.HasCard(session))
            {
                client.ShowCards(session)
                    .ForEach(p => 
                    {
                        Console.WriteLine("Número do cartão                   Nome no cartão                 Validade");
                        Console.WriteLine($"\n(Crédito) com final {p.CardNumber.Substring(12)}        {p.Name}        {p.ShelfLife.ToString("MM/yyyy")}\n");
                    });
                Console.Write("Escolher um desses cartões: (s/n) ");
                while (invalid)
                {
                    try
                    {
                        opc = char.Parse(Console.ReadLine());
                        invalid = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Opção inválida, tente novamente.");
                    }

                }

                switch (opc)
                {
                    case 's':
                        invalid = true;
                        Console.Write("\nDigite os últimos 4 números do cartão: ");
                        while (invalid)
                        {
                            try
                            {
                                op = Console.ReadLine();
                                invalid = false;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Opção inválida, tente novamente.");
                            }

                        }

                        preview.Bought(session, PaymentMethodEnum.CreditCard, op);
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
                        preview.Bought(session, PaymentMethodEnum.CreditCard, card.CardNumber.ToString().Substring(12, card.CardNumber.ToString().Length - 12));
                        break;
                    default:
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        CreditCard(session);
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
                preview.Bought(session, PaymentMethodEnum.CreditCard, card.CardNumber.ToString());
            }
        }

        public void BankSlip(string session)
        {
            var rand = new Random();
            var numberCode = rand.Next(100000000, 200000000);

            preview.Bought(session, PaymentMethodEnum.BankSlip, numberCode.ToString());
        }

        public void Pix(string session)
        {
            string opc = "";
            var op = 0;
            var invalid = true;

            Console.WriteLine($"\n+{new string('-', j)}+\n");
            Console.WriteLine("Escolha uma chave");
            Console.WriteLine("1. CPF");
            Console.WriteLine("2. Celular");
            Console.WriteLine("3. E-mail");
            Console.WriteLine("4. Chave aleatória");
            while (invalid)
            {
                try
                {
                    op = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }

            }

            switch (op)
            {
                case 1:
                    Console.Write("Digite o CPF: ");
                    opc = Console.ReadLine();
                    break;
                case 2:
                    Console.Write("Digite o número de celular: ");
                    opc = Console.ReadLine();
                    break;
                case 3:
                    Console.Write("Digite o e-mail: ");
                    opc = Console.ReadLine();
                    break;
                case 4:
                    Console.Write("Digite o chave aleatória: ");
                    opc = Console.ReadLine();
                    break;
                default:
                    break;
            }

            preview.Bought(session, PaymentMethodEnum.Pix, opc);
        }
    }
}
