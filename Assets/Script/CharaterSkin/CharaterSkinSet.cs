using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCharaterSkin", menuName = "CreatObject/CharaterSkin")]
public class CharaterSkinSet : ScriptableObject
{
    public string SkinID;
    public Sprite skinImg;
    public AnimatorOverrideController overrideController;


}
