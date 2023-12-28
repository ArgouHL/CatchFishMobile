using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "NewPlayer")]
public class PlayerData : ScriptableObject
{
    public  string player_Name;
    public int Player_Coin;
    public int Player_Can;
    public int Player_Energy;
    public int Player_MaxEnergy;
    public bool UseForTest;
    public Dictionary<string,FishColletRecord> fishCollection;

    

    public List<string> unLockedSkins;
    public int Player_Exp;
    public string currentSkin;
    public int shockItemCount;
    public int freeShockItemCount;
    public string currentShip;
    public List<string> unLockedShip;

    internal void NewData(string name)
    {
        player_Name = name;
        Player_Coin = 0;
        Player_Can = 0;
        Player_Energy = 100;
        Player_MaxEnergy = 100;
        Player_Exp = 0;
        fishCollection = new Dictionary<string, FishColletRecord>();
        unLockedSkins = new List<string>() { "01" };
        //unLockedFishs.Add("16");
       // unLockedSkins.Add("01");
        currentSkin = "01";
        shockItemCount = 0;
        freeShockItemCount = 0;
        currentShip = "01";
        unLockedShip =new List<string>() { "01" };
        if (UseForTest)
        {
            Player_Coin = 100000;
            Player_Can = 100;
            Player_Exp = 1000;
            shockItemCount = 100;
        }
    }

    internal void AddCan(int canCount)
    {
        Player_Can += canCount;
    }

    internal void AddEnergy(int energy)
    {
        Player_Energy += energy;
    }

    internal void GetMoneyAndExp(int money,int exp)
    {
        Player_Coin += money;
        Player_Exp += exp;
    }

    internal void AddItemCount(itemType type, int itemCount)
    {
        switch (type)
        {
            case itemType.PaidShock:
                shockItemCount += itemCount;
                break;
            case itemType.FreeShock:
                freeShockItemCount += itemCount;
                break;
            default:
                break;
        }
    }

    internal void GetCollectReward(string fishID)
    {
        fishCollection[fishID].GetReward();
    }

    internal bool TakeCurrency(currencyType currency, int price)
    {
        bool succ = false;
        switch (currency)
        {
            case currencyType.Coin:
                succ= TakeCurrency(ref Player_Coin, price);
                break;
            case currencyType.Can:
                succ = TakeCurrency(ref Player_Can, price);
                break;
            case currencyType.Money:
                break;
            default:
                break;
        }
        return succ;
    }

    private bool TakeCurrency(ref int currency, int price)
    {
        bool succ = false;
        if(currency>= price)
        {
            succ = true;
            currency -= price;
        }
        return succ;
    }



    internal void AddCurrency(currencyType currency, int addCount)
    {
        switch (currency)
        {
            case currencyType.Coin:
                Player_Coin += addCount;
                break;
            case currencyType.Can:
                Player_Can += addCount;
                break;
            case currencyType.Money:
                break;
            default:
                break;
        }
    }

    internal bool AddUnlockSkin(string skinID)
    {
        if (unLockedSkins.Contains(skinID))
            return true;
        else
        {
            unLockedSkins.Add(skinID);
            return false;
        }
    }

    internal bool CheckUnlockSkin(string skinID)
    {
        if (unLockedSkins.Contains(skinID))
            return true;
        else                   
            return false;
        
    }
    internal void Reverse(Account ac)
    {
        unLockedSkins = ac.unLockedSkins;
        player_Name = ac.Player_Name;
        Player_Coin = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_Exp = ac.Player_Exp;
        fishCollection = ac.fishCollection;
        currentSkin = ac.currentSkin;
        shockItemCount = ac.ShockItemCount;
        currentShip = ac.currentShip;
        unLockedShip = ac.unLockedShip;
        freeShockItemCount = ac.freeShockItemCount;
    }


    internal List<string> GetColletedFish()
    {
        return fishCollection.Where(x => x.Value.hasGetReward).Select(x => x.Key).ToList();
    }

    internal bool UseEnergy(int e)
    {
        if (Player_Energy - e < 0)
            return false;
        Player_Energy -= e;
        return true;
    }
}
[System.Serializable]
public class Account
{
    internal string Player_Name;
    internal int Player_Money;
    internal int Player_Can;
    internal int Player_Energy;
    internal int Player_Exp;
    internal int Player_MaxEnergy;
    public Dictionary<string, FishColletRecord> fishCollection;
    public List<string> unLockedSkins;
    public string currentSkin;
    public int ShockItemCount;
    public string currentShip;
    public List<string> unLockedShip;
    public int freeShockItemCount;
    internal Account(PlayerData ac)
    {
        unLockedSkins=ac.unLockedSkins;
        Player_Name = ac.player_Name;
        Player_Money = ac.Player_Coin;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_Exp = ac.Player_Exp;
        fishCollection = ac.fishCollection;
        currentSkin = ac.currentSkin;
        ShockItemCount = ac.shockItemCount;
        currentShip = ac.currentShip;
        unLockedShip = ac.unLockedShip;
        freeShockItemCount = ac.freeShockItemCount;

    }

   
}


