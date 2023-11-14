using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Fish10 : Shark
{
    
    internal override IEnumerator EnterIE()
    {
        Vector3 targetPos = new Vector3(0, -3, 5);
        Vector3 spawnPos =  RandomSide();
        transform.position = spawnPos;
        while (!ToTarget(targetPos,4f, speed/2))
        {
            yield return null;
        }
        ChaseFish();
    }
    internal override IEnumerator ChaseFishIE()
    {
        System.Random random = new System.Random();
        while (FishControl.instance.FishsOnScreen.Count>0)
        {        
            var fishs = new List<Fish>(FishControl.instance.FishsOnScreen);
            Fish selectedFish = fishs.OrderBy(order => random.Next()).ToArray()[0];
            while (selectedFish.canbeEat&&!ToTarget(selectedFish.transform.position, 0.2f))
            {                
                yield return null;
            }
            
         //   sharkFear.RemoveInRange(selectedFish);
            selectedFish.Eat();
            
             yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Nofish");
        StopMove();
    }
       
        private Vector3 RandomSide()
    {
       return new Vector3(8.5f, UnityEngine.Random.Range(-6f,-5f),5);
    }

    private Vector3 RandomBottom()
    {
        return new Vector3(UnityEngine.Random.Range(0, 100) > 50 ? -8.5f : 8.5f, UnityEngine.Random.Range(-6f, 4f), 5);
    }

    internal override IEnumerator OutScreen()
    {
        Debug.Log("OutScreen");
        while (!ToTarget(new Vector3(14,-3,5), 0.7f))
        {
            yield return null;
        }
    }
}




