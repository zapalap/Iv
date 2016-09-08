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
        public Dictionary<Type, ServiceRecord> ServiceRegistry { get; set; }
        public HashSet<IDisposable> DisposeHistory { get; private set; }

        public Container()
        {
            ServiceRegistry = new Dictionary<Type, ServiceRecord>();
            DisposeHistory = new HashSet<IDisposable>();
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
                DisposeHistory.Add(instance as IDisposable);
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

            if (serviceRecord.Provisioning == Provisioning.ByInstance && serviceRecord.Instance != null)
            {
                if (serviceRecord.Lifetime == Lifetime.Singleton)
                {
                    return serviceRecord.Instance;
                }

                if (serviceRecord.Lifetime == Lifetime.Transient)
                {
                    return serviceRecord.ProvideFunction.Invoke(this);
                }
            }

            // TODO : Introduce extensible strategy for choosing the right constructor (e.g. the one that has the most registered dependencies)
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

        public void Dispose()
        {
            // Dispose all instances that need disposing
            foreach (var instance in DisposeHistory)
            {
                instance.Dispose();
            }
        }
    }
}