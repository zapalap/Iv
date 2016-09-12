using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Tests.Helpers
{
    public class ConsumingServiceD
    {
        public DisposableProvidedServiceA ServiceA { get; set; }

        public ConsumingServiceD(DisposableProvidedServiceA serviceA)
        {
            ServiceA = serviceA;
        }
    }
}
