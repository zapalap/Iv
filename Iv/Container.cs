using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var instance =  ResolveService(type);

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
                throw new InvalidOperationException($"No service registered for type:{type.Name}");
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

    public enum Provisioning
    {
        ByType,
        ByInstance
    }

    public enum Lifetime
    {
        Transient,
        Singleton
    }

    public class ServiceRecord
    {
        public Type ProvideType { get; set; }
        public object Instance { get; set; }
        public Provisioning Provisioning { get; set; }
        public Lifetime Lifetime { get; set; }
        public Container Container { get; set; }
        public Func<Container, object> ProvideFunction { get; set; }
    }

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
