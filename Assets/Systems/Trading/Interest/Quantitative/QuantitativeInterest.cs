using TradingSystem.Distribution;
using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    public readonly struct QuantitativeInterest
    {
        private readonly ProbabilityDensityFunctionSampler _probabilityDensityFunction;
        public QuantitativeInterest(ProbabilityDensityFunctionSampler probabilityDensityFunction)
        {
            _probabilityDensityFunction = probabilityDensityFunction;
        }

        public int GetRandomItemQuantity() => Mathf.CeilToInt(_probabilityDensityFunction.RandomSample());

        public float GetQuantityRangeInterest(int minimumQuantity, int maximumQuantity) =>
            _probabilityDensityFunction.ProbabilityOfRange(minimumQuantity, maximumQuantity);
    }
}