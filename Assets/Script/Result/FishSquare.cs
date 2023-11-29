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
    [SerializeField] private Sprite normal, rare, super,box;
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
        switch (FishData.instance.GetRarity(t.fishID))
        {
            case FishRarity.Normal:
                boarder.sprite= normal;
                break;
            case FishRarity.Rare:
                boarder.sprite = rare;
                break;
            case FishRarity.SuperRare:
                boarder.sprite = super;
                break;
            case FishRarity.Chest:
                boarder.sprite = box;
                break;
        }
        fishicon.sprite = FishData.instance.GetFishIcon(t.fishID);
        countText.text ="x"+ t.count.ToString();
    }
}
