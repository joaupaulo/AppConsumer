using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.Entidades;

namespace ApiConsumer.ApiServices
{
    public interface IProdutosService
    {
        Task CriarProduto(Produtos product);
        Task<bool> VenderProduto(Produtos product);
    }
}
