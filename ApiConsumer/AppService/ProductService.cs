using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppConsumer.DatabaseSettings;
using AppConsumer.Model;
using MongoDB.Driver;

namespace AppConsumer.AppServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _context;

        public ProductService(IAppConsumerDatabase settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Product>(settings.ProductCollectionName);
        }



        public async Task CreateProduct(Product product)
        {

           

            await _context.InsertOneAsync(product);
        }

     

        public async Task<bool> SellProduct(Product product)
        {

            

            var updateResult = await _context.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

    }
}


