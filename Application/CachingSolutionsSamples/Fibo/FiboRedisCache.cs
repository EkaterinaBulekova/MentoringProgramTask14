using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace CachingSolutionsSamples.Fibo
{
    public class FiboRedisCache : IFiboCache 
	{
		private ConnectionMultiplexer redisConnection;
	    private string prefix = "Cache_Fibo";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<int>));

		public FiboRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

        public void Set(string forUser, IEnumerable<int> numbers)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (numbers == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, numbers);
                db.StringSet(key, stream.ToArray());
            }
        }

	    public IEnumerable<int> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<int>)serializer
                .ReadObject(new MemoryStream(s));
        }
    }
}
