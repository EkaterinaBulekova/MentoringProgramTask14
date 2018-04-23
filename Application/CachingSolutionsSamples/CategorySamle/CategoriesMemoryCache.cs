using NorthwindLibrary;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.CategorySamle
{
	internal class CategoriesMemoryCache : ICategoriesCache
	{
	    readonly ObjectCache _cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) _cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> categories)
		{
			_cache.Set(prefix + forUser, categories, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
