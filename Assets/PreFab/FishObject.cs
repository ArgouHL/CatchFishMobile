using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "CreatObject/New Fish")]
public class FishObject : ScriptableObject
{
    public string fishName;
    public GameObject fishObj;
    public int fishID;
    public int hitTimes;
    public float size;
    

}

public enum fishRarity { Normal, Rare, SuperRara ,Special,Shark}
public enum fishReagon { Pacific }
