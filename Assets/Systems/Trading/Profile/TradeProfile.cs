using System.Collections.Generic;
using TradingSystem.Interest;

namespace TradingSystem.Profile
{
    public readonly struct TradeProfile
    {
        private readonly Dictionary<Good, TradeInterest> _trades;
        private readonly Dictionary<Good, int> _buyingInterests;
        public IReadOnlyDictionary<Good, TradeInterest> Trades => _trades;
        public IReadOnlyDictionary<Good, int> BuyingInterests => _buyingInterests;

        public TradeProfile(Dictionary<Good, TradeInterest> trades, Dictionary<Good, int> buyingInterests)
        {
            _trades = trades;
            _buyingInterests = buyingInterests;
        }
    }
}