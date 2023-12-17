using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUICtr : MonoBehaviour
{

    public static ShopUICtr instance;
    [SerializeField] CanvasGroup[] ShopPages;

    [SerializeField] CanvasGroup ShopUI;
    [SerializeField] private int nowIndex = 0;

    public void Awake()
    {
        instance = this;
    }
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
        HideShopUI();
    }
    public void ShowShopUI()
    {
        nowIndex = 0;
        RaisePage(nowIndex);
        UIHelper.ShowAndClickable(ShopUI, true);
        PlayerInputManager.instance.ChangeType(InputType.Shop);

    }
    public void HideShopUI()
    {
        Debug.Log("back");
        UIHelper.ShowAndClickable(ShopUI, false);
        PlayerInputManager.instance.ChangeType(InputType.Lobby);

    }

    public void ToLeft()
    {
        if (nowIndex <= 0)
            return;
        nowIndex--;
        RaisePage(nowIndex);
    }

    public void ToRight()
    {
        if (nowIndex == ShopPages.Length - 1)
            return;
        nowIndex++;
        RaisePage(nowIndex);
    }




    public void RaisePage(int nowIndex)
    {
        Debug.Log("SwipeShopPage");
        foreach (var page in ShopPages)
        {
            if (page != ShopPages[nowIndex])
            {
                UIHelper.ShowAndClickable(page, false);
            }
            else
            {
                UIHelper.ShowAndClickable(ShopPages[nowIndex], true);
                
            }


        }
    }

}
