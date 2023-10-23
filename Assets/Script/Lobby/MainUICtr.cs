using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUICtr : MonoBehaviour
{
    [SerializeField] TMP_Text playName_MainUI;

    private void Start()
    {
        UpdateShownData();
    }

    private void UpdateShownData()
    {
        playName_MainUI.text = PlayerDataControl.instance.playerData.player_Name;
    }



    public void ShowShop()
    { 
        ShopUICtr.instance.ShowShopUI(); 
    }

    public void ShowOption()
    {
        OptionUICtr.instance.ShowOptionUI();
    }
}
