using Iv;
using Iv.Binding;
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
            Container.For<TestServiceOne>().Provide<TestServiceOne>();
            Container.For<TestServiceTwo>().Provide<TestServiceTwo>();
            Container.For<TestServiceThree>().Provide<TestServiceThree>();
            Container.For<TestServiceNested>().Provide(c => new TestServiceNested()).AsSingleton();
            Container.For<TestController>().Provide<TestController>();

            var controller = Container.Resolve<TestController>();

            controller.Work();

            Container.Dispose();

        }
    }
}
