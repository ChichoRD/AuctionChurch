using TradingSystem.Distribution;
using UnityEngine;

namespace TradingSystem.Interest.Qualitative
{
    [CreateAssetMenu(fileName = "New Qualitative Interest", menuName = "Trading System/Interest/Qualitative Interest Flyweight")]
    internal class QualitativeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private ProbabilityDensityFunctionSampler.Builder _interestParameters;

        public QualitativeInterest Create() =>
            new QualitativeInterest(_interestParameters.Build().NormalizedRandomSample());
    }
}