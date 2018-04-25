using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Generic
{
    public class EntitiesMemoryCache<TEntity> : IEntitiesCache<TEntity> where TEntity : class
	{
	    private const string Sql = "SELECT [Extent1].[SupplierID] AS[SupplierID], [Extent1].[CompanyName] AS[CompanyName], [Extent1].[ContactName] AS[ContactName], [Extent1].[ContactTitle] AS[ContactTitle], [Extent1].[Address] AS[Address], [Extent1].[City] AS[City], [Extent1].[Region] AS[Region], [Extent1].[PostalCode] AS[PostalCode], [Extent1].[Country] AS[Country], [Extent1].[Phone] AS[Phone], [Extent1].[Fax] AS[Fax], [Extent1].[HomePage]AS[HomePage]FROM[dbo].[Suppliers] AS[Extent1]";
        private readonly ObjectCache _cache = MemoryCache.Default;
		private readonly string _prefix  = "Cache_" + typeof(TEntity).Name;
	    private string _forUser;

	    public PolicyType PolicyLevel { get; set; } = PolicyType.None;


	    public void Set(string forUser, IEnumerable<TEntity> entities)
	    {
	        _forUser = forUser;
            var key = _prefix + _forUser;
	        switch (PolicyLevel)
	        {
                case PolicyType.ChangeMonitorSql :
                    var connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
                    SqlDependency.Stop(connectionString);
                    SqlDependency.Start(connectionString);
                    var policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(10)
                    };
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand(Sql,connection))
                        {
                            command.Notification = null;
                            var dependency = new SqlDependency(command);
                            var monitor = new SqlChangeMonitor(dependency);
                            policy.ChangeMonitors.Add(monitor);
                            _cache.Set(key, entities, policy);
                            command.ExecuteNonQuery();
                        }
                    }
                    break;
                case PolicyType.ExpirationTime :
                    _cache.Set(key, entities, new DateTimeOffset(DateTime.UtcNow.AddSeconds(3)));
                    break;
                case PolicyType.None:
                    _cache.Set(key, entities, ObjectCache.InfiniteAbsoluteExpiration);
                    break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
        }

        IEnumerable<TEntity> IEntitiesCache<TEntity>.Get(string forUser)
        {
            return (IEnumerable<TEntity>)_cache.Get(_prefix + forUser);
        }
    }
}
