using System.Collections.Generic;
using System.Runtime.Caching;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Fibo
{
	internal class FiboMemoryCache : IFiboCache
	{
	    readonly ObjectCache _cache = MemoryCache.Default;
		string prefix  = "Cache_Fibo";

		public IEnumerable<int> Get(string forUser)
		{
			return (IEnumerable<int>) _cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<int> number)
		{
			_cache.Set(prefix + forUser, number, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
