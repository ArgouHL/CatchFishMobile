using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup usingIcon;
    [SerializeField] private Image shipIcon;
    [SerializeField] private TMP_Text price;
    [SerializeField] private TMP_Text shipName;
    private ShipData shipData;

    internal void ShowData(ShipData _shipdata)
    {        
        shipData = _shipdata;
        shipIcon.sprite = ShipSys.instance.GetShipSprite(shipData.shipID);
        usingIcon.alpha = ShipSys.instance.CheckUsing(shipData.shipID)?1:0;
        shipName.text = shipData.shipName;
        Debug.Log(shipData.price);
        if (shipData.price > 200000)
            SetPrice("???");
        else
            SetPrice(shipData.price.ToString());


    }

    private void SetPrice(string t)
    {
      
        var count = t.Length;
        price.text = t;
        price.GetComponent<RectTransform>().sizeDelta = new Vector2(count *50, 100);

    }

    public void BuyAndUse()
    {
        if (!ShipSys.instance.CheckUnlocked(shipData.shipID))
        {
            ShowBuySure();
        }
        else
            Equied();
    }

    private void Equied()
    {
        ShipSys.instance.SelectShip(shipData.shipID);
        Seleted(true);
    }



    internal void Seleted(bool b)
    {
        if (b)
        {
            usingIcon.alpha = 1;
        }
        else
        {
            usingIcon.alpha = 0;
        }
    }

    public void ShowBuySure()
    {
        Debug.Log("show");
        SureUIctr.instance.ShowUp("ΩT©w¡ ∂R" + shipData.shipName);
        SureUIctr.sureAction = BuyItem;
    }

    public void BuyItem()
    {
        if (!ShopSys.TakeCurrency(currencyType.Coin, shipData.price))
        {
            ShopSys.instance.ShowBuyFail();
            return;
        }
        PlayerDataControl.instance.playerData.unLockedShip.Add(shipData.shipID);
        ShopSys.instance.ShowBuySuccess();
        MainUICtr.instance.UpdateShownData();
        Equied();
        PlayerDataControl.instance.Save();
    }

}
