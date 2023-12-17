using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyItems : ItemInterface
{
    private EnergyData energyData;
    [SerializeField] private TMP_Text energyCountShow;

    internal void ShowErengyItem(EnergyData _energyData)
    {
        energyData = _energyData;
        ShowErengyItem();
    }

    private void ShowErengyItem()
    {
        icon.sprite = ShopSys.instance.GetEnergyIcon(energyData.energy);
        currencyIcon.sprite = ShopSys.instance.GetCurrencyIcon(energyData.currency);
        priceShow.text = energyData.price.ToString();
        energyCountShow.text =energyData.energy.ToString();
        nameShow.text = energyData.energy.ToString() + energyData.energyItemName;
        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    public void ShowBuySure()
    {
        Debug.Log("show");
        ShopSys.instance.ShowBuySure(energyData.energy.ToString()+energyData.energyItemName);
        SureUIctr.sureAction = BuyItem;
    }

    public void BuyItem()
    {
        if (!ShopSys.TakeCurrency(energyData.currency, energyData.price))
        {
            ShopSys.instance.ShowBuyFail();
            return;
        }
        ShopSys.instance.ShowBuySuccess();
        PlayerDataControl.instance.playerData.AddEnergy(energyData.energy);       
        MainUICtr.instance.UpdateShownData();
        PlayerDataControl.instance.Save();
    }

}

