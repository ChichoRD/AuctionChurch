namespace TradingSystem.Interest
{
    public readonly struct OutgoingTrade
    {
        public readonly Good good;
        public readonly int amount;

        public OutgoingTrade(Good good, int amount)
        {
            this.good = good;
            this.amount = amount;
        }

        override public string ToString()
        {
            return $"OutgoingTrade: {good} x{amount}";
        }
    }
}