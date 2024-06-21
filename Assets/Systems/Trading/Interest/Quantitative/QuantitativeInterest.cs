using TradingSystem.Distribution;
using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    public readonly struct QuantitativeInterest
    {
        public readonly int minimumQuantity;
        public readonly int maximumQuantity;
        private readonly ProbabilityDensityFunctionSampler _probabilityDensityFunction;
        public QuantitativeInterest(int minimumQuantity, int maximumQuantity, ProbabilityDensityFunctionSampler probabilityDensityFunction)
        {
            this.minimumQuantity = minimumQuantity;
            this.maximumQuantity = maximumQuantity; 
            _probabilityDensityFunction = probabilityDensityFunction;
        }

        public int GetRandomItemQuantity() => Mathf.CeilToInt(Mathf.Lerp(minimumQuantity, maximumQuantity, _probabilityDensityFunction.NormalizedRandomSample()));

        public float GetQuantityRangeInterest(int minimumQuantity, int maximumQuantity) =>
            _probabilityDensityFunction.ProbabilityOfRange(
                Mathf.InverseLerp(this.minimumQuantity, this.maximumQuantity, minimumQuantity),
                Mathf.InverseLerp(this.minimumQuantity, this.maximumQuantity, maximumQuantity));
    }
}