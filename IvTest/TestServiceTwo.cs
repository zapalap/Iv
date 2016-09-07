using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    public class TestServiceTwo
    {
        private TestServiceThree ServiceThree;
        private TestServiceNested NestedService;

        public TestServiceTwo(TestServiceThree three, TestServiceNested nested)
        {
            ServiceThree = three;
            NestedService = nested;
        }

        public void Use()
        {
            ServiceThree.Use();
            NestedService.Use();
        }
    }
}
