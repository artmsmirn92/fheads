using mazing.common.Runtime.Managers.IAP;
using UnityEngine.Events;
using YG;

namespace Common.Managers.IAP
{
    public class YandexGameIapShopManager : ShopManagerBase
    {
        public override void RestorePurchases() { }

        public override void Purchase(int _Key) { }

        public override bool RateGame()
        {
            YandexGame.ReviewShow(false);
            return true;
        }

        public override IAP_ProductInfo GetItemInfo(int _Key) => null;

        public override void AddPurchaseAction(int _ProductKey, UnityAction _Action) { }

        public override void AddDeferredAction(int _ProductKey, UnityAction _Action) { }
    }
}