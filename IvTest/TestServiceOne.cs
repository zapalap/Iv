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
        private TestServiceNested Nested;

        public TestServiceOne(TestServiceTwo two, TestServiceNested nested)
        {
            ServiceTwo = two;
            Nested = nested;
        }

        public void Use()
        {
            ServiceTwo.Use();
            Nested.Use();
        }
    }
}
