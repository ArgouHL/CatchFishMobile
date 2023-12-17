using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour
{
    public static SelectMap instance;
    [SerializeField] Transform player;
    [SerializeField] private Transform[] points;
    private ZoneFromPoint nowZoneInfo;
    [SerializeField] private Sprite locked, unlocked;




    internal Sprite GetLockedIcon(bool b)
    {
        if (b)
            return unlocked;
        return locked;
    }

    //[SerializeField] private int maxPoint = 2;
    [SerializeField] private int nowIndex = 0;
    [SerializeField] private float swipeTime = 0.5f;

    bool moving = false;
    private int opened = 1;

    private void Awake()
    {
        instance = this;
    }
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
        nowZoneInfo = points[0].GetComponent<ZoneFromPoint>();
        var allZoneInfo = points.Select(x => x.GetComponent<ZoneFromPoint>()).ToArray();
        foreach (var zoneInfo in allZoneInfo)
        {
            if (zoneInfo.point < opened)
            {
                zoneInfo.SetUnlocked(true);
            }
        }
        DrawLine();
    }

    private void DrawLine()
    {
        //points[0].position
        //     points[1].position
    }

    public void ToLeft()
    {
        if (nowIndex <= 0)
            return;
        if (moving) return;
        nowIndex--;
        moving = true;
        var point = points[nowIndex];
        nowZoneInfo = point.GetComponent<ZoneFromPoint>();
        LeanTween.move(player.gameObject, point.position, swipeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => moving = false);
        MainUICtr.instance.SetEnterBtnInteractable(nowIndex >= opened ? false : true);

    }

    public void ToRight()
    {
        if (nowIndex == points.Length - 1)
            return;
        //if (nowIndex == opened - 1)
        //    return;
        if (moving) return;
        moving = true;
        nowIndex++;
        var point = points[nowIndex];
        nowZoneInfo = point.GetComponent<ZoneFromPoint>();
        LeanTween.move(player.gameObject, point.position, swipeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => moving = false);
        MainUICtr.instance.SetEnterBtnInteractable(nowIndex >= opened ? false : true);
    }




    internal void ShowNowZone()
    {
        TempData.instance.targetReagon = nowZoneInfo.reagon;
        ZonesShow.instance.ShowZone(nowZoneInfo.zone);
        MainUICtr.instance.SetEnterBtnEnable(false);
    }
}
public enum Zone { zone_1, zone_2, zone_3, SharkZone_1 }

