using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Fish10Shark : Shark
{

    
   

  

    private Vector3 RandomBottom()
    {
        return new Vector3(UnityEngine.Random.Range(0, 100) > 50 ? -8.5f : 8.5f, UnityEngine.Random.Range(-6f, 4f), 5);
    }

    internal override IEnumerator OutScreen()
    {
        canFear = false;
        Debug.Log("OutScreen");
        while (!ToTarget(new Vector3(14, -3, 5), 0.7f))
        {
            yield return null;
        }
    }
}




