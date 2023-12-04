using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SkinBtn : MonoBehaviour
{
    internal string skinID;
    [SerializeReference] private TMP_Text tempIdText;
    [SerializeReference] private CanvasGroup cover;
    
    internal void SetButtonInfo(string iD)
    {
        skinID = iD;
        tempIdText.text = iD;
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
        Seletable(false);
    }
}
