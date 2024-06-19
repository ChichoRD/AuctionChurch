using System.Collections.Generic;
using TradingSystem.Interest;
using UnityEngine;

namespace TradingSystem.Profile
{
    [CreateAssetMenu(fileName = "New Trade Profile", menuName = "Trading System/Profile/Trade Profile Flyweight")]
    internal class CommercialProfileFlyweight : ScriptableObject
    {
        [SerializeField]
        private TradeInterestFlyweight[] _tradeInterests;

        [SerializeField]
        private PurchaseInterestFlyweight[] _purchaseInterests;

        public TradeProfile CreateTradeProfile()
        {
            Dictionary<Good, TradeInterest> trades = new Dictionary<Good, TradeInterest>();
            Dictionary<Good, int> buyingInterests = new Dictionary<Good, int>();
            foreach (var tradeInterestFlyweight in _tradeInterests)
            {
                TradeInterest interest = tradeInterestFlyweight.Create();

                trades.Add(interest.buyingGood.good, interest);
                buyingInterests.Add(interest.buyingGood.good, interest.buyingGood.GetRandomItemQuantity());
            }

            return new TradeProfile(trades, buyingInterests);
        }

        public PurchaseProfile CreatePurchaseProfile()
        {
            Dictionary<Good, PurchaseInterest> purchases = new Dictionary<Good, PurchaseInterest>();
            Dictionary<Good, int> buyingInterests = new Dictionary<Good, int>();
            foreach (var purchaseInterestFlyweight in _purchaseInterests)
            {
                PurchaseInterest interest = purchaseInterestFlyweight.Create();

                purchases.Add(interest.buyingGood.good, interest);
                buyingInterests.Add(interest.buyingGood.good, interest.buyingGood.GetRandomItemQuantity());
            }

            return new PurchaseProfile(purchases, buyingInterests);
        }

        public CommercialProfile CreateCommercialProfile() =>
            new CommercialProfile(CreateTradeProfile(), CreatePurchaseProfile());
    }
}