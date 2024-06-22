using System;
using System.Collections.Generic;
using TradingSystem.Interest;

namespace TradingSystem.Profile
{
    public readonly struct AccountablePurchaseProfile
    {
        private readonly PurchaseProfile _purchaseProfile;
        private readonly Dictionary<Good, int> _remainingPurchases;
        private readonly Action<Good> _itemDepleted;

        public IReadOnlyDictionary<Good, int> RemainingPurchases => _remainingPurchases;

        public AccountablePurchaseProfile(PurchaseProfile purchaseProfile, Action<Good> itemDepleted)
        {
            _purchaseProfile = purchaseProfile;
            _remainingPurchases = new Dictionary<Good, int>(purchaseProfile.BuyingInterests);
            _itemDepleted = itemDepleted;
        }

        public bool TryPurchase(IncomingTrade incomingTrade, out Purchase purchase)
        {
            purchase = default;

            if (!_remainingPurchases.TryGetValue(incomingTrade.good, out int remainingQuantity)
                || !_purchaseProfile.Prices.TryGetValue(incomingTrade.good, out PurchaseInterest interest))
                return false;

            int clampedIncomingQuantity = Math.Min(incomingTrade.amount, remainingQuantity);
            IncomingTrade clampedIncomingTrade = incomingTrade.WithAmount(clampedIncomingQuantity);

            purchase = new Purchase(clampedIncomingTrade, interest.GetPurchase().ExtrapolatePrice(clampedIncomingTrade));
            int newRemainingQuantity = remainingQuantity - incomingTrade.amount;

            if (newRemainingQuantity <= 0)
            {
                _remainingPurchases.Remove(incomingTrade.good);
                _itemDepleted(incomingTrade.good);
            }
            else
                _remainingPurchases[incomingTrade.good] = newRemainingQuantity;

            return true;    
        }
    }   
}