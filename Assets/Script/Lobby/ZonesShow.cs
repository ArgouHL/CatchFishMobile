using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZonesShow : MonoBehaviour
{
    public static ZonesShow instance;

    [SerializeField] CanvasGroup zone_1UI, zone_2UI, zone_3UI,ui;
    
    private CanvasGroup nowZoneUI;

    public void Awake()
    {
        instance = this;
    }


    public void ShowZone(zone zone)
    {      
        switch (zone)
        {
            case zone.zone_1:
                ShowZone(zone_1UI);
                break;
            case zone.zone_2:
                ShowZone(zone_2UI);
                break;
            case zone.zone_3:
                ShowZone(zone_3UI);
                break;
        }
    }

    private void ShowZone(CanvasGroup zone)
    {
        PlayerInputManager.instance.ChangeType(InputType.None);
        nowZoneUI = zone;
        zone.blocksRaycasts = true;
        zone.alpha = 1;
        ui.blocksRaycasts = true;
        ui.alpha = 1;
        ui.interactable = true;

    }

    public void Hide()
    {
        PlayerInputManager.instance.ChangeType(InputType.Lobby);
        nowZoneUI.blocksRaycasts = false;
        nowZoneUI.alpha = 0;
        ui.blocksRaycasts = false;
        ui.alpha = 0;
        ui.interactable = false;
    }

    public void EnterGame()
    {       
        SceneManager.LoadScene(2);
    }

}

public enum zone { zone_1, zone_2, zone_3 }
