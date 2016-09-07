using Iv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvTest
{
    class Program
    {
        private static Container Container = new Container();

        static void Main(string[] args)
        {
            Container.Register<TestServiceOne, TestServiceOne>();
            Container.Register<TestServiceTwo, TestServiceTwo>();
            Container.Register<TestServiceThree, TestServiceThree>();
            Container.Register<TestServiceNested, TestServiceNested>();
            Container.Register<TestController, TestController>();

            var controller = (TestController)Container.Resolve<TestController>();

            controller.Work();

        }
    }
}
