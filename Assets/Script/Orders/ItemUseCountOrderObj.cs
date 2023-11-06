using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderObj", menuName = "NewOrderObj/ItemUseCountOrderObj")]
public class ItemUseCountOrderObj : OrderObj
{
    public int requirdCount;
    public ItemType itemType;
  
}
