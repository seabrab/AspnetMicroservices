using Basket.API.Data.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _RedisConnectionMultiplexer;

        public BasketContext(ConnectionMultiplexer redisConnectionMultiplexer)
        {
            _RedisConnectionMultiplexer = redisConnectionMultiplexer;
            Redis = _RedisConnectionMultiplexer.GetDatabase();
        }


        public IDatabase Redis { get; }
    }
}
