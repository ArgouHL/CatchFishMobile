using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopFunc 
{
 public static bool PurchaseSkinWithMoney(int price,string SkinID)
    {
        if (PlayerDataControl.instance.playerData.Player_Coin < price)
            return false;
        if (!PlayerDataControl.instance.playerData.AddUnlockSkin(SkinID))
            PlayerDataControl.instance.playerData.Player_Coin -= price;
        PlayerDataControl.instance.Save();
        return true;

    }

    public static bool PurchaseSkinWithCan(int price, string SkinID)
    {
        if (PlayerDataControl.instance.playerData.Player_Can < price)
            return false;
        if (!PlayerDataControl.instance.playerData.AddUnlockSkin(SkinID))
            PlayerDataControl.instance.playerData.Player_Can -= price;
        PlayerDataControl.instance.Save();
        return true;
    }

    public static bool PurchaseShockWithMoney(int price, int count)
    {
        if (PlayerDataControl.instance.playerData.Player_Coin < price)
            return false;
        PlayerDataControl.instance.playerData.shockItemCount += count;
        PlayerDataControl.instance.Save();
        return true;
    }


    public static bool PurchaseShockWithCan(int price, int count)
    {
        if (PlayerDataControl.instance.playerData.Player_Can < price)
            return false;
        PlayerDataControl.instance.playerData.shockItemCount += count;
        PlayerDataControl.instance.Save();
        return true;
    }

    public static bool PurchaseEnergyWithCan(int price, int count)
    {
        if (PlayerDataControl.instance.playerData.Player_Can < price)
            return false;
       // PlayerDataControl.instance.playerData.ShockItemCount += count;
        PlayerDataControl.instance.Save();
        return true;
    }
}
