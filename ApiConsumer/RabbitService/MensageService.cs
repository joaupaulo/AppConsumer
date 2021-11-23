using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiConsumer.ApiServices;
using ApiConsumer.DatabaseSettings;
using ApiConsumer.Entidades;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiConsumer.RabbitService
{
    public class MensageService : IMessageService
    {

        private readonly IMongoCollection<Produtos> _context;
        private readonly IProdutosService _produtosService;
        public MensageService(IApiConsumerDatabase settings, IProdutosService produtosService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Produtos>(settings.ProdutosCollectionName);
            _produtosService = produtosService;
        }





        public void ConnectMensage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var pedido = System.Text.Json.JsonSerializer.Deserialize<Produtos>(message);



                    long verificacao = _context.CountDocuments(x => x.Id == pedido.Id);

                    if (verificacao > 0)
                    {


                        _produtosService.VenderProduto(pedido);




                    }
                    else
                    {


                        pedido.Amount = pedido.Amount - pedido.QuantVendidos;
                        _produtosService.CriarProduto(pedido);
                    }




                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }





        }
    }
}
