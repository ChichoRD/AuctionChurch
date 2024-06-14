using System.Collections.Generic;
using TradingSystem.Interest;
using UnityEngine;

namespace TradingSystem.Profile
{
    [CreateAssetMenu(fileName = "New Trade Profile", menuName = "Trading System/Profile/Trade Profile Flyweight")]
    internal class TradeProfileFlyweight : ScriptableObject
    {
        [SerializeField]
        private TradeInterestFlyweight[] _tradeInterests;

        public TradeProfile Create()
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
    }
}