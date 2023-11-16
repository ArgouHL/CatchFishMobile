using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMap : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] private Transform[] points;
    //[SerializeField] private int maxPoint = 2;
    [SerializeField] private int nowIndex = 0;
    [SerializeField] private float swipeTime = 0.5f;
    bool moving = false;
    private void OnEnable()
    {
        SwipeControl.SwipeLeft += ToRight;
        SwipeControl.SwipeRight += ToLeft;
    }
    private void OnDisable()
    {
        SwipeControl.SwipeLeft -= ToRight;
        SwipeControl.SwipeRight -= ToLeft;
    }

    public void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.Lobby);
        player.position = points[0].position;
    }

    public void ToLeft()
    {
        if (nowIndex <= 0)
            return;
        if (moving) return;
        nowIndex--;
        moving = true;
        LeanTween.move(player.gameObject, points[nowIndex].position, swipeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => moving = false);
    }

    public void ToRight()
    {
        if (nowIndex == points.Length - 1)
            return;

        if (moving) return;
        moving = true;
        nowIndex++;
        LeanTween.move(player.gameObject, points[nowIndex].position, swipeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => moving = false);
    }


    public void ShowZone_1()
    {
        TempData.instance.targetReagon = FishReagon.Pacific;
        ZonesShow.instance.ShowZone(zone.zone_1);
    }
    public void ShowZone2()
    {
        TempData.instance.targetReagon = FishReagon.Indian;
        ZonesShow.instance.ShowZone(zone.zone_2);
    }
    public void ShowZone3()
    {
        TempData.instance.targetReagon = FishReagon.Atlantic;
        ZonesShow.instance.ShowZone(zone.zone_2);
    }

    
}
public enum Zone { zone_1, zone_2, zone_3 }

