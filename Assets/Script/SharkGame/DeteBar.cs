using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeteBar : MonoBehaviour
{

    public static DeteBar instance;
    [SerializeField] private RectTransform area,successbar;
    [SerializeField] private Slider detebar;
    [SerializeField] private CanvasGroup dete, button;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dete.alpha = 0;
        button.interactable = false;
    }
    

    public void ShowBar(float success, float total)
    {
        SetBar(success, total);
        dete.alpha = 1;
        button.interactable = false;
    }

    internal void StartDete()
    {
        button.interactable = true;
    }
    internal void StopDete()
    {
        dete.alpha = 0;
        button.interactable = false;
    }

    internal void UpdateBar(float value)
    {
        detebar.value = value;
    }

    void SetBar(float success, float total)
    {
        detebar.value = 0;
        successbar.sizeDelta = new Vector2((success / total) * area.rect.width, successbar.rect.height);
        detebar.maxValue = total;
    }

    internal void ShockDisable()
    {
        button.interactable = false;
    }
}
