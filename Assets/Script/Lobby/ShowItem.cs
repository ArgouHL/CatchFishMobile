using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItem : MonoBehaviour
{
    public static ShowItem instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private ItemCountShow free, paid;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        var data = PlayerDataControl.instance.playerData;
        paid.ShowCount(data.shockItemCount);
        free.ShowCount(data.freeShockItemCount);
        UIHelper.ShowAndClickable(canvasGroup, true);
    }
    public void Hide()
    {
        UIHelper.ShowAndClickable(canvasGroup, false);
    }
}
