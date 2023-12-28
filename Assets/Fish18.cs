using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish18 : Fish
{   
    Vector3 direction;
    internal override IEnumerator SwimToEndPoint(float way)
    {
        direction = new Vector3(way, 0, 0);
        var centre = transform.position;
        float angle;
        if (transform.position.y < -1)
        {
            angle = -15;
        }
        else
            angle = 15;
        direction = ChangeWay(direction, angle * -way);
        transform.RotateAround(centre, new Vector3(0, 0, 1), angle * -way);      
        
        while (!IsOutScreen())
        {
        
            transform.position += direction * speed*Time.deltaTime;
            yield return new WaitWhile(() => isPause);
            yield return null;
        }     
        Dispawn();
    }

    
}
