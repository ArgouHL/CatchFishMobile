using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderObj", menuName = "NewOrderObj/CountOrderObj")]
public class CountOrderObj : OrderObj
{
    public int requirdCount;
    public TypeNeed rule;  
}

public enum TypeNeed { None,SameRarity, DifferentRarity ,SameFish, DifferentFish}