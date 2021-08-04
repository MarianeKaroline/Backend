using SingleExperience.Services.ProductServices;
using SingleExperience.Services.CartServices;
using SingleExperience.Entities.ProductEntities.Enums;
using System;
using System.Globalization;
using SingleExperience.Services.ClientServices;
using SingleExperience.Services.CartServices.Models;
using SingleExperience.Services.ProductServices.Models.ProductModels;
using SingleExperience.Entities.ProductEntities.DB;

namespace SingleExperience.Views
{
    class SelectedProductView
    {
        private ProductService productService = null;
        public SelectedProductView()
        {
            productService = new ProductService();
        }

        //Listar Produtos
        public void SelectedProduct(int productId, int countProduct, string session)
        {
            Console.Clear();
            var product = productService.SelectedProduct(productId);
            var j = 41;
            var category = (CategoryProductEnum)product.CategoryId;

            Console.WriteLine($"\nInício > Pesquisa > {category} > {product.Name}\n");

            var aux = product.Detail.Split(';');

            Console.WriteLine($"+{new string('-', j)}+");
            Console.WriteLine($"|#{product.ProductId}{new string(' ', j - 1 - product.ProductId.ToString().Length)}|");
            Console.WriteLine($"|*{product.Rating.ToString("F1", CultureInfo.InvariantCulture)}{new string(' ', j - 3 - product.Rating.ToString().Length)}|");
            Console.WriteLine($"|{product.Name}{new string(' ', j - product.Name.ToString().Length)}|");
            Console.WriteLine($"|R${product.Price.ToString("F2", CultureInfo.CurrentCulture)}{new string(' ', j - 5 - product.Price.ToString().Length)}|");
            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Detalhes{new string(' ', j - "Detalhes".Length)}|");

            for (int i = 0; i < aux.Length; i++)
            {
                Console.WriteLine($"|{aux[i]}{new string(' ', j - aux[i].Length)}|");
            }

            Console.WriteLine($"|{new string(' ', j)}|");
            Console.WriteLine($"|Quantidade em estoque: {product.Amount}{new string(' ', j - "Quantidade em estoque".Length - 2 - product.Amount.ToString().Length)}|");
            Console.WriteLine($"+{new string('-', j)}+");


            Menu(product, countProduct, session, productId);
        }

        //Mostra Menu
        public void Menu(ProductSelectedModel list, int countProduct, string session, int productId)
        {
            var signIn = new SignInView();
            var signUp = new SignUpView();
            var client = new ClientService();
            var categoryProduct = new ProductCategoryView();
            var inicio = new HomeView();
            var cartList = new CartView();
            var category = (CategoryProductEnum)list.CategoryId;
            var cart = new CartService();
            var cartDB = new CartDB();
            var op = 0;
            var invalid = true;

            Console.WriteLine("\n0. Início");
            Console.WriteLine("1. Pesquisar por categoria");
            Console.WriteLine($"2. Voltar para a categoria: {category}");
            Console.WriteLine("3. Adicionar produto ao carrinho");
            Console.WriteLine($"4. Ver Carrinho (Quantidade: {countProduct})");
            if (session.Length < 11)
            {
                Console.WriteLine("5. Fazer Login");
                Console.WriteLine("6. Cadastrar-se");
            }
            else
            {
                Console.WriteLine("5. Desconectar-se");
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
                    inicio.ListProducts(countProduct, session);
                    break;
                case 1:
                    inicio.Search(countProduct, session);
                    break;
                case 2:
                    categoryProduct.Category(list.CategoryId, countProduct, session);
                    break;
                case 3:
                    CartModel cartModel = new CartModel();
                    cartModel.ProductId = list.ProductId;
                    cartModel.UserId = session;
                    cartModel.Name = list.Name;
                    cartModel.CategoryId = list.CategoryId;
                    cartModel.StatusId = Convert.ToInt32(StatusProductEnum.Ativo);
                    cartModel.Price = list.Price;

                    cartDB.AddItensCart(cartModel);
                    var count = cart.TotalCart(session);

                    Console.WriteLine("\nProduto adicionado com sucesso (Aperte enter para continuar)");
                    Console.ReadKey();
                    SelectedProduct(list.ProductId, count.TotalAmount, session);
                    break;
                case 4:
                    cartList.ListCart(session);
                    break;
                case 5:
                    if (session.Length == 11)
                    {
                        var ip = client.SignOut();
                        SelectedProduct(productId, countProduct, ip);
                    }
                    else
                    {
                        signIn.Login(countProduct, session, true);
                    }
                    break;
                case 6:
                    signUp.SignUp(countProduct, true);
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
