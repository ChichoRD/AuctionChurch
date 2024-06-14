using UnityEngine;

namespace TradingSystem.Interest
{
    [CreateAssetMenu(fileName = "New Trade Interest", menuName = "Trading System/Interest/Trade Interest Flyweight")]
    internal class TradeInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private GoodInterestFlyweight _buyingGood;

        [SerializeField]
        private GoodInterestFlyweight[] _sellingGoods;

        public TradeInterest Create() =>
            new TradeInterest(_buyingGood.Create(), System.Array.ConvertAll(_sellingGoods, x => x.Create()));
    }
}