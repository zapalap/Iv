using Iv.Binding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iv
{
    /// <summary>
    /// Very simple DI container
    /// </summary>
    public class Container : IDisposable
    {
        public Dictionary<Type, CachedServiceConfiguration> ServiceRegistry { get; set; }
        public HashSet<IDisposable> InstanceCache { get; private set; }

        public Container()
        {
            ServiceRegistry = new Dictionary<Type, CachedServiceConfiguration>();
            InstanceCache = new HashSet<IDisposable>();
        }

        /// <summary>
        /// Obtains service instance from the container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            var instance = ResolveService(type);

            if (typeof(IDisposable).IsAssignableFrom(instance.GetType()))
            {
                InstanceCache.Add(instance as IDisposable);
            }

            return instance;
        }

        /// <summary>
        /// Obtains service instance from the container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ResolveService(Type type)
        {
            if (!ServiceRegistry.ContainsKey(type))
            {
                throw new DependencyResolutionException($"There is no service registered for type: {type.Name}", type, ServiceRegistry);
            }

            var serviceRecord = ServiceRegistry[type];

            if (serviceRecord.Provisioning == Provisioning.ByInstance)
            {
                return ProvideByInstance(serviceRecord);
            }

            // TODO : Introduce extensible strategy for choosing the right constructor (e.g. the one that has the most registered dependencies)
            return ProvideByType(serviceRecord);
        }

        private object ProvideByType(CachedServiceConfiguration serviceRecord)
        {
            var dependencies = serviceRecord.ProvideType.GetConstructors().FirstOrDefault().GetParameters();
            var dependencyInstances = new List<object>();

            foreach (var dependency in dependencies)
            {
                dependencyInstances.Add(Resolve(dependency.ParameterType));
            }

            var instance = Activator.CreateInstance(serviceRecord.ProvideType, dependencyInstances.ToArray());

            if (serviceRecord.Lifetime == Lifetime.Singleton)
            {
                serviceRecord.Provisioning = Provisioning.ByInstance;
                serviceRecord.Instance = instance;
            }

            return instance;
        }

        private object ProvideByInstance(CachedServiceConfiguration serviceRecord)
        {
            if (serviceRecord.Instance == null)
            {
                serviceRecord.Instance = serviceRecord.ProvideFunction.Invoke(this);
            }

            switch (serviceRecord.Lifetime)
            {
                case Lifetime.Transient:
                    EnsureProvideFunctionPresent(serviceRecord);
                    return serviceRecord.ProvideFunction.Invoke(this);
                case Lifetime.Singleton:
                    return serviceRecord.Instance;
                default:
                    return serviceRecord.Instance;
            }
        }

        private void EnsureProvideFunctionPresent(CachedServiceConfiguration serviceRecord)
        {
            if (serviceRecord.ProvideFunction == null)
            {
                throw new DependencyResolutionException(
                    $"Cannot provide service instance because provider function is null",
                    serviceRecord.ProvideType,
                    ServiceRegistry);
            }
        }

        public void Dispose()
        {
            // Dispose all instances that need disposing
            foreach (var instance in InstanceCache)
            {
                instance.Dispose();
            }
        }
    }
}