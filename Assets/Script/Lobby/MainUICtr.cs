using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUICtr : MonoBehaviour
{
    public static MainUICtr instance;
    [SerializeField] private Button enterGameBtn;
    [SerializeField] TMP_Text playName_MainUI, playMoney_MainUI, playEnergy_MainUI, playCan_UI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateShownData();
        MusicControl.instance.PlayBGM(bgmType.Lobby);
    }

    internal void UpdateShownData()
    {
        playName_MainUI.text = PlayerDataControl.instance.playerData.player_Name;
        playMoney_MainUI.text = PlayerDataControl.instance.playerData.Player_Coin.ToString();
        playCan_UI.text = PlayerDataControl.instance.playerData.Player_Can.ToString();
        playEnergy_MainUI.text = PlayerDataControl.instance.playerData.Player_Energy + "/" + PlayerDataControl.instance.playerData.Player_MaxEnergy;
    }

    internal void SetEnterBtnEnable(bool isEnable)
    {
        enterGameBtn.interactable = isEnable;
        enterGameBtn.gameObject.SetActive(isEnable);
    }

    internal void SetEnterBtnInteractable(bool isEnable)
    {
        enterGameBtn.interactable = isEnable;
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
