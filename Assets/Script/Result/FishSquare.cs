using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishSquare : MonoBehaviour
{
    [SerializeField] private RectTransform square;
    [SerializeField] private Image boarder;
    [SerializeField] private Image fishicon;
    [SerializeField] private TMP_Text countText;

    internal void ShowUP(FishByType t)
    {
        ChangeData(t);
        LeanTween.value(0f, 1, 0.5f).setEase(LeanTweenType.easeInCubic).setOnUpdate((float val) =>
        {
            square.localScale = Vector3.one *val;
        });
    }

    private void ChangeData(FishByType t)
    {
        switch (t.fishObj.rarity)
        {
            case FishRarity.Normal:
                boarder.color = Color.blue;
                break;
            case FishRarity.Rare:
                boarder.color = Color.yellow;
                break;
            case FishRarity.SuperRare:
                boarder.color = new Color(1,0.7f,0,1);
                break;
            case FishRarity.Chest:
                boarder.color = Color.red;
                break;
        }
     //   fishicon.sprite = t.fishObj.fishIcon;
        countText.text ="x"+ t.count.ToString();
    }
}
