using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCharaterSkin", menuName = "CreatObject/CharaterSkin")]
public class CharaterSkinSet : ScriptableObject
{
    public string SkinID;
    public string skinName;
    public Sprite skinImg;
    public Sprite skinIcon;
    public AnimatorOverrideController overrideController;
    public SkinType skinType;
    public Sprite shotOutSprite;
    public AnimatorOverrideController catchEffectController;
}

public enum SkinType { Normal,Magic}
