using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSharkGameSet", menuName = "NewSharkGameSet")]
public class SharkGameSet : ScriptableObject
{
    public int hp = 5;
    public GameObject shark;
    public FishReagon reagon;
    public float[] hpThresholds;
}
