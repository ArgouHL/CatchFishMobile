using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUICtr : MonoBehaviour
{
    [SerializeField] TMP_Text playName_MainUI, playMoney_MainUI, playEnergy_MainUI, playCan_UI;

    private void Start()
    {
        UpdateShownData();
        MusicControl.instance.PlayBGM(bgmType.Lobby);
    }

    private void UpdateShownData()
    {
        playName_MainUI.text = PlayerDataControl.instance.playerData.player_Name;
        playMoney_MainUI.text = PlayerDataControl.instance.playerData.Player_Money.ToString();
        playEnergy_MainUI.text = PlayerDataControl.instance.playerData.Player_Energy + "/" + PlayerDataControl.instance.playerData.Player_MaxEnergy;
    }



    public void ShowShop()
    { 
        ShopUICtr.instance.ShowShopUI(); 
    }

    public void ShowOption()
    {
        OptionUICtr.instance.ShowOptionUI();
    }

    public void EnterZone()
    {
        SelectMap.instance.ShowNowZone();
    }
}
