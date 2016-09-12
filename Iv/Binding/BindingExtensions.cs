using System;

namespace Iv.Binding
{
    public static class BindingExtensions
    {
        public static CachedServiceConfiguration For<TService>(this Container container)
        {
            var record = new CachedServiceConfiguration();
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

        public static CachedServiceConfiguration Provide<TService>(this CachedServiceConfiguration record)
        {
            record.ProvideType = typeof(TService);
            record.Provisioning = Provisioning.ByType;
            record.Lifetime = Lifetime.Transient;
            return record;
        }

        public static CachedServiceConfiguration Provide(this CachedServiceConfiguration record, Func<Container, object> provide)
        {
            record.Provisioning = Provisioning.ByInstance;
            record.ProvideFunction = provide;
            return record;
        }

        public static CachedServiceConfiguration AsSingleton(this CachedServiceConfiguration record)
        {
            record.Lifetime = Lifetime.Singleton;
            return record;
        }
    }
}