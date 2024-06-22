namespace TradingSystem.Interest
{
    public readonly struct PurchaseInterest
    {
        public readonly GoodInterest buyingGood;
        private readonly PriceInterest _sellingPrice;

        public PurchaseInterest(GoodInterest buyingGood, PriceInterest sellingPrice)
        {
            this.buyingGood = buyingGood;
            _sellingPrice = sellingPrice;
        }

        public Purchase GetPurchase() =>
            new Purchase(new IncomingTrade(buyingGood.good, buyingGood.GetRandomItemQuantity()), _sellingPrice.GetSellingPrice());

        public override string ToString()
        {
            return $"Buying Good: {buyingGood}, Selling Price: {_sellingPrice}";
        }
    }
}