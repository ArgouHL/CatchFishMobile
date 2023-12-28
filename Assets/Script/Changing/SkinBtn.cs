using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkinBtn : MonoBehaviour
{
    internal string skinID;
    [SerializeReference] private Image icon;
    [SerializeReference] private CanvasGroup cover;
    
    internal void SetButtonInfo(CharaterSkinSet set)
    {
        skinID = set.SkinID;
        icon.sprite = set.skinIcon;
    }

    internal void Seletable(bool b)
    {
        if (b)
        {
            cover.alpha = 0;
        }
        else
        {
            cover.alpha = 1;
        }
    }


    public void Click()
    {

        SkinSelectCtr.instance.SelectSkin(skinID);
        SfxControl.instance.ClickPlay();
        Seletable(false);
    }
}
