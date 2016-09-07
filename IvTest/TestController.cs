using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class TestController
    {
        private TestServiceOne One;
        private TestServiceTwo Two;
        private TestServiceThree Three;

        public TestController(TestServiceOne one, TestServiceTwo two, TestServiceThree three)
        {
            One = one;
            Two = two;
            Three = three;
        }

        public void Work()
        {
            One.Use();
            Two.Use();
            Three.Use();
        }

    }
}
