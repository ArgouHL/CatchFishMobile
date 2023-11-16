using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkFear : MonoBehaviour
{
    [SerializeField] private Shark shark;
    Coroutine fearCoro;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shark.canFear)
            return;
        if (!collision.transform.CompareTag("FishMid"))
            return;
        collision.transform.GetComponentInParent<Fish>().Feared(transform.position);
   
    }


}
