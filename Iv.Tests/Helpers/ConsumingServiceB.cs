using IvTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Tests.Helpers
{
    public class ConsumingServiceB : ServiceWithId
    {
        public ProvidedServiceC ServiceC { get; set; }

        public ConsumingServiceB(ProvidedServiceC serviceC)
        {
            ServiceC = serviceC;
        }
    }
}
