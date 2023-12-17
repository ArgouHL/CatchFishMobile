using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneFromPoint : MonoBehaviour
{
    public Zone zone;
    public FishReagon reagon;
    public int point;
    public Image lockIcon;
    internal void SetUnlocked(bool b)
    {
        if (b)
            lockIcon.color = new Color(0, 0, 0, 0);
        else
        {
            lockIcon.sprite = SelectMap.instance.GetLockedIcon(b);
            lockIcon.color = Color.white;
        }
            
        
    }
}
