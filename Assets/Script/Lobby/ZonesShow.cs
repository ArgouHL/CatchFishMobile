using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonesShow : MonoBehaviour
{
    public static ZonesShow instance;

    [SerializeField] CanvasGroup zone_1, zone_2, zone_3,ui;
    private CanvasGroup nowZone;

    public void Awake()
    {
        instance = this;
    }


    public void ShowZone(zone zone)
    {      
        switch (zone)
        {
            case zone.zone_1:
                ShowZone(zone_1);
                break;
            case zone.zone_2:
                ShowZone(zone_2);
                break;
            case zone.zone_3:
                ShowZone(zone_3);
                break;
        }
    }

    private void ShowZone(CanvasGroup zone)
    {
        PlayerInputManager.instance.ChangeType(InputType.None);
        nowZone = zone;
        zone.blocksRaycasts = true;
        zone.alpha = 1;
        ui.blocksRaycasts = true;
        ui.alpha = 1;
        ui.interactable = true;

    }

    public void Hide()
    {
        PlayerInputManager.instance.ChangeType(InputType.Lobby);
        nowZone.blocksRaycasts = false;
        nowZone.alpha = 0;
        ui.blocksRaycasts = false;
        ui.alpha = 0;
        ui.interactable = false;
    }

    public void EnterGame()
    {
            
    }

}

public enum zone { zone_1, zone_2, zone_3 }
