using SingleExperience.Entities.DB;
using SingleExperience.Entities.CartEntities;
using SingleExperience.Enums;
using SingleExperience.Services.CartServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ClientServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;

namespace SingleExperience.Views
{
    class ClientCartView
    {
        private CartService cartService = new CartService();
        private ProductDB productDB = new ProductDB();
        private CartDB cartDB = new CartDB();


        public void ListCart(SessionModel parameters)
        {
            Console.Clear();
            var total = cartService.TotalCart(parameters);
            var j = 41;

            Console.WriteLine($"\nInício > Carrinho\n");

            var itens = cartService.ItemCart(parameters, StatusProductEnum.Ativo);


            itens.ForEach(p =>
            {
                var product = productDB.ListProducts().FirstOrDefault(i => i.ProductId == p.ProductId);

                if (p.StatusId == Convert.ToInt32(StatusProductEnum.Ativo))
                {
                    Console.WriteLine($"+{new string('-', j)}+");
                    Console.WriteLine($"|#{p.ProductId}{new string(' ', j - 1 - p.ProductId.ToString().Length)}|");
                    Console.WriteLine($"|{product.Name}{new string(' ', j - product.Name.Length)}|");
                    Console.WriteLine($"|Qtd: {p.Amount}{new string(' ', j - 5 - p.Amount.ToString().Length)}|");
                    Console.WriteLine($"|R${product.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - product.Price.ToString().Length)}|");
                    Console.WriteLine($"+{new string('-', j)}+");
                }
            });

            Console.WriteLine($"\nSubtotal ({total.TotalAmount} itens): R${total.TotalPrice.ToString("F2")}");
            Menu(itens, parameters);
        }

        public void Menu(List<ProductCartModel> list, SessionModel parameters)
        {
            ClientProductCategoryView productCategory = new ClientProductCategoryView();
            ClientSendingAddressView address = new ClientSendingAddressView();
            ClientHomeView inicio = new ClientHomeView();
            ClientService client = new ClientService();
            ClientSignUpView signUp = new ClientSignUpView();
            ClientSignInView signIn = new ClientSignInView();

            var op = 0;
            var code = 0;
            var invalid = true;
            var invalidCode = true;
            var invalidCodeRemove = true;
            var total = cartService.TotalCart(parameters);
            var category = cartService.ItemCart(parameters, StatusProductEnum.Ativo)
                .Select(p => p.CategoryId)
                .FirstOrDefault();

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Buscar por categoria");

            if (parameters.CountProduct > 0)
            {
                Console.WriteLine($"2. Voltar para a categoria: {(CategoryProductEnum)category}");
                Console.WriteLine("3. Adicionar o produto mais uma vez ao carrinho");
                Console.WriteLine("4. Excluir um produto");
                Console.WriteLine("5. Finalizar compra");
            }

            if (parameters.Session.Length < 11)
            {
                Console.WriteLine("6. Fazer Login");
                Console.WriteLine("7. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("6. Desconectar-se");
            }
            Console.WriteLine("9. Sair do Sistema");
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
                case 0:
                    inicio.ListProducts(parameters);
                    break;
                case 1:
                    inicio.Search(parameters);
                    break;
                case 2:
                    productCategory.Category(category, parameters);
                    break;
                case 3:
                    Console.Write("\nPor favor digite o código do produto #");
                    while (invalidCode)
                    {
                        try
                        {
                            code = int.Parse(Console.ReadLine());
                            invalidCode = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                        }
                    }
                    list.ForEach(p =>
                    {
                        if (p.ProductId == code)
                        {
                            CartModel cartModel = new CartModel();
                            cartModel.ProductId = p.ProductId;
                            cartModel.UserId = parameters.Session;
                            cartModel.Name = p.Name;
                            cartModel.CategoryId = p.CategoryId;
                            cartModel.StatusId = Convert.ToInt32(StatusProductEnum.Ativo);
                            cartModel.Price = p.Price;

                            if (parameters.Session.Length < 11)
                            {
                                parameters.CartMemory = cartDB.AddItensMemory(cartModel, parameters.CartMemory);
                            }
                            else
                            {
                                cartDB.AddItemCart(parameters, cartModel);
                                parameters.CartMemory = new List<ItemEntitie>();
                            }
                        }
                    });

                    parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;

                    Console.WriteLine("\n\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();

                    ListCart(parameters);
                    break;
                case 4:
                    Console.Write("\nPor favor digite o codigo do produto #");
                    while (invalidCodeRemove)
                    {
                        try
                        {
                            if (parameters.CountProduct == 0)
                            {
                                Console.WriteLine("Carrinho vazio. Não é mais possível remover um item (Aperte enter para continuar)");
                                Console.ReadKey();
                                ListCart(parameters);
                            }
                            else
                            {
                                int codeRemove = int.Parse(Console.ReadLine());
                                cartService.RemoveItem(codeRemove, parameters.Session, parameters);
                                parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                                Console.WriteLine("\n\nProduto removido com sucesso (Aperte enter para continuar)");
                                Console.ReadKey();
                                invalidCodeRemove = false;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Opção inválida, tente novamente.");
                        }
                    }


                    ListCart(parameters);
                    break;
                case 5:
                    if (parameters.Session.Length < 11)
                    {
                        Console.WriteLine("\nVocê não está logado!!\n");
                        Console.WriteLine("1. Fazer login");
                        Console.WriteLine("2. Cadastrar-se");
                        Console.WriteLine("3. Cancelar");
                        Console.WriteLine("9. Sair do Sistema");
                        while (true)
                        {
                            try
                            {
                                int opc = int.Parse(Console.ReadLine());
                                switch (opc)
                                {
                                    case 1:
                                        signIn.Login(parameters, false);
                                        break;
                                    case 2:
                                        signUp.SignUp(parameters, false);
                                        break;
                                    case 3:
                                        ListCart(parameters);
                                        break;
                                    case 9:
                                        Environment.Exit(0);
                                        break;
                                    default:
                                        Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                                        Console.ReadKey();
                                        Menu(list, parameters);
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Opção inválida, tente novamente.");
                            }
                        }
                    }
                    else
                    {
                        address.Address(parameters);
                    }
                    break;
                case 6:
                    if (parameters.Session.Length == 11)
                    {
                        parameters.Session = client.SignOut();
                        parameters.CountProduct = cartService.TotalCart(parameters).TotalAmount;
                        ListCart(parameters);
                    }
                    else
                    {
                        parameters.CountProduct = total.TotalAmount;
                        signIn.Login(parameters, true);
                    }
                    break;
                case 7:
                    parameters.CountProduct = total.TotalAmount;
                    signUp.SignUp(parameters, true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Essa opção não existe. Tente novamente. (Tecle enter para continuar)");
                    Console.ReadKey();
                    Menu(list, parameters);
                    break;
            }
        }
    }
}
