using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class TestServiceNested : IDisposable
    {
        public Guid Id = Guid.NewGuid();

        public TestServiceNested()
        {

        }

        public void Dispose()
        {
            Console.WriteLine("Disposing...");
        }

        public void Use()
        {
            Console.WriteLine(Id);
        }
    }
}
