namespace TradingSystem.Interest
{
    public readonly struct Purchase
    {
        public readonly IncomingTrade incomingTrade;
        public readonly float price;

        public Purchase(IncomingTrade incomingTrade, float price)
        {
            this.incomingTrade = incomingTrade;
            this.price = price;
        }
        
        public float UnitPrice => price / incomingTrade.amount;

        public float ExtrapolatePrice(IncomingTrade incomingTrade) =>
            UnitPrice * incomingTrade.amount;

        override public string ToString()
        {
            return $"Purchase: {incomingTrade} for {price}";
        }
    }
}