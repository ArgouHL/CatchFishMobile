using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShipObj", menuName = "NewShipObj")]

public class ShipObject : ScriptableObject
{
    public string id;
    public Sprite afterBuyIcon;
    public Sprite beforeBuyIcon;
}
