using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace CachingSolutionsSamples.Generic
{
	class EntitiesRedisCache<TEntity> : IEntitiesCache<TEntity> where TEntity : class 
	{
		private ConnectionMultiplexer redisConnection;
	    private string prefix = "Cache_" + typeof(TEntity).Name;
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<TEntity>));

		public EntitiesRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

        public void Set(string forUser, IEnumerable<TEntity> entities)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (entities == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, entities);
                db.StringSet(key, stream.ToArray());
            }
        }

	    public IEnumerable<TEntity> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<TEntity>)serializer
                .ReadObject(new MemoryStream(s));
        }
    }
}
