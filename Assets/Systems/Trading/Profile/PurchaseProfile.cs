using System.Collections.Generic;
using TradingSystem.Interest;

namespace TradingSystem.Profile
{
    public readonly struct PurchaseProfile
    {
        private readonly Dictionary<Good, PurchaseInterest> _purchases;
        private readonly Dictionary<Good, int> _buyingInterests;
        public IReadOnlyDictionary<Good, PurchaseInterest> Prices => _purchases;
        public IReadOnlyDictionary<Good, int> BuyingInterests => _buyingInterests;

        public PurchaseProfile(Dictionary<Good, PurchaseInterest> purchases, Dictionary<Good, int> buyingInterests)
        {
            _purchases = purchases;
            _buyingInterests = buyingInterests;
        }
    }
}