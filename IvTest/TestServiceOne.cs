using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class TestServiceOne
    {
        private TestServiceTwo ServiceTwo;

        public TestServiceOne(TestServiceTwo two)
        {
            ServiceTwo = two;
        }

        public void Use()
        {
            ServiceTwo.Use();
        }
    }
}
