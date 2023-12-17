using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PageSwape : MonoBehaviour
{
    [SerializeField] private TMP_Text pageNum;
    [SerializeField] private bool showOnStart;
    public CanvasGroup[] pages;
    private int index = 0;

    public void OnEnable()
    {
        SwipeControl.SwipeLeftPage += ToLeft;
        SwipeControl.SwipeRightPage += ToRight;
    }



    public void OnDisable()
    {
        SwipeControl.SwipeLeftPage -= ToLeft;
        SwipeControl.SwipeRightPage -= ToRight;

    }



    private void Start()
    {
        HideAll();
        if(showOnStart)
        {
            ShowPage(0);
        }
        
    }

    private void HideAll()
    {
        foreach (var p in pages)
        {
            UIHelper.ShowAndClickable(p, false);
        }
    }

  
    private void ToRight()
    {
        if (index <= 0)
            return;
        index--;
        ShowPage(index);
    }


    private void ToLeft()
    {
        if (index >= pages.Length - 1)
            return;
        index++;
        ShowPage(index);
    }

    private void ShowPage(int index)
    {
        foreach (var p in pages)
        {
            UIHelper.ShowAndClickable(p, false);
        }
        UIHelper.ShowAndClickable(pages[index], true);
        ShowPageNum(index);
    }

    private void ShowPageNum(int index)
    {
        if (pageNum == null)
            return;
        pageNum.text = (index + 1) + "/" + pages.Length;
    }
}
