using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkinItems : ItemInterface
{

    private CharaterSkinSet charaterSkinSet;
    private SkinData skinData;


    internal void ShowSkin(SkinData _skinData)
    {
        soldCover.color = new Color(0,0,0,0);
        skinData = _skinData;
        currencyIcon.sprite = ShopSys.instance.GetCurrencyIcon(skinData.currency);
        priceShow.text = skinData.price.ToString();

        charaterSkinSet = SkinController.instance.GetSkin(skinData.skinID);
        icon.sprite = charaterSkinSet.skinIcon;
        nameShow.text = charaterSkinSet.skinName;
        //  icon.sprite = charaterSkinSet.skinIcon;
        UIHelper.ShowAndClickable(canvasGroup, true);
        if (PlayerDataControl.instance.playerData.CheckUnlockSkin(skinData.skinID))
            SkinSold();
    }


    public void ShowBuySure()
    {
        Debug.Log("show");
        ShopSys.instance.ShowBuySure(charaterSkinSet.skinName);
        SureUIctr.sureAction = BuySkin;
    }


    public void BuySkin()
    {
        if (!ShopSys.TakeCurrency(skinData.currency, skinData.price))
        {
            ShopSys.instance.ShowBuyFail();
            return;
        }
        ShopSys.instance.ShowBuySuccess();
        PlayerDataControl.instance.playerData.AddUnlockSkin(skinData.skinID);
        MainUICtr.instance.UpdateShownData();
        SkinSold();
        PlayerDataControl.instance.Save() ;
    }


    public void SkinSold()
    {
        soldCover.sprite = ShopSys.instance.GetSoldIcon();
        soldCover.color = new Color(1, 1, 1, 1);
        canvasGroup.interactable = false;
    }
}
