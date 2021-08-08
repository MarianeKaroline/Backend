using SingleExperience.Enums;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.ClientServices.Models;
using System;
using System.Globalization;
using SingleExperience.Entities.DB;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.CartServices;

namespace SingleExperience.Views
{
    class ClientPaymentMethodView
    {
        public int j = 41;
        private ClientPreviewBoughtView preview;
        private ClientService client;
        private ClientDB clientDB;
        private CardModel cardModel;

        public ClientPaymentMethodView()
        {
            preview = new ClientPreviewBoughtView();
            client = new ClientService();
            clientDB = new ClientDB();
            cardModel = new CardModel();
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
                    CreditCard(parameters, addressId, false);
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

        public void CreditCard(ParametersModel parameters, int addressId, bool home)
        {
            var op = "";
            char opc = '\0';
            var invalid = true;

            if (home)
            {
                Console.Clear();

                Console.WriteLine("\nSua conta > Seus cartões cadastrados\n");
            }

            if (client.HasCard(parameters.Session))
            {
                client.ShowCards(parameters.Session)
                    .ForEach(p => 
                    {
                        Console.WriteLine($"\n(Crédito) com final {p.CardNumber.Substring(12)}        {p.Name}        {p.ShelfLife.ToString("MM/yyyy")}\n");
                    });

                if (home)
                {
                    Console.Write("Adicionar um novo cartão? (s/n) \n");
                    while (invalid)
                    {
                        try
                        {
                            opc = char.Parse(Console.ReadLine().ToLower());
                            invalid = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\nOpção inválida, tente novamente.\n");
                        }
                    }

                    switch (opc)
                    {
                        case 's':
                            AddNewCreditCard(parameters, addressId, home);
                            break;
                        case 'n':
                            Menu(parameters);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.Write("Escolher um desses cartões: (s/n) ");
                    while (invalid)
                    {
                        try
                        {
                            opc = char.Parse(Console.ReadLine().ToLower());
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
                            AddNewCreditCard(parameters, addressId, false);
                            break;
                        default:
                            Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                            Console.ReadKey();
                            CreditCard(parameters, addressId, home);
                            break;
                    }
                }
            }
            else
            {
                AddNewCreditCard(parameters, addressId, false);
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

        //Caso a edição do cartão seja do perfil, o home tem que ser true
        public void AddNewCreditCard(ParametersModel parameters, int addressId, bool home)
        {
            Console.WriteLine($"\n+{new string('-', j)}+\n");
            Console.Write("Novo Cartão\n");
            Console.Write("Número do cartão: ");
            cardModel.CardNumber = long.Parse(Console.ReadLine());
            Console.Write("Nome no cartão: ");
            cardModel.Name = Console.ReadLine();
            Console.Write("Data de expiração(01/2021): ");
            cardModel.ShelfLife = DateTime.ParseExact(Console.ReadLine(), "MM/yyyy", CultureInfo.InvariantCulture);
            Console.Write("Código de segurança(CVV): ");
            cardModel.CVV = int.Parse(Console.ReadLine());

            clientDB.AddCard(parameters.Session, cardModel);

            if (home)
            {
                CreditCard(parameters, addressId, home);
            }
            else
            {
                preview.Bought(parameters, PaymentMethodEnum.CreditCard, cardModel.CardNumber.ToString(), addressId);
            }
        }

        public void Menu(ParametersModel parameters)
        {
            var home = new ClientHomeView();
            var cartService = new CartService();
            var client = new ClientService();
            var cart = new ClientCartView();
            var opc = 0;
            var invalid = true;

            Console.WriteLine("\n0. Precisa de ajuda?");
            Console.WriteLine("1. Voltar para o início");
            Console.WriteLine($"2. Ver Carrinho (Quantidade: {parameters.CountProduct})");
            Console.WriteLine("3. Desconectar-se");
            Console.WriteLine("9. Sair do Sistema");
            while (invalid)
            {
                try
                {
                    opc = int.Parse(Console.ReadLine());
                    invalid = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Opção inválida, tente novamente.");
                }
            }

            switch (opc)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\nInício > Assistência\n");
                    Console.WriteLine("Esta com problema com seu produto?");
                    Console.WriteLine("Contate-nos: ");
                    Console.WriteLine("Telefone: (41) 1234-5678");
                    Console.WriteLine("Email: exemplo@email.com");
                    Console.WriteLine("Tecle enter para continuar");
                    Console.ReadLine();

                    home.ListProducts(parameters);
                    break;
                case 1:
                    home.ListProducts(parameters);
                    break;
                case 2:
                    cart.ListCart(parameters);
                    break;
                case 3:
                    parameters.Session = client.SignOut();
                    parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                    home.ListProducts(parameters);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(parameters);
                    break;
            }
        }
    }
}
