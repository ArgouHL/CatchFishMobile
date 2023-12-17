using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTargetSys : MonoBehaviour
{
    public static ShootTargetSys instance;
    [SerializeField] private Transform catOrg;
    [SerializeField] private Transform marker;
    [SerializeField] private float speed = 10;
    [SerializeField] private GameObject tempCatHand;
    [SerializeField] private Color catHandColor;
    private GameObject _catHand;

    private Coroutine markerShooting;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        DragControl.dragoff += ShootMarker;
    }

    private void OnDisable()
    {
        DragControl.dragoff -= ShootMarker;
    }

    private void Start()
    {
        _catHand = Instantiate(tempCatHand, marker);
    }
    public void ShootMarker(float way, float maxDis)
    {
        if (maxDis < 1f)
            return;
        Vector3 startPos = catOrg.position;
        
        markerShooting = StartCoroutine(ShootMarkerIE(way, maxDis));
    }

    private IEnumerator ShootMarkerIE(float angle, float maxDis)
    {
        marker.rotation = Quaternion.Euler(new Vector3(0, 0, 180-angle)) ;
        marker.position = catOrg.position;
        float angleRadians = (angle+180) * Mathf.Deg2Rad;

        // Calculate the components of the vector
        float x = Mathf.Cos(angleRadians);
        float y = Mathf.Sin(angleRadians);

        // Create a normalized Vector2
        var wayVector = new Vector2(y, x).normalized;

       
      //  var aimRotate = Quaternion.AngleAxis(angle, Vector3.forward);
        _catHand.GetComponent<SpriteRenderer>().color = catHandColor;
        marker.GetComponent<MarkerHit>().tracking = true;
        while (Vector3.Distance(marker.position, catOrg.position) < maxDis)
        {
            marker.transform.position += (Vector3)wayVector * speed * Time.deltaTime;
            yield return null;
        }
        MarkerDissapper();
    }

    internal void MarkerDissapper()
    {
        _catHand.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

        marker.GetComponent<MarkerHit>().tracking = false;
    }

    internal void MarkerHitted()
    {
        if (markerShooting != null)
            StopCoroutine(markerShooting);
        MarkerDissapper();
    }
}
