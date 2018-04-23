using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using NorthwindLibrary;

namespace CachingSolutionsSamples.Generic
{
    public class EntitiesManager<TEntity> where TEntity : class, new()
    {
        private IEntitiesCache<TEntity> cache;

        public EntitiesManager(IEntitiesCache<TEntity> cache)
        {
            this.cache = cache;
        }

        public void AddItem(TEntity entity)
        {
            using (var dbContext = GetContext())
            {
                dbContext.Set<TEntity>().Add(entity);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<TEntity> GetEntities()
        {
            Console.WriteLine("Get " + typeof(TEntity).Name);

            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = cache.Get(user);

            if (entities == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = GetContext())
                {
                    entities = dbContext.Set<TEntity>().ToList();
                    cache.Set(user, entities);
                }
            }

            return entities;
        }

        private DbContext GetContext()
        {
            var dbContext = new Northwind();
            dbContext.Configuration.LazyLoadingEnabled = false;
            dbContext.Configuration.ProxyCreationEnabled = false;

            return dbContext;
        }
    }
}
