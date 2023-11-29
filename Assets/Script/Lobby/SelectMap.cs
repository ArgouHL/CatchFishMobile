using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMap : MonoBehaviour
{
    public static SelectMap instance;
    [SerializeField] Transform player;
    [SerializeField] private Transform[] points;
    private ZoneFromPoint nowZoneInfo;
    //[SerializeField] private int maxPoint = 2;
    [SerializeField] private int nowIndex = 0;
    [SerializeField] private float swipeTime = 0.5f;
    bool moving = false;


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
    }

    public void ToRight()
    {
        if (nowIndex == points.Length - 1)
            return;

        if (moving) return;
        moving = true;
        nowIndex++;
        var point = points[nowIndex];
        nowZoneInfo = point.GetComponent<ZoneFromPoint>();
        LeanTween.move(player.gameObject, point.position, swipeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => moving = false);

    }


  

    internal void ShowNowZone()
    {
        TempData.instance.targetReagon = nowZoneInfo.reagon;
        ZonesShow.instance.ShowZone(nowZoneInfo.zone);
    }
}
public enum Zone { zone_1, zone_2, zone_3, SharkZone_1 }

