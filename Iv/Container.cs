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
    public class Container
    {
        private Dictionary<Type, Type> ServiceRegistry;

        public Container()
        {
            ServiceRegistry = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Obtains service instance from the container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public object Resolve<T>()
        {
            return Resolve(typeof(T));
        }

        /// <summary>
        /// Obtains service instance from the container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            if (!ServiceRegistry.ContainsKey(type))
            {
                throw new InvalidOperationException($"No service registered for type:{type.Name}");
            }

            var serviceType = ServiceRegistry[type];
            var dependencies = type.GetConstructors().FirstOrDefault().GetParameters();
            var dependencyInstances = new List<object>();

            foreach (var dependency in dependencies)
            {
                dependencyInstances.Add(Resolve(dependency.ParameterType));
            }

            return Activator.CreateInstance(serviceType, dependencyInstances.ToArray());
        }

        /// <summary>
        ///  Registers service implementations for injection
        /// </summary>
        /// <typeparam name="TAbstract"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        public void Register<TAbstract, TConcrete>()
        {
            if (ServiceRegistry.ContainsKey(typeof(TAbstract)))
            {
                ServiceRegistry[typeof(TAbstract)] = typeof(TConcrete);
            }
            else
            {
                ServiceRegistry.Add(typeof(TAbstract), typeof(TConcrete));
            }
        }
    }
}
