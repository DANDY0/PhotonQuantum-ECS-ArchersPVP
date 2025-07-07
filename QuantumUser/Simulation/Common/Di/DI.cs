using Zenject;

namespace Quantum.QuantumUser.Simulation.Common.Di
{
    public static class DI
    {
        private static DiContainer _container;

        private static DiContainer Container => _container ??= ProjectContext.Instance.Container;

        public static T Resolve<T>() => Container.Resolve<T>();
    }
}