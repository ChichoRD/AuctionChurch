using System;
using System.Collections.Generic;
using TradingSystem.Interest;

namespace TradingSystem.Profile
{
    public readonly struct AccountableTradeProfile
    {
        private readonly TradeProfile _tradeProfile;
        private readonly Dictionary<Good, int> _remainingTrades;
        private readonly Action<Good> _itemDepleted;

        public IReadOnlyDictionary<Good, int> RemainingTrades => _remainingTrades;
        
        public AccountableTradeProfile(TradeProfile tradeProfile, Action<Good> itemDepleted)
        {
            _tradeProfile = tradeProfile;
            _remainingTrades = new Dictionary<Good, int>(tradeProfile.BuyingInterests);
            _itemDepleted = itemDepleted;
        }

        public bool TryTrade(IncomingTrade incomingTrade, out Trade trade)
        {
            trade = default;

            if (!_remainingTrades.TryGetValue(incomingTrade.good, out int remainingQuantity)
                || !_tradeProfile.Trades.TryGetValue(incomingTrade.good, out TradeInterest interest))
                return false;

            int clampedIncomingQuantity = Math.Min(incomingTrade.amount, remainingQuantity);
            IncomingTrade clampedIncomingTrade = incomingTrade.WithAmount(clampedIncomingQuantity);

            trade = new Trade(clampedIncomingTrade, interest.GetRandomTrade().Extrapolate(clampedIncomingTrade));
            int newRemainingQuantity = remainingQuantity - incomingTrade.amount;

            if (newRemainingQuantity <= 0)
            {
                _remainingTrades.Remove(incomingTrade.good);
                _itemDepleted(incomingTrade.good);
            }
            else
                _remainingTrades[incomingTrade.good] = newRemainingQuantity;

            return true;
        }
    }
}