using UnityEngine;

namespace TradingSystem.Interest
{
    [CreateAssetMenu(fileName = "New Trade Interest", menuName = "Trading System/Interest/Trade Interest Flyweight")]
    internal class TradeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private GoodInterestBuilder _buyingGood;

        [SerializeField]
        private GoodInterestBuilder[] _sellingGoods;

        public TradeInterest Create() =>
            new TradeInterest(_buyingGood.Build(), System.Array.ConvertAll(_sellingGoods, x => x.Build()));
    }
}