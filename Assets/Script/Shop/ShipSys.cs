using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSys : MonoBehaviour
{
    public static ShipSys instance;

    private ShipData[] shipDatas;
    [SerializeField] private ShipItem[] shipItems;
    [SerializeField] private Image ship;
    [SerializeField] private Image shipIcon;
    [SerializeField] private ShipObject[] shipObjects;
    [SerializeField] private CanvasGroup canvasGroup;

    private Dictionary<string, ShipObject> shipSpriteDict;
    private ShipData nowShip;
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        HideUp();
        shipSpriteDict = new Dictionary<string, ShipObject>();
        foreach(var s in shipObjects)
        {
            shipSpriteDict.Add(s.id, s);
        }
        shipDatas = ShopSys.instance.GetShipDatas();
       
        ChangeSkin(PlayerDataControl.instance.playerData.currentShip);
    }

    internal bool CheckUsing(string shipID)
    {
        return PlayerDataControl.instance.playerData.currentShip==shipID;
    }

    internal Sprite GetShipSprite(string shipID)
    {
       if(CheckUnlocked(shipID))
        {
            return shipSpriteDict[shipID].afterBuyIcon;
        }
       else
            return shipSpriteDict[shipID].beforeBuyIcon;
    }

    internal bool CheckUnlocked(string shipID)
    {
        return PlayerDataControl.instance.playerData.unLockedShip.Contains(shipID);
    }

    private void ShowInfos()
    {
        for (int i = 0; i < shipDatas.Length; i++)
        {
            var _shipdata = shipDatas[i];
            shipItems[i].ShowData(_shipdata);
        }
    }

   


    internal void SelectShip(string shipID)
    {
        PlayerDataControl.instance.playerData.currentShip = shipID;
        ChangeSkin(shipID);
        foreach (var btn in shipItems)
        {
            btn.Seleted(false);
        }
        PlayerDataControl.instance.Save();
        PlayerDataControl.instance.Load();
    }

    private void ChangeSkin(string shipID)
    {
        var sprite = GetShipSprite(shipID);
        ship.sprite = sprite;
        shipIcon.sprite = sprite;

    }

    public void ShowUp()
    {
        ShowInfos();
        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    public void HideUp()
    {
        UIHelper.ShowAndClickable(canvasGroup, false);
    }
}
