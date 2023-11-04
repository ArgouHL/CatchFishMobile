using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderObj", menuName = "NewOrderObj/ReagonLimitOrder")]
public class ReagonLimitOrderObj : CountOrderObj
{
    public FishReagon reagon;
    public FishRarity rarityRequired;
}
