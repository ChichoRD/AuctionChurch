using UnityEngine;

namespace TradingSystem.Interest
{
    [CreateAssetMenu(fileName = "New Purchase Interest Flyweight", menuName = "Trading System/Interest/Purchase Interest Flyweight")]
    internal class PurchaseInterestFlyweight : ScriptableObject
    {
        [SerializeField]
        private GoodInterestBuilder _buyingGood;

        [SerializeField]
        private PriceInterestBuilder _sellingPrice;

        public PurchaseInterest Create() =>
            new PurchaseInterest(_buyingGood.Build(), _sellingPrice.Build());
    }
}