using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInterface: MonoBehaviour
{
    [SerializeField] internal Image icon;
    [SerializeField] internal Image currencyIcon;
    [SerializeField] internal Image soldCover;
    [SerializeField] internal TMP_Text priceShow;
    [SerializeField] internal TMP_Text nameShow;
    [SerializeField] internal CanvasGroup canvasGroup;
    protected internal void SetPrice(string t)
    {

        var count = t.Length;
        priceShow.text = t;
        priceShow.GetComponent<RectTransform>().sizeDelta = new Vector2(count * 50, 100);

    }

}
