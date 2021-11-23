using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Entidades;

namespace AppConsumer.AppServices
{
    public interface IProdutosService
    {
        Task CreateProduct(Product product);
        Task<bool> SellProduct(Product product);
    }
}
