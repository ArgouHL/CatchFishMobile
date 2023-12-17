using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanItems : ItemInterface
{
    private CanData canData;
    [SerializeField] private TMP_Text itemCountShow;

    internal void ShowItem(CanData _itemData)
    {
        canData = _itemData;
        ShowItem();
    }

    private void ShowItem()
    {
        icon.sprite = ShopSys.instance.GetCanIcon(canData.canCount);
        currencyIcon.sprite = ShopSys.instance.GetCurrencyIcon(currencyType.Money);
        priceShow.text = canData.price.ToString();
        itemCountShow.text = canData.canCount.ToString();
        nameShow.text = canData.canCount.ToString() + "툴툴";

        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    public void ShowBuySure()
    {
        Debug.Log("show");
        ShopSys.instance.ShowBuySure(canData.canCount.ToString() + "툴툴");
        SureUIctr.sureAction = BuyItem;
    }

    public void BuyItem()
    {
        PlayerDataControl.instance.playerData.AddCan(canData.canCount);
        MainUICtr.instance.UpdateShownData();
        ShopSys.instance.ShowBuySuccess();
        // ShopSys.TakeCurrency(canData.currency, canData.price);
        PlayerDataControl.instance.Save();
    }


    public void ItemSold()
    {
        soldCover.color = new Color(0, 0, 0, 0.5f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
