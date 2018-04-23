using System.Collections.Generic;

namespace CachingSolutionsSamples.Generic
{
    public interface IEntitiesCache<TEntity> where TEntity : class 
    {
        IEnumerable<TEntity> Get(string forUser);
        void Set(string forUser, IEnumerable<TEntity> entities);
    }
}