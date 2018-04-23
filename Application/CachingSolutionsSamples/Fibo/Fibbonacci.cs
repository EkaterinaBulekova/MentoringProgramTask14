using System;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples.Fibo
{
    public class Fibbonacci
    {
        private readonly IFiboCache _cache;
        public Fibbonacci(IFiboCache cache)
        {
            _cache = cache;
        }
 

        public int Get(int n)
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            int[] fiboArray = {1, 1};
                for (var i = 2; i <= n; i++)
                {
                    var fiboPrevInts = _cache.Get(user);
                    fiboArray = fiboPrevInts?.ToArray() ?? fiboArray;
                    Console.WriteLine($"{fiboArray[0]},{fiboArray[1]}");
                    fiboArray[i % 2] = fiboArray[0] + fiboArray[1];
                    _cache.Set(user, fiboArray);
                }              
            
            return fiboArray[n % 2];
        }
    }
}
