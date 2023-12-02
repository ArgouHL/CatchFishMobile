using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "NewPlayer")]
public class PlayerData : ScriptableObject
{
    public  string player_Name;
    public int Player_Money;
    public int Player_Can;
    public int Player_Energy;
    public int Player_MaxEnergy;
    public bool UseForTest;
    public HashSet<string> unLockedFishs;
    public int Player_Exp;
    public string currentSkin;

    internal void NewData(string name)
    {
        player_Name = name;
        Player_Money = 0;
        Player_Can = 0;
        Player_Energy = 10;
        Player_MaxEnergy = 10;
        Player_Exp = 0;
        unLockedFishs = new HashSet<string>();
        unLockedFishs.Add("16");
        currentSkin = "01";
    }

    internal void GetMoneyAndExp(int money,int exp)
    {
        Player_Money += money;
        Player_Exp += exp;
    }
    internal void AddUnlockedFish(string fishID)
    {
        unLockedFishs.Add(fishID);
    }

    internal void AddUnlockedFish(string[] fishIDs)
    {
        foreach(var fishID in fishIDs)
        {
            AddUnlockedFish(fishID);
        }
      
    }

    internal void Reverse(Account ac)
    {
        
        player_Name = ac.Player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_Exp = ac.Player_Exp;
        unLockedFishs = ac.unLockedFishs;
        currentSkin = ac.currentSkin;
        if (UseForTest)
        {
            Player_Money = 1000000; 
            Player_Can = 99999;
            Player_Energy = 1000;
            Player_MaxEnergy = 1000;
            Player_Exp = 1000;

        }
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
    public HashSet<string> unLockedFishs;
    public string currentSkin;


    internal Account(PlayerData ac)
    {
        Player_Name = ac.player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_Exp = ac.Player_Exp;
        unLockedFishs = ac.unLockedFishs;
        currentSkin = ac.currentSkin;
        if (ac.UseForTest)
        {
            Player_Money = 1000000;
            Player_Can = 99999;
            Player_Energy = 1000;
            Player_MaxEnergy = 1000;
            Player_Exp = 1000;
        }
    }

   
}


