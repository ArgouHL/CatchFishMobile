using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TargetMarkCtr : MonoBehaviour
{
    public static TargetMarkCtr instance;

    [SerializeField] private RectTransform Marker;
    [SerializeField] private CanvasGroup MarkerCanvasGroup;
    [SerializeField] private Image markerImg;
    [SerializeField] private Sprite org,bubble;
    private Transform trackingFish;
    private Coroutine TrackingCoro;
    private Camera cam;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cam = Camera.main;
        switch (SkinController.instance.GetSkin(PlayerDataControl.instance.playerData.currentSkin).skinType)
        {
            case SkinType.Normal:
            default:
                markerImg.sprite = org;
                break;
            case SkinType.Magic:
                markerImg.sprite = bubble;
                break;
        }
            ;
    }

    public void StartTracking(Transform fish)
    {

        trackingFish = fish;
        if (TrackingCoro != null)
            return;
        TrackingCoro = StartCoroutine(TrackingIE());
    }
    private IEnumerator TrackingIE()
    {
        MarkerCanvasGroup.alpha = 1;
        float time = 0;
        while (trackingFish!=null)
        {
            float _scale = (Mathf.Cos(time)+1)/2;
            float scale = Mathf.Lerp(0.9f, 1.2f, _scale);
            Marker.position = cam.WorldToScreenPoint(trackingFish.transform.position);
            Marker.localScale = Vector3.one * scale;
            time += Time.deltaTime*5;
            yield return null;
        }
        MarkerCanvasGroup.alpha = 0;
        TrackingCoro = null;

    }

    internal void StopTracking()
    {
        trackingFish = null;
    }


}
