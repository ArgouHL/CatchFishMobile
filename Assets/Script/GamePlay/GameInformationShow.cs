using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameInformationShow : MonoBehaviour
{
    public static GameInformationShow instance;

    [SerializeField] private CanvasGroup preCountUI;
    [SerializeField] private TMP_Text preCount;
    [SerializeField] private TMP_Text countDown;
    [SerializeField] private TMP_Text catchedCount;

    private void Awake()
    {
        instance = this;
    }

    public void HidePreCountUI()
    {
        UIHelper.ShowAndClickable(preCountUI, false);
    }

    public void UpdatePreCount(int count)
    {
        preCount.text = count.ToString();
    }

    internal void UpdateCountDown(int count)
    {
        if (count < 0)
            count = 0;
        countDown.text = count.ToString();
    }

    internal void UpdateCatchedCount(int count)
    {
        catchedCount.text = "x"+count.ToString();
    }
}
