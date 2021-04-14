using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
//using EventBusRabbitMQ.Events;
//using EventBusRabbitMQ.Producer;
//using EventBusRabbitMQ.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        //private readonly EventBusRabbitMQProducer _eventBus;

        public BasketController(IBasketRepository basketRepository, IMapper mapper/*, EventBusRabbitMQProducer eventBus*/)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            //_eventBus = eventBus;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
  
            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            await _basketRepository.DeleteBasket(basket.UserName);
            

            //var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            //eventMessage.RequestId =  Guid.NewGuid();
            //eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                //_eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue,eventMessage);
            }
            catch (Exception)
            { 

                throw;
            }

            return Accepted();
        }


    }
}
