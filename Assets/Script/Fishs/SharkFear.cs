using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkFear : MonoBehaviour
{
  //  [SerializeField] List<Fish> inrRange= new List<Fish>();
    Coroutine fearCoro;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("FishMid"))
            return;
        collision.transform.GetComponentInParent<Fish>().Feared(transform.position);
       // AddInRange(collision.transform.GetComponentInParent<Fish>());


        //Debug.Log(collision.transform.gameObject.name);
        //collision.transform.GetComponentInParent<Fish>().Fear();
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (!collision.transform.CompareTag("FishMid"))
        //    return;
        //RemoveInRange(collision.transform.GetComponentInParent<Fish>());

        //Debug.Log(collision.transform.gameObject.name);
        //collision.transform.GetComponentInParent<Fish>().Fear();
    }

    //private IEnumerator FearIE()
    //{
    //    //while(true)
    //    //{
    //    //    foreach (var f in inrRange)
    //    //    {
    //    //        f.Feared(transform.position);
                
    //    //    }
    //    //    inrRange.Clear();
    //    // yield return new WaitForSeconds(0.2f);
    //    //}
    //}

    private void Start()
    {
       // fearCoro = StartCoroutine(FearIE());
    }

    private void OnDestroy()
    {
      //  StopCoroutine(fearCoro);
    }

    public void AddInRange(Fish fish)
    {
        //if (fish.feared)
        //    return;
        //inrRange.Add(fish);
    }

    public void RemoveInRange(Fish fish)
    {
        //if (!inrRange.Contains(fish))
        //    return;
        //inrRange.Remove(fish);
    }
}
