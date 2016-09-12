using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class DisposableServiceWithId : IDisposable
    {
        public Guid Id = Guid.NewGuid();

        public DisposableServiceWithId()
        {

        }

        public void Dispose()
        {
            Console.WriteLine($"Disposing {Id}");
        }

        public void Use()
        {
            Console.WriteLine(Id);
        }
    }
}
