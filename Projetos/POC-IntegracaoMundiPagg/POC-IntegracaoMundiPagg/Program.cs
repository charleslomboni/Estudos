using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using GatewayApiClient;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GatewayApiClient.EnumTypes;

namespace POC_IntegracaoMundiPagg
{
    class Program
    {
        public static Guid MerchantKey { get; set; }
        public static Guid OrderKey { get; set; }

        public static void CreateTransaction()
        {
            // Cria a transação.
            var transaction = new CreditCardTransaction()
            {
                AmountInCents = 10000,
                CreditCard = new CreditCard()
                {
                    CreditCardBrand = CreditCardBrandEnum.Visa,
                    CreditCardNumber = "4111111111111111",
                    ExpMonth = 10,
                    ExpYear = 22,
                    HolderName = "Charles Lomboni - console",
                    SecurityCode = "123"
                },
                CreditCardOperation = CreditCardOperationEnum.AuthOnly,
                InstallmentCount = 1
            };

            // Cria requisição.
            var createSaleRequest = new CreateSaleRequest()
            {
                // Adiciona a transação na requisição.
                CreditCardTransactionCollection = new Collection<CreditCardTransaction>(new CreditCardTransaction[] { transaction }),
                Order = new Order()
                {
                    OrderReference = "NumeroDoPedido"
                }
            };

            // Creates the client that will send the transaction.
            var serviceClient = new GatewayServiceClient();

            // Autoriza a transação e recebe a resposta do gateway.
            var httpResponse = serviceClient.Sale.Create(createSaleRequest);

            Console.WriteLine("Código retorno: {0}", httpResponse.HttpStatusCode);
            Console.WriteLine("Chave do pedido: {0}", httpResponse.Response.OrderResult.OrderKey);
            if (httpResponse.Response.CreditCardTransactionResultCollection != null)
            {
                Console.WriteLine("Status da transação: {0}", httpResponse.Response.CreditCardTransactionResultCollection.FirstOrDefault().CreditCardTransactionStatus);
            }

            MerchantKey = serviceClient.Sale.MerchantKey;
            OrderKey = httpResponse.Response.OrderResult.OrderKey;
        }
        public static void CancelTransaction(Guid merchantKey, Guid orderKey)
        {
            // Creates the client that will send the transaction.
            var serviceClient = new GatewayServiceClient();

            // Cria o cliente para cancelar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri("https://sandbox.mundipaggone.com"));

            // Cancela as transações de cartão de crédito do pedido.
            var httpResponse = client.Sale.Manage(ManageOperationEnum.Cancel, orderKey);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK
                && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true))
            {
                Console.WriteLine("Transações canceladas.");
            }
        }
        public static void CaptureTransaction(Guid merchantKey, Guid orderKey)
        {
            // Cria o cliente para capturar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri("https://sandbox.mundipaggone.com"));

            // Captura as transações de cartão de crédito do pedido.
            var httpResponse = client.Sale.Manage(ManageOperationEnum.Capture, orderKey);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK
                && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true))
            {
                Console.WriteLine("Transações capturadas.");
            }
        }
        public static void RetryTransaction(Guid merchantKey, Guid orderKey)
        {
            // Cria o cliente para retentar as transações.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri("https://sandbox.mundipaggone.com"));

            // Retenta as transações não autorizadas de cartão de crédito no pedido.
            var httpResponse = client.Sale.Retry(orderKey);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK
                && httpResponse.Response.CreditCardTransactionResultCollection.Any()
                && httpResponse.Response.CreditCardTransactionResultCollection.All(p => p.Success == true))
            {
                Console.WriteLine("Transações autorizadas.");
            }
            else
            {
                Console.WriteLine("Uma ou mais transações não foram autorizadas.");
            }
        }
        public static void QueryTransaction(Guid merchantKey, Guid orderKey)
        {
            // Cria o cliente para consultar o pedido no gateway.
            IGatewayServiceClient client = new GatewayServiceClient(merchantKey, new Uri("https://sandbox.mundipaggone.com"));

            // Consulta o pedido.
            var httpResponse = client.Sale.QueryOrder(orderKey);

            if (httpResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                foreach (var sale in httpResponse.Response.SaleDataCollection)
                {
                    Console.WriteLine("Número do pedido: {0}", sale.OrderData.OrderReference);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "[ POC de Integração MundiPagg ] - Charles Lomboni";

            Console.WriteLine("================================================================================");
            Console.WriteLine("============================= SELECIONE A OPERAÇÃO =============================");
            Console.WriteLine("================================================================================");
            Console.WriteLine("1 - Criar Transação");
            Console.WriteLine("2 - Cancelar Transação");
            Console.WriteLine("3 - Capturar Transação");
            Console.WriteLine("4 - Retentar Transação");
            Console.WriteLine("5 - Consultar Transação");

            var key = Console.ReadKey();

            while (key.KeyChar != '0')
            {
                Console.Clear();

                switch (key.KeyChar)
                {
                    case '1':
                        CreateTransaction();
                        break;

                    case '2':
                        CancelTransaction(MerchantKey, OrderKey);
                        break;

                    case '3':
                        CaptureTransaction(MerchantKey, OrderKey);
                        break;

                    case '4':
                        RetryTransaction(MerchantKey, OrderKey);
                        break;

                    case '5':
                        QueryTransaction(MerchantKey, OrderKey);
                        break;

                    default:
                        Environment.Exit(0);
                        break;
                }

                Console.WriteLine("================================================================================");
                Console.WriteLine("============================= SELECIONE A OPERAÇÃO =============================");
                Console.WriteLine("================================================================================");
                Console.WriteLine("1 - Criar Transação");
                Console.WriteLine("2 - Cancelar Transação");
                Console.WriteLine("3 - Capturar Transação");
                Console.WriteLine("4 - Retentar Transação");
                Console.WriteLine("5 - Consultar Transação");
                key = Console.ReadKey();
            }

            Console.ReadKey();
        }
    }
}
