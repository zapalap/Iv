using System;

namespace Iv.Binding
{
    public static class BindingExtensions
    {
        public static ServiceRecord For<TService>(this Container container)
        {
            var record = new ServiceRecord();
            record.Container = container;

            if (container.ServiceRegistry.ContainsKey(typeof(TService)))
            {
                container.ServiceRegistry[typeof(TService)] = record;
            }
            else
            {
                container.ServiceRegistry.Add(typeof(TService), record);
            }

            return record;
        }

        public static ServiceRecord Provide<TService>(this ServiceRecord record)
        {
            record.ProvideType = typeof(TService);
            record.Provisioning = Provisioning.ByType;
            record.Lifetime = Lifetime.Transient;
            return record;
        }

        public static ServiceRecord Provide(this ServiceRecord record, Func<Container, object> provide)
        {
            record.Instance = provide.Invoke(record.Container);
            record.Provisioning = Provisioning.ByInstance;
            record.ProvideFunction = provide;
            return record;
        }

        public static ServiceRecord AsSingleton(this ServiceRecord record)
        {
            record.Lifetime = Lifetime.Singleton;
            return record;
        }
    }
}