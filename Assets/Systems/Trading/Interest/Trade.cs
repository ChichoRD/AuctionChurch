using UnityEngine;

namespace TradingSystem.Interest
{
    public readonly struct Trade
    {
        public readonly IncomingTrade incomingTrade;
        public readonly OutgoingTrade outgoingTrade;

        public Trade(IncomingTrade incomingTrade, OutgoingTrade outgoingTrade)
        {
            this.incomingTrade = incomingTrade;
            this.outgoingTrade = outgoingTrade;
        }

        public float TradeRatio => (float)outgoingTrade.amount / incomingTrade.amount;

        public OutgoingTrade Extrapolate(IncomingTrade incomingTrade) =>
            new OutgoingTrade(outgoingTrade.good, Mathf.CeilToInt(incomingTrade.amount * TradeRatio));

        override public string ToString()
        {
            return $"Trade: {incomingTrade} -> {outgoingTrade}";
        }
    }
}