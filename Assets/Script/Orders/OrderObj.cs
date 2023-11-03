using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrderObj : ScriptableObject
{
    public string orderName;
    public string orderInfo;
    public int rewardMoney;
    public int rewardExp;

}

public class CountOrderObj : OrderObj
{
    public int requirdCount;

}

public class UseItemOrder : OrderObj
{
    public int requirdCount;
    public ItemType requirdItemType;
}

public enum ItemType { None, ElecticShock,Bait}