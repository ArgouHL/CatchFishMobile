using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish17 : Fish
{
    int _way;
    internal override IEnumerator SwimToEndPoint(float way)
    {

        _way = (int)way;
        // Generate a random number between 0 and 10

        Vector3 direction = new Vector3(way, 0, 0);
       
       // transform.position += new Vector3(way * 1.8f* (indexInGroup>0?0:-1), indexInGroup > 0 ? (3.6f - 2.4f * indexInGroup) : 0, 0);
        
        while (!IsOutScreen())
        {
        
            transform.position += direction * speed*Time.deltaTime;
            yield return new WaitWhile(() => isPause);
            yield return null;
        }
     
        Dispawn();
    }

    internal override void Feared(Vector3 sharkPosition)
    {
        if (feared)
            return;
        if (!gameObject.activeInHierarchy)
            return;
        Debug.Log("Feared" + gameObject.name);
        int way = _way * -1;
        GetComponentInChildren<SpriteRenderer>().flipX = way > 0 ? false : true;
        StartCoroutine(GetFearIE(speeduptime, way));
        feared = true;
    }
}
