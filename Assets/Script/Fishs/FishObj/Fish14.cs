using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish16 : Fish
{
    internal override void Feared()
    {
        speed = accelerate;
    }
    internal override IEnumerator SwimToEndPoint(float way)
    {
        

        // Generate a random number between 0 and 10
        float angle = (randomIndex-1)*25f;

        var centre = transform.position;
        Vector3 direction = new Vector3(-way, 0, 0);
        Vector3 realdirection = ChangeWay(direction, angle);
        transform.position += new Vector3(way * 1.8f* (indexInGroup>0?0:-1), indexInGroup > 0 ? (3.6f - 2.4f * indexInGroup) : 0, 0);
        transform.RotateAround(centre, new Vector3(0,0,1), angle);       
        while (transform.position.x * way > -8f)
        {
            
            transform.position += realdirection*speed*Time.deltaTime;
            yield return null;
        }
        FishControl.instance.FishOutScreen(this);
        Destroy(gameObject);
    }

}
