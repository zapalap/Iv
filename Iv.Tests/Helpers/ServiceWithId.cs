using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class ServiceWithId
    {
        public Guid Id = Guid.NewGuid();

        public ServiceWithId()
        {

        }

        public void Use()
        {
            Console.WriteLine(Id);
        }
    }
}
