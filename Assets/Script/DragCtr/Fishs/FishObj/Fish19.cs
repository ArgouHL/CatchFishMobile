using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish19 : Fish
{
    Vector3 direction;
    internal override IEnumerator SwimToEndPoint(float way)
    {
        direction = new Vector3(way, 0, 0);
        var centre = transform.position;
        float angle = 0;
        int ran = Random.Range(1, 3);
        switch (ran)
        {
            case 1:
                angle = -15;
                break;
            case 2:
                angle = 0;
                break;
            case 3:
                angle = 15;
                break;
        }

        direction = ChangeWay(direction, angle * -way);
        transform.RotateAround(centre, new Vector3(0, 0, 1), angle * -way);
        float time = 0;
        var orgspeed = speed;
        while (!IsOutScreen())
        {
            if (time > 0.6f)
            {
                speed = orgspeed;
                time = 0;
            }
            else if (time > 0.4f)
                speed = accelerate;

            
            transform.position += direction * speed * Time.deltaTime;
            yield return new WaitWhile(() => isPause);
            yield return new WaitWhile(() => isShocking);
            time += Time.deltaTime;
            yield return null;
        }
        Dispawn();
    }


}
