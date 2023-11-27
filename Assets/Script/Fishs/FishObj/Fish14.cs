using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish14 : Fish
{
    int updown;
    [SerializeField] Vector3 realdirection;

    internal override IEnumerator SwimToEndPoint(float way)
    {
        int updown = randomIndex > 0 ? 1 : -1;


        float angle = 20 * updown;

        Vector3 direction = new Vector3(-way, 0, 0);
        realdirection = ChangeWay(direction, angle);
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), angle);
        while (transform.position.x * way > -8f)
        {
            if (transform.position.x * way < -6f)
                canbeEat = false;
            float time = 0;
            while (time <= 1)
            {


                transform.position += realdirection * speed * Time.deltaTime;
                time += Time.deltaTime * (speed > 0 ? 1 : 0);
                yield return new WaitWhile(()=>isPause);
                yield return null;

            }
            updown *= -1;
   
            yield return new WaitForSeconds(0.75f);
            yield return new WaitWhile(() => isPause);
            yield return new WaitUntil(() => speed > 0);
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), 40 * updown);
            realdirection = ChangeWay(direction, updown * 20);
            time = 0;

        }

        Dispawn();
    }




}
