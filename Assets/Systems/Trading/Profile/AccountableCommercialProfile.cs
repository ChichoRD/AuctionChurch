using System;
using System.Collections.Generic;
using TradingSystem.Interest;

namespace TradingSystem.Profile
{
    public readonly struct AccountableCommercialProfile
    {
        private readonly AccountableTradeProfile _tradeProfile;
        private readonly AccountablePurchaseProfile _purchaseProfile;

        public IReadOnlyDictionary<Good, int> RemainingTrades => _tradeProfile.RemainingTrades;
        public IReadOnlyDictionary<Good, int> RemainingPurchases => _purchaseProfile.RemainingPurchases;

        public AccountableCommercialProfile(CommercialProfile commercialProfile, Action<Good> tradeItemDepleted, Action<Good> purchaseItemDepleted)
        {
            _tradeProfile = new AccountableTradeProfile(commercialProfile.tradeProfile, tradeItemDepleted);
            _purchaseProfile = new AccountablePurchaseProfile(commercialProfile.purchaseProfile, purchaseItemDepleted);
        }

        public bool TryDeal(IncomingTrade incomingTrade, out Trade trade) =>
            _tradeProfile.TryTrade(incomingTrade, out trade);
        public bool TryDeal(IncomingTrade incomingTrade, out Purchase purchase) =>
            _purchaseProfile.TryPurchase(incomingTrade, out purchase);
    }
}