using System;

namespace Iv.Binding
{
    public class CachedServiceConfiguration
    {
        public Type ProvideType { get; set; }
        public object Instance { get; set; }
        public Provisioning Provisioning { get; set; }
        public Lifetime Lifetime { get; set; }
        public Container Container { get; set; }
        public Func<Container, object> ProvideFunction { get; set; }
    }
}