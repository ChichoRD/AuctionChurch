using System.Collections.Generic;

namespace TradingSystem.Interest
{
    public readonly struct TradeInterest
    {
        public readonly GoodInterest buyingGood;
        private readonly GoodInterest[] _sellingGoods;

        public TradeInterest(GoodInterest buyingGood, GoodInterest[] sellingGoods)
        {
            this.buyingGood = buyingGood;
            _sellingGoods = sellingGoods;
        }

        public IReadOnlyList<GoodInterest> SellingGoods => _sellingGoods;
        public Trade GetRandomTrade()
        {
            GoodInterest buying = buyingGood;
            GoodInterest selling = _sellingGoods[UnityEngine.Random.Range(0, _sellingGoods.Length)];
            return new Trade(new IncomingTrade(buying.good, buying.GetRandomItemQuantity()), new OutgoingTrade(selling.good, selling.GetRandomItemQuantity()));
        }
    }
}