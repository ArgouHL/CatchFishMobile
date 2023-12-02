using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeteBar : MonoBehaviour
{

    public static DeteBar instance;

    [SerializeField] private Slider detebar;
    [SerializeField] private CanvasGroup dete;
    [SerializeField] private TMP_Text resultText ;
    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        dete.alpha = 0;
   
    }
    

    public void ShowBar(float time)
    {
        resultText.text = "";
        SetBar(time);
        dete.alpha = 1;

    }


    internal void StopDete(bool success)
    {
        if (success)
            resultText.text = "SUCCESS";
        else
            resultText.text = "Fail";
       LeanTween.delayedCall(1,()=> dete.alpha = 0);
    }

    internal void UpdateBar(float value)
    {
        detebar.value = value;
    }

    void SetBar(float time)
    {
        detebar.value = 0;
        detebar.maxValue = time;
    }


}
