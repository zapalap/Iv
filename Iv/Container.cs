﻿using System;
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
        private Dictionary<Type, object> InstanceRegistry;

        public Container()
        {
            ServiceRegistry = new Dictionary<Type, Type>();
            InstanceRegistry = new Dictionary<Type, object>();
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

        /// <summary>
        /// Obtains service instance from the container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            //If we already have instance registered provide it right away
            if (InstanceRegistry.ContainsKey(type))
            {
                return InstanceRegistry[type];
            }

            if (!ServiceRegistry.ContainsKey(type))
            {
                throw new InvalidOperationException($"No service registered for type:{type.Name}");
            }

            var serviceType = ServiceRegistry[type];
            // TODO : Introduce extensible strategy for choosing the right constructor (e.g. the one that has the most registered dependencies)
            var dependencies = serviceType.GetConstructors().FirstOrDefault().GetParameters();
            var dependencyInstances = new List<object>();

            foreach (var dependency in dependencies)
            {
                dependencyInstances.Add(Resolve(dependency.ParameterType));
            }
            // TODO : Introduce different scopes for created services. 
            // For now the only scope is transient (one per call to Resolve()), but should also have singleton and possiblity to extend for things like one-per-request etc.
            // container.For<IFoo>.Provide<Bar>().AsSingleton();
            var instance = Activator.CreateInstance(serviceType, dependencyInstances.ToArray());

            return instance;
        }

        /// TODO : Use builder pattern and fluent interface to make apis like this more developer friendly. 
        /// Ideally should be something like this: 
        /// normal way:
        ///     container.For<IFoo>().Provide<Bar>()
        ///     
        /// to instance:
        ///     container.For<IFoo>().Provide(c => new Bar())

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
        
        /// <summary>
        /// Registers an already created object instance to be used during Resolve
        /// </summary>
        /// <typeparam name="TAbstract"></typeparam>
        /// <param name="creator"></param>
        public void RegisterInstance<TAbstract>(Func<Container, TAbstract> creator)
        {
            if (InstanceRegistry.ContainsKey(typeof(TAbstract)))
            {
                InstanceRegistry[typeof(TAbstract)] = creator.Invoke(this);
            }
            else
            {
                InstanceRegistry.Add(typeof(TAbstract), creator.Invoke(this));
            }
        }
    }
}
