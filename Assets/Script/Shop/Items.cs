using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Items : ItemInterface
{
    private ItemData itemData;
    [SerializeField] private TMP_Text itemCountShow;

    internal void ShowItem(ItemData _itemData)
    {
        itemData = _itemData;
        ShowItem();
    }

    private void ShowItem()
    {
        currencyIcon.sprite = ShopSys.instance.GetCurrencyIcon(itemData.currency);
        priceShow.text = itemData.price.ToString();
        itemCountShow.text = "x" + itemData.itemCount.ToString();
        nameShow.text = itemData.itemName;
        //   charaterSkinSet = SkinController.instance.GetSkin(skinData.skinID);
        //  icon.sprite = charaterSkinSet.skinIcon;
        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    public void ShowBuySure()
    {
        Debug.Log("show");
        ShopSys.instance.ShowBuySure(itemData.itemName + "x" + itemData.itemCount.ToString());
        SureUIctr.sureAction = BuyItem;
    }

    public void BuyItem()
    {
        if (!ShopSys.TakeCurrency(itemData.currency, itemData.price))
        {
            ShopSys.instance.ShowBuyFail();
            return;
        }
        
        PlayerDataControl.instance.playerData.AddItemCount(itemData.type, itemData.itemCount);
        ShopSys.instance.ShowBuySuccess();
        MainUICtr.instance.UpdateShownData();
        if (itemData.currency == currencyType.Coin)
            ItemSold();
        PlayerDataControl.instance.Save();
    }


    public void ItemSold()
    {
        soldCover.color = new Color(0, 0, 0, 0.5f);
        canvasGroup.interactable = false;
    }
}

