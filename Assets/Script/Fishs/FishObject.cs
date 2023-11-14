using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "CreatObject/New Fish")]
public class FishObject : ScriptableObject
{
    public string fishName;
    public GameObject fishObj;
    public  string fishID;
    public int hitTimes;
    public float size;
    public FishRarity rarity;
    public FishReagon[] reagon;
    public int income;
    public int exp;
    public float[] startHeights;
    public float speed;
    public float acceleratedSpeed;
    public int numberOfGroup;
    public int randomRoadCount=1;
    public int weights;
    public float speedUpTime = 0.5f;
}



