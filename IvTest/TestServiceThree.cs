using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class TestServiceThree : IDisposable
    {
        public TestServiceThree()
        {

        }

        public void Dispose()
        {
            Console.WriteLine("Disposing...");
        }

        public void Use()
        {
            Console.WriteLine("Three");
        }
    }
}
