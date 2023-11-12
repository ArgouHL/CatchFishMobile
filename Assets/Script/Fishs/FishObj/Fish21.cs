using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish21 : Fish
{
    int updown;
    float amplitude = 4.0f;
    float period = 2f;
    private Vector3 currentPosition;

    internal override IEnumerator SwimToEndPoint(float way)
    {

        int updown = randomIndex > 0 ? 1 : -1;

        Vector3 direction = new Vector3(-way, 0, 0);
        //realdirection = ChangeWay(direction, angle);
        //transform.RotateAround(transform.position, new Vector3(0,0,1), angle);

       float  time = Random.Range(0, period);
        while (transform.position.x * way > -8f)
        {
            if (transform.position.x * way < -6f)
                canbeEat = false;
            float dx = speed * Time.deltaTime;
            float x = time * speed;  // Linear motion along the x-axis
            float y = amplitude * Mathf.Sin(x / period);  // Calculate Y position based on the sine function

            float dydx = (amplitude / period) * Mathf.Cos(x / period);
            Vector3 tangent = new Vector3(1, dydx, 0).normalized;
            var dist = tangent * Time.deltaTime * speed;
            dist.x *= -way;
            transform.position += dist;
            time += Time.deltaTime;
            yield return null;
           

        }
        
        Dispawn();
    }

    


}
