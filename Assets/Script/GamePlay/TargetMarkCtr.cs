using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetMarkCtr : MonoBehaviour
{
    public static TargetMarkCtr instance;

    [SerializeField] private RectTransform Marker;
    [SerializeField] private CanvasGroup MarkerCanvasGroup;
    private Fish trackingFish;
    private Coroutine TrackingCoro;
    private Camera cam;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cam = Camera.main;
    }

    public void StartTracking(Fish fish)
    {

        trackingFish = fish;
        if (TrackingCoro != null)
            return;
        TrackingCoro = StartCoroutine(TrackingIE());
    }
    private IEnumerator TrackingIE()
    {
        MarkerCanvasGroup.alpha = 1;
        while (trackingFish!=null)
        {
            Marker.position = cam.WorldToScreenPoint(trackingFish.transform.position);
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
