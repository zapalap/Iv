using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Iv;
using Iv.Binding;
using Iv.Tests.Helpers;

namespace Iv.Tests
{
    [TestClass]
    public class ContainerTest
    {
        [TestMethod]
        public void Container_GiveTheSameInstancePerResolutionForSingletonLifetime()
        {
            var container = new Container();

            container.For<ProvidedServiceC>().Provide<ProvidedServiceC>().AsSingleton();
            container.For<ConsumingServiceA>().Provide<ConsumingServiceA>();
            container.For<ConsumingServiceB>().Provide<ConsumingServiceB>();

            var serviceA = container.Resolve<ConsumingServiceA>();
            var serviceB = container.Resolve<ConsumingServiceB>();

            Assert.AreEqual(serviceA.ServiceC.Id, serviceB.ServiceC.Id);
        }

        [TestMethod]
        public void Container_GiveNewInstancePerResolutionForTransientLifetime()
        {
            var container = new Container();

            container.For<ProvidedServiceC>().Provide<ProvidedServiceC>();
            container.For<ConsumingServiceA>().Provide<ConsumingServiceA>();
            container.For<ConsumingServiceB>().Provide<ConsumingServiceB>();

            var serviceA = container.Resolve<ConsumingServiceA>();
            var serviceB = container.Resolve<ConsumingServiceB>();

            Assert.AreNotEqual(serviceA.ServiceC.Id, serviceB.ServiceC.Id);
        }
        
        [TestMethod]
        public void Container_DisposableServicesGetCachedForEventualDisposal()
        {
            var container = new Container();

            container.For<DisposableProvidedServiceA>().Provide<DisposableProvidedServiceA>().AsSingleton();
            container.For<ConsumingServiceD>().Provide<ConsumingServiceD>();

            var serviceD = container.Resolve<ConsumingServiceD>();
            var serviceA = container.Resolve<DisposableProvidedServiceA>();

            Assert.IsTrue(container.InstanceCache.Contains(serviceA));
        }
    }
}
