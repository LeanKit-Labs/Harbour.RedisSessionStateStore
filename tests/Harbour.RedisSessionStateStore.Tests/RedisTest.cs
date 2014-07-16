using ServiceStack.Redis;
using System;

namespace Harbour.RedisSessionStateStore.Tests
{
    public abstract class RedisTest : IDisposable
    {
        // TODO: Should be different than development port!
        protected virtual string Host { get { return "172.16.83.1:6379"; } }

        public IRedisClientsManager ClientManager { get; protected set; }

        protected readonly IRedisClient redis;

        protected RedisTest()
        {
            ClientManager = new BasicRedisClientManager(Host);
            redis = GetRedisClient();
        }

        protected virtual IRedisClient GetRedisClient()
        {
            var client = ClientManager.GetClient();
            client.FlushAll();
            return client;
        }

        public virtual void Dispose()
        {
            redis.Dispose();
            ClientManager.Dispose();
        }
    }
}
