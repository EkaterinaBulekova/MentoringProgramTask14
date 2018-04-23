using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.CategorySamle;
using CachingSolutionsSamples.Fibo;
using CachingSolutionsSamples.Generic;
using NorthwindLibrary;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
	    [TestMethod]
	    public void MemoryCache()
	    {
	        var categoryManager = new CategoriesManager(new CategoriesMemoryCache());

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(categoryManager.GetCategories().Count());
	            Thread.Sleep(100);
	        }
	    }

	    [TestMethod]
	    public void RedisCache()
	    {
	        var categoryManager = new CategoriesManager(new CategoriesRedisCache("localhost"));

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(categoryManager.GetCategories().Count());
	            Thread.Sleep(100);
	        }
	    }

        [TestMethod]
        public void MemoryCacheGenericCustomerWithOutInvalidation()
        {
            var entityManager = new EntitiesManager<Customer>(new EntitiesMemoryCache<Customer>());

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(1000);
            }
        }

	    [TestMethod]
	    public void MemoryCacheGenericSupplierWithOutInvalidation()
	    {
	        var entityManager = new EntitiesManager<Supplier>(new EntitiesMemoryCache<Supplier>());

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(1000);
	        }
	    }

	    [TestMethod]
	    public void MemoryCacheGenericCustomerWithTimeInvalidation()
	    {
	        var entityManager = new EntitiesManager<Customer>(new EntitiesMemoryCache<Customer>(){PolicyLevel = PolicyType.ExpirationTime});

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(1000);
	        }
	    }

	    [TestMethod]
	    public void MemoryCacheGenericSupplierWithTimeInvalidation()
	    {
	        var entityManager = new EntitiesManager<Supplier>(new EntitiesMemoryCache<Supplier>(){PolicyLevel = PolicyType.ExpirationTime});

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(1000);
	        }
	    }

	    [TestMethod]
	    public void MemoryCacheGenericOrderWithTimeInvalidation()
	    {
	        var entityManager = new EntitiesManager<Order>(new EntitiesMemoryCache<Order>() { PolicyLevel = PolicyType.ExpirationTime });

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(1000);
	        }
	    }


        [TestMethod]
	    public void MemoryCacheGenericSupplierWithChangeDbInvalidation()
        {
	        var entityManager = new EntitiesManager<Supplier>(new EntitiesMemoryCache<Supplier>
	        {
	            PolicyLevel = PolicyType.ChangeMonitorSql 
	        });
	        var supplier = new Supplier
	        {
	            CompanyName = "Company",
	            ContactName = "Contact"
	        };
	        for (var i = 0; i < 10; i++)
	        {
	            if (i == 5)
	                entityManager.AddItem(supplier);
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(100);
	        }

        }

        [TestMethod]
        public void RedisCacheGenericCustomer()
        {
            var entityManager = new EntitiesManager<Customer>(new EntitiesRedisCache<Customer>("localhost"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entityManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

	    [TestMethod]
	    public void RedisCacheGenericSupplier()
	    {
	        var entityManager = new EntitiesManager<Supplier>(new EntitiesRedisCache<Supplier>("localhost"));

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(100);
	        }
	    }

	    [TestMethod]
	    public void RedisCacheGenericOrder()
	    {
	        var entityManager = new EntitiesManager<Order>(new EntitiesRedisCache<Order>("localhost"));

	        for (var i = 0; i < 10; i++)
	        {
	            Console.WriteLine(entityManager.GetEntities().Count());
	            Thread.Sleep(100);
	        }
	    }


	    [TestMethod]
	    public void MemoryCacheFibonacci()
	    {
	        var fibbonacci = new Fibbonacci(new FiboMemoryCache());

	        Console.WriteLine(fibbonacci.Get(10));
	    }

	    [TestMethod]
	    public void RedisCacheFibonacci()
	    {
	        var fibbonacci = new Fibbonacci(new FiboRedisCache("localhost"));

	        Console.WriteLine(fibbonacci.Get(10));
	    }
    }
}
