using System;
using UnityEngine;

namespace TradingSystem.Interest
{
    public readonly struct IncomingTrade
    {
        public readonly Good good;
        public readonly int amount;

        public IncomingTrade(Good good, int amount)
        {
            this.good = good;
            this.amount = amount;
        }

        public IncomingTrade WithAmount(int amount) =>
            new IncomingTrade(good, amount);

        public IncomingTrade WithGood(Good good) =>
            new IncomingTrade(good, amount);

        override public string ToString()
        {
            return $"IncomingTrade: {good} x{amount}";
        }

        [Serializable]
        public struct Builder
        {
            public Good good;
            [Min(0)]
            public int amount;

            public Builder(Good good, int amount)
            {
                this.good = good;
                this.amount = amount;
            }

            public readonly IncomingTrade Build() =>
                new IncomingTrade(good, amount);
        }
    }
}