using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ZonesShow : MonoBehaviour
{
    public static ZonesShow instance;

    [SerializeField] CanvasGroup enterGameUI;
    [SerializeField] private TMP_Text infoText;
    private Zone nowZone;

    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIHelper.ShowAndClickable(enterGameUI, false);
    }

    public void ShowZone(Zone zone)
    {
        nowZone = zone;
        switch (zone)
        {
            case Zone.zone_1:
                ShowZone("太平洋");
                break;
            case Zone.zone_2:
                ShowZone("印度洋");
                break;
            case Zone.zone_3:
                ShowZone("大西洋");
                break;



        }
    }

    private void ShowZone(string info)
    {
        PlayerInputManager.instance.ChangeType(InputType.None);
        infoText.text = "確認進入" + info + "?";
        UIHelper.ShowAndClickable(enterGameUI, true);
    
    }


    public void Hide()
    {
        PlayerInputManager.instance.ChangeType(InputType.Lobby);
        UIHelper.ShowAndClickable(enterGameUI, false);
    
        MainUICtr.instance.SetEnterBtnEnable(true);
    }

    public void EnterGame()
    {
        switch (nowZone)
        {
            case Zone.zone_1:
                SceneManager.LoadScene(2);
                break;
            case Zone.zone_2:
               // ShowZone("印度洋");
                break;
            case Zone.zone_3:
            //   ShowZone("大西洋");
                break;
        }
       
    }

 
  
}


