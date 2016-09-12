using Iv.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv
{
    [Serializable]
    public class DependencyResolutionException : Exception
    {
        public Type ForType { get; private set; }

        public DependencyResolutionException(string message, Type forType, Dictionary<Type, CachedServiceConfiguration> serviceRegistry) : base(message)
        {
            ForType = forType;
            Data.Add("ServiceRegistry", serviceRegistry);
        }
    }
}
