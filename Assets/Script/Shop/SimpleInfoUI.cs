using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleInfoUI : MonoBehaviour
{
    public static SimpleInfoUI instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text info;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HideUp();
    }
    internal void ShowUp(string v)
    {
        info.text = v;
        UIHelper.ShowAndClickable(canvasGroup, true);
    }

    public void HideUp()
    {
        UIHelper.ShowAndClickable(canvasGroup, false);
    }


}
