using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class ShopSys : MonoBehaviour
{
    public static ShopSys instance;

    [SerializeField] private Sprite shockIcon;
    [SerializeField] private Sprite energyIcon;
    [SerializeField] private Sprite soldIcon;

  

    [SerializeField] private Sprite coinIcon, canIcon, MoneyIcon;
    [SerializeField] private Sprite smallEnergyIcon, bigEnergyIcon, smallCanIcon, bigCanIcon;
    [SerializeField] private SureUIctr sureUIctr;
    [SerializeField] private SkinItems[] skinItems;
    [SerializeField] private Items[] items;
    [SerializeField] private EnergyItems[] energyItems;
    [SerializeField] private CanItems[] canItems;
    private GoodsDatas itemList;



    private void Awake()
    {
        instance = this;
      
    }

    private void Start()
    {
        BetterStreamingAssets.Initialize();
        LoadAllGoods();
        GetItem();
    }
    
    internal Sprite GetCurrencyIcon(currencyType currency)
    {
        Sprite icon = coinIcon;
        switch (currency)
        {
            case currencyType.Coin:
                icon = coinIcon;
                break;
            case currencyType.Can:
                icon = canIcon;
                break;
            case currencyType.Money:
                icon = MoneyIcon;
                break;

        }
        return icon;
    }

    internal void ShowBuyFail()
    {
        SureUIctr.instance.HideUp();
        SimpleInfoUI.instance.ShowUp("購買失敗");
    }
    internal void ShowBuySuccess()
    {
        SureUIctr.instance.HideUp();
        SimpleInfoUI.instance.ShowUp("購買成功");
    }



    internal Sprite GetSoldIcon()
    {
        return soldIcon;
    }

    internal static bool TakeCurrency(currencyType currency, int price)
    {
    bool result= PlayerDataControl.instance.playerData.TakeCurrency(currency, price);
        if (result)
            SfxControl.instance.CoinPlay();
        return result;
    }

    internal void ShowBuySure(string skinName)
    {
        sureUIctr.ShowUp("確定購買" + skinName);
    }




 

    private void GetItem()
    {
        GetAllSkin();
        GetAllItem();
        GetAllEnergy();
        GetAllCan();
    }

    private void GetAllEnergy()
    {
        for (int i = 0; i < itemList.energysData.Length; i++)
        {
            var _item = itemList.energysData[i];
            energyItems[i].ShowErengyItem(_item);
        }
    }

    private void GetAllItem()
    {
        for (int i = 0; i < itemList.itemsData.Length; i++)
        {
            var _item = itemList.itemsData[i];
            items[i].ShowItem(_item);
        }
    }

    private void GetAllSkin()
    {
    
        for (int i = 0; i < itemList.skinsData.Length; i++)
        {
            var _skindata = itemList.skinsData[i];
           skinItems[i].ShowSkin(_skindata);
        }
    }
    private void GetAllCan()
    {

        for (int i = 0; i < itemList.cansData.Length; i++)
        {
            var canData = itemList.cansData[i];
            canItems[i].ShowItem(canData);
        }
    }

    void LoadAllGoods()
    {
        string jsonString = null;
        jsonString = BetterStreamingAssets.ReadAllText("ItemData/AllItem.json");
        Debug.Log(jsonString);
        itemList = JsonUtility.FromJson<GoodsDatas>(jsonString);
    }

    internal Sprite GetEnergyIcon(int count)
    {
        if (count > 50)
            return bigEnergyIcon;
        return smallEnergyIcon;
    }
    internal Sprite GetCanIcon(int count)
    {
        if (count > 50)
            return bigCanIcon;
        return smallCanIcon;
    }

    internal ShipData[] GetShipDatas()
    {
        return itemList.shipData;
    }
}

[Serializable]
public class GoodsDatas
{
    public SkinData[] skinsData;
    public ItemData[] itemsData;
    public EnergyData[] energysData;
    public CanData[] cansData;
    public ShipData[] shipData;

}

[Serializable]
public class SkinData
{
    public string skinID;
    public currencyType currency;
    public int price;
}

[Serializable]
public class ItemData
{
    public string itemName;
    public int itemCount;
    public itemType type;
    public currencyType currency;
    public int price;
}

[Serializable]
public class EnergyData
{
    public string energyItemName;
    public int energy;
    public currencyType currency;
    public int price;
}


[Serializable]
public class CanData
{
    public int canCount;
    public int price;
}



[Serializable]
public class ShipData
{
    public string shipID;
    public string shipName;
    public int price;

   
}
public enum currencyType { Coin, Can, Money }
public enum itemType { PaidShock ,FreeShock}
