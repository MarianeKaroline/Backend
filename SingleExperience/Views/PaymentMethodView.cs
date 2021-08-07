using SingleExperience.Enums;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices.Models;

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

        public void Methods(ParametersModel parameters, int addressId)
        {
            var op = 0;
            var invalid = true;
            Console.Clear();

            Console.WriteLine("\nCarrinho > Informações pessoais > Endereço > Método de pagamento\n");

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
                    CreditCard(parameters, addressId);
                    break;
                case 2:
                    BankSlip(parameters, addressId);
                    break;
                case 3:
                    Pix(parameters, addressId);
                    break;
                default:
                    break;
            }
        }

        public void CreditCard(ParametersModel parameters, int addressId)
        {
            CardModel card = new CardModel();
            var op = "";
            char opc = '\0';
            var invalid = true;
            if (client.HasCard(parameters.Session))
            {
                client.ShowCards(parameters.Session)
                    .ForEach(p => 
                    {
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

                        preview.Bought(parameters, PaymentMethodEnum.CreditCard, op, addressId);
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

                        clientDB.AddCard(parameters.Session, card);
                        preview.Bought(parameters, PaymentMethodEnum.CreditCard, card.CardNumber.ToString().Substring(12, card.CardNumber.ToString().Length - 12), addressId);
                        break;
                    default:
                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                        Console.ReadKey();
                        CreditCard(parameters, addressId);
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

                clientDB.AddCard(parameters.Session, card);
                preview.Bought(parameters, PaymentMethodEnum.CreditCard, card.CardNumber.ToString(), addressId);
            }
        }

        public void BankSlip(ParametersModel parameters, int addressId)
        {
            var numberCode = Guid.NewGuid();

            preview.Bought(parameters, PaymentMethodEnum.BankSlip, numberCode.ToString(), addressId);
        }

        public void Pix(ParametersModel parameters, int addressId)
        {           
            var numberPix = Guid.NewGuid();

            preview.Bought(parameters, PaymentMethodEnum.Pix, numberPix.ToString(), addressId);
        }
    }
}
