using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiConsumer.DatabaseSettings;
using ApiConsumer.Entidades;
using AppConsumer.AppServices;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiConsumer.RabbitService
{
    public class MessageService : IMessageService
    {

        private readonly IMongoCollection<Product> _context;
        private readonly IProductService _productService;
        public MessageService(IAppConsumerDatabase settings, IProductService productService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Product>(settings.ProductCollectionName);
            _productService = productService;
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
                    var request = System.Text.Json.JsonSerializer.Deserialize<Product>(message);



                    long check = _context.CountDocuments(x => x.Id == request.Id);

                    if (check > 0)
                    {

                        request.Amount = request.Amount - request.QuantityProducts;
                        _productService.SellProduct(request);




                    }
                    else
                    {


                        _productService.CreateProduct(request);
                    }




                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

              
            }





        }
    }
}
