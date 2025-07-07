using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Common.Extensions
{
    public static class NumericExtensions
    {
        public static FP ZeroIfNegative(this FP value)
        {
            return value >= 0 ? value : 0;
        }

        public static int ZeroIfNegative(this int value)
        {
            return value >= 0 ? value : 0;
        }
    }
}