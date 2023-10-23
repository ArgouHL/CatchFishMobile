using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "NewPlayer")]
public class PlayerData : ScriptableObject
{
    public  string Player_Name;
    public int Player_Money;
    public int Player_Can;
    public int Player_Energy;
    public int Player_CatStick;
    public bool Created;


    internal void NewData(string name)
    {
        Player_Name = name;
        Player_Money = 0;
        Player_Can = 0;
        Player_Energy = 100;
        Player_CatStick = 0;
    }

    internal void GetMoney(int money)
    {

    }


    internal void Reverse(Account ac)
    {
        Player_Name = ac.Player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
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


    internal Account(PlayerData ac)
    {
        Player_Name = ac.Player_Name;
        Player_Money = ac.Player_Money;
        Player_Can = ac.Player_Can;
        Player_Energy = ac.Player_Energy;
        Player_CatStick = ac.Player_CatStick;
    }
}


