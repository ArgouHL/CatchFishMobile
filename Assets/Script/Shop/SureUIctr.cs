using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SureUIctr : MonoBehaviour
{

    public static SureUIctr instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text info;

    public delegate void SureEvent();
    public static SureEvent sureAction;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HideUp();
    }

    internal void ShowUp(string infoText)
    {
        info.text = infoText;
        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    internal void HideUp()
    {
        UIHelper.ShowAndClickable(canvasGroup, false);
    }

    public void SureBuy()
    {
        sureAction.Invoke();
        sureAction = null;
        HideUp();
    }

    public void SureCancel()
    {
        sureAction = null;
       HideUp();
    }

}

