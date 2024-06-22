namespace TradingSystem.Profile
{
    public readonly struct CommercialProfile
    {
        public readonly TradeProfile tradeProfile;
        public readonly PurchaseProfile purchaseProfile;

        public CommercialProfile(TradeProfile tradeProfile, PurchaseProfile purchaseProfile)
        {
            this.tradeProfile = tradeProfile;
            this.purchaseProfile = purchaseProfile;
        }
    }
}