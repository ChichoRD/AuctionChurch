using TradingSystem.Interest.Qualitative;
using TradingSystem.Interest.Quantitative;
using UnityEngine;

namespace TradingSystem.Interest
{
    [CreateAssetMenu(fileName = "New Good Interest", menuName = "Trading System/Interest/Good Interest Flyweight")]
    internal class GoodInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private Good _good;

        [SerializeField]
        private QualitativeInterestFlyweight _qualitativeInterest;

        [SerializeField]
        private QuantitativeInterestFlyweight _quantitativeInterest;

        public GoodInterest Create() =>
            new GoodInterest(_good, _qualitativeInterest.Create(), _quantitativeInterest.Create());
    }
}