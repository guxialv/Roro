using System;
using System.Collections.Generic;

namespace Roro.Flows.Services
{
    public sealed class ServiceCollection
    {
        private readonly Dictionary<Type, Func<object>> _factories;
        private readonly Dictionary<Type, object> _instances;

        internal ServiceCollection()
        {
            _factories = new Dictionary<Type, Func<object>>();
            _instances = new Dictionary<Type, object>();
        }

        public void Add<TService>(Func<TService> factory) where TService : class
        {
            _factories[typeof(TService)] = factory;
        }

        public TService? GetShared<TService>() where TService : class
        {
            if (!_factories.TryGetValue(typeof(TService), out Func<object> factory))
                return null;
            if (!_instances.TryGetValue(typeof(TService), out object instance))
                instance = _instances[typeof(TService)] = factory.Invoke();
            return (TService)instance;
        }
    }
}
