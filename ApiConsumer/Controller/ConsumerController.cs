using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiConsumer.ApiServices;
using ApiConsumer.RabbitService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiConsumer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {


        private ILogger<ConsumerController> _logger;

        private readonly IMessageService _mensagem;
        public ConsumerController( IMessageService servico)
        {

            _mensagem = servico;
        }


        [HttpGet]
        public ActionResult ReceberPedido()
        {
            try
            {
                _mensagem.ConnectMensage();

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar criar um novo pedido", ex);


               
            }
            return new StatusCodeResult(200);

        }


    }
}
