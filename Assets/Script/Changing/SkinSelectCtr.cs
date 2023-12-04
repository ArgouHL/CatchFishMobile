using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinSelectCtr : MonoBehaviour
{
    public static SkinSelectCtr instance;
    public List<SkinBtn> skinBtns;
    public RectTransform[] normalBtnRects;
    public RectTransform[] magicBtnRects;
    public RectTransform skinBtnPre;
    public List<string> skins;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateAllBtn();
    }

    private void GenerateAllBtn()
    {
        skinBtns = new List<SkinBtn>();
        skins = new List<string>(PlayerDataControl.instance.playerData.unLockedSkins).OrderBy(x => x).ToList();

        List<string> iDsNormal = skins.Where(x => SkinController.instance.GetSkin(x).skinType == SkinType.Normal).ToList();
        List<string> iDsMagic = skins.Where(x => SkinController.instance.GetSkin(x).skinType == SkinType.Magic).ToList();

        for (int i = 0; i < iDsNormal.Count; i++)
        {
            RectTransform btnRect = Instantiate(skinBtnPre, normalBtnRects[i]);
            btnRect.anchoredPosition = Vector2.zero;
            var btn = btnRect.GetComponent<SkinBtn>();
            btn.SetButtonInfo(iDsNormal[i]);
            if(iDsNormal[i]== PlayerDataControl.instance.playerData.currentSkin)
                btn.Seletable(false);
            skinBtns.Add(btn);

        }


    }

    
   


    internal void SelectSkin(string skinID)
    {
        PlayerDataControl.instance.playerData.currentSkin = skinID;
        CatSkinShow.instance.ChangeSkin(skinID);
        foreach( var btn in skinBtns)
        {
            btn.Seletable(true);
        }
        PlayerDataControl.instance.Save();
        PlayerDataControl.instance.Load();
    }


}
