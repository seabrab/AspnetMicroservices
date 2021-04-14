using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisDistributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            _redisDistributedCache = distributedCache;
        }


        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisDistributedCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
            
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
             await _redisDistributedCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);    
        }
        public async Task DeleteBasket(string userName)
        {
            await _redisDistributedCache.RemoveAsync(userName);
                
        }
    }
}
