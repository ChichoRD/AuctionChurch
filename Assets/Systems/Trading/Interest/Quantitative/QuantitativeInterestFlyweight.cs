using TradingSystem.Distribution;
using UnityEngine;

namespace TradingSystem.Interest.Quantitative
{
    [CreateAssetMenu(fileName = "New Quantitative Interest", menuName = "Trading System/Interest/Quantitative Interest Flyweight")]
    internal class QuantitativeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private ProbabilityDensityFunctionSampler.Builder _interestParameters;

        public QuantitativeInterest Create() =>
            new QuantitativeInterest(_interestParameters.Build());
    }
}