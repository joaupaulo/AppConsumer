using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.DatabaseSettings;
using ApiConsumer.Entidades;
using MongoDB.Driver;

namespace ApiConsumer.ApiServices
{
    public class ProdutosService : IProdutosService
    {
        private readonly IMongoCollection<Produtos> _context;

        public ProdutosService(IApiConsumerDatabase settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _context = database.GetCollection<Produtos>(settings.ProdutosCollectionName);
        }



        public async Task CriarProduto(Produtos product)
        {

           

            await _context.InsertOneAsync(product);
        }

     

        public async Task<bool> VenderProduto(Produtos product)
        {

            

            var updateResult = await _context.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

    }
}


