using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerHit : MonoBehaviour
{
    public static MarkerHit instance;
    public delegate void MarkerEvent(Fish fish);
    public static MarkerEvent HitFish;

    internal bool tracking = false;
    private void Awake()
    {
        instance = this;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!tracking)
            return;
        if (collision.collider.CompareTag("Fish"))
        {            
            var f = collision.collider.GetComponentInParent<Fish>();
            
            HitFish.Invoke(f);
        }
        ShootTargetSys.instance.MarkerHitted();

    }
}
