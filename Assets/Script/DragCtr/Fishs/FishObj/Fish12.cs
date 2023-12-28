using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish12 : Fish
{

    Vector3 direction;
    internal override IEnumerator SwimToEndPoint(float way)
    {
        direction = new Vector3(way, 0, 0);


        bool samelevel = Random.Range(-1, 1) > 0;

      if(samelevel)
            yield return SameLevelMove();
      else
            yield return DiffLevelMove();
        // Generate a random number between 0 and 10
        float angle = (randomIndex - 1) * 25f;

        var centre = transform.position;
        float moveDis = Random.Range(2f, 5);
        yield return ForwardWithDis(direction, moveDis);

       

        
        while (!IsOutScreen())
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return new WaitWhile(() => isPause);
            yield return null;
        }

        Dispawn();
    }


    private void Turn(rotate _rotate, ref Vector3 direction)
    {
        var centre = transform.position;
        float angle = 0;
        switch (_rotate)
        {
            case rotate.Left:
                angle = -90; 
                break;
            case rotate.Right:
                angle = 90;
                break;
        }
        direction = ChangeWay(direction, angle*-way);
        transform.RotateAround(centre, new Vector3(0, 0, 1), angle * -way);
    }

    private IEnumerator ForwardWithDis(Vector3 direction,float dis)
    {
        float movedDis = 0;
        while (movedDis < dis)
        {
            float deltaDis = speed * Time.deltaTime;
            movedDis += deltaDis;
            transform.position += direction * deltaDis;
            yield return new WaitWhile(() => isPause);
            yield return null;
        }
    }
    private IEnumerator SameLevelMove()
    {

        float moveDisDown = Random.Range(2f, 4);
        Turn(rotate.Right, ref direction);
        yield return ForwardWithDis(direction, moveDisDown);

        Turn(rotate.Left, ref direction);
       float moveDis = Random.Range(2f, 6f);
        yield return ForwardWithDis(direction, moveDis);

        Turn(rotate.Left, ref direction);
        yield return ForwardWithDis(direction, moveDisDown);
        Turn(rotate.Right, ref direction);
    }

    private IEnumerator DiffLevelMove()
    {
        int rotateCount = Random.Range(2, 3);
        Debug.Log("RT" + rotateCount);
        for(int i=0;i< rotateCount; i++)
        {
            float moveDisDown = Random.Range(1f, 4);
            Turn(rotate.Right, ref direction);
            yield return ForwardWithDis(direction, moveDisDown);

            Turn(rotate.Left, ref direction);
            float moveDis = Random.Range(2f, 5f);
            yield return ForwardWithDis(direction, moveDis);
           
        }
       
    }


    enum rotate { Left, Right }
}
