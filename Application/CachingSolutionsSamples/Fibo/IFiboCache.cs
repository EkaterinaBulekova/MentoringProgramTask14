using System.Collections.Generic;

namespace CachingSolutionsSamples.Fibo
{
    public interface IFiboCache 
    {
        IEnumerable<int> Get(string forUser);
        void Set(string forUser, IEnumerable<int> number);
    }
}