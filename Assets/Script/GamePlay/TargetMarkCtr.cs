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
        float time = 0;
        while (trackingFish!=null)
        {
            float _scale = (Mathf.Cos(time)+1)/2;
            float scale = Mathf.Lerp(0.7f, 1f, _scale);
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
