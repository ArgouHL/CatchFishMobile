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
    public int Player_CatStick;
    public bool UseForTest;


    internal void NewData(string name)
    {
        player_Name = name;
        Player_Money = 0;
        Player_Can = 0;
        Player_Energy = 10;
        Player_MaxEnergy = 10;
        Player_CatStick = 0;
    }

    internal void GetMoney(int money)
    {

    }


    internal void Reverse(Account ac)
    {
        player_Name = ac.Player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_CatStick = ac.Player_CatStick;
    }
}
[System.Serializable]
public class Account
{
    internal string Player_Name;
    internal int Player_Money;
    internal int Player_Can;
    internal int Player_Energy;
    internal int Player_CatStick;
    internal int Player_MaxEnergy;

    internal Account(PlayerData ac)
    {
        Player_Name = ac.player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_MaxEnergy = ac.Player_MaxEnergy;
        Player_CatStick = ac.Player_CatStick;
    }
}


