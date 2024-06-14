using UnityEngine;

namespace TradingSystem.Interest.Qualitative
{
    [CreateAssetMenu(fileName = "New Qualitative Interest", menuName = "Trading System/Interest/Qualitative Interest Flyweight")]
    internal class QualitativeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private QualitativeInterestParameters.Builder _interestParameters;

        public QualitativeInterest Create()
        {
            QualitativeInterestParameters parameters = _interestParameters.Build();
            return new QualitativeInterest(parameters.interest.Evaluate(UnityEngine.Random.value));
        }
    }
}