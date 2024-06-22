using TradingSystem.Interest;
using TradingSystem.Profile;
using UnityEngine;

namespace TradingSystem.Test
{
    internal class TradingProfileTest : MonoBehaviour
    {
        [SerializeField]
        private CommercialProfileFlyweight _tradeProfile;
        private CommercialProfile _profile;
        private AccountableCommercialProfile _accountableProfile;

        [SerializeField]
        private IncomingTrade.Builder _incomingTestTrade;

        private void Awake()
        {
            _profile = _tradeProfile.CreateCommercialProfile();
            _accountableProfile = new AccountableCommercialProfile(_profile, OnTradeItemDepleted, OnPurchaseItemDepleted);
        }

        private void OnTradeItemDepleted(Good good)
        {
            Debug.Log($"Trade Item {good} depleted");
        }

        private void OnPurchaseItemDepleted(Good good)
        {
            Debug.Log($"Purchase Item {good} depleted");
        }

        [ContextMenu(nameof(TradeWithIncoming))]
        private void TradeWithIncoming()
        {
            IncomingTrade incomingTrade = _incomingTestTrade.Build();
            bool traded = _accountableProfile.TryDeal(incomingTrade, out Trade trade);

            if (traded)
                Debug.Log($"Trade successful: {incomingTrade} -> {trade}");
            else
                Debug.Log($"Trade failed: {incomingTrade}");
        }

        [ContextMenu(nameof(PurchaseWithIncoming))]
        private void PurchaseWithIncoming()
        {
            IncomingTrade incomingTrade = _incomingTestTrade.Build();
            bool purchased = _accountableProfile.TryDeal(incomingTrade, out Purchase purchase);

            if (purchased)
                Debug.Log($"Purchase successful: {incomingTrade} -> {purchase}");
            else
                Debug.Log($"Purchase failed: {incomingTrade}");
        }

        [ContextMenu(nameof(ShowRemainingTrades))]
        private void ShowRemainingTrades()
        {
            foreach (var buyingGood in _accountableProfile.RemainingTrades)
                Debug.Log($"{buyingGood.Key}: {buyingGood.Value}");
        }

        [ContextMenu(nameof(ShowRemainingPurchases))]
        private void ShowRemainingPurchases()
        {
            foreach (var buyingGood in _accountableProfile.RemainingPurchases)
                Debug.Log($"{buyingGood.Key}: {buyingGood.Value}");
        }
    }
}