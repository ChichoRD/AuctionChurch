using TradingSystem.Interest;
using TradingSystem.Profile;
using UnityEngine;

namespace TradingSystem.Test
{
    internal class TradingProfileTest : MonoBehaviour
    {
        [SerializeField]
        private TradeProfileFlyweight _tradeProfile;
        private TradeProfile _profile;
        private AccountableTradeProfile _accountableProfile;

        [SerializeField]
        private IncomingTrade.Builder _incomingTestTrade;

        private void Awake()
        {
            _profile = _tradeProfile.Create();
            _accountableProfile = new AccountableTradeProfile(_profile, OnItemDepleted);
        }

        private void OnItemDepleted(Good good)
        {
            Debug.Log($"Item {good} depleted");
        }

        [ContextMenu(nameof(TradeWithIncoming))]
        private void TradeWithIncoming()
        {
            IncomingTrade incomingTrade = _incomingTestTrade.Build();
            bool traded = _accountableProfile.TryTrade(incomingTrade, out Trade trade);

            if (traded)
                Debug.Log($"Trade successful: {incomingTrade} -> {trade}");
            else
                Debug.Log($"Trade failed: {incomingTrade}");
        }

        [ContextMenu(nameof(ShowRemainingTrades))]
        private void ShowRemainingTrades()
        {
            foreach (var buyingGood in _accountableProfile.RemainingTrades)
                Debug.Log($"{buyingGood.Key}: {buyingGood.Value}");
        }
    }
}