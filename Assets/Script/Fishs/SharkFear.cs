using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkFear : MonoBehaviour
{
    List<Fish> inrRange= new List<Fish>();
    Coroutine fearCoro;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Fish"))
            return;
        inrRange.Add(collision.transform.GetComponentInParent<Fish>());


        //Debug.Log(collision.transform.gameObject.name);
        //collision.transform.GetComponentInParent<Fish>().Fear();
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Fish"))
            return;
        inrRange.Remove(collision.transform.GetComponentInParent<Fish>());


        //Debug.Log(collision.transform.gameObject.name);
        //collision.transform.GetComponentInParent<Fish>().Fear();
    }

    private IEnumerator FearIE()
    {
        while(true)
        {
            foreach (var f in inrRange)
                f.Feared();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Start()
    {
        fearCoro = StartCoroutine(FearIE());
    }

    private void OnDestroy()
    {
        StopCoroutine(fearCoro);
    }
}
