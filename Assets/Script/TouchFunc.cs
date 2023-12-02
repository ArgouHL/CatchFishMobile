using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TouchFunc 
{
    public static Fish FindClosestFish(Vector3 touchPos)
    {
        Fish hitFish = null;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit2D[] hit2D = Physics2D.GetRayIntersectionAll(ray);
        if (hit2D.Length == 0)
            return null;
        float mindist = 9999999;

        foreach (var hit in hit2D)
        {
            if (hit.collider.CompareTag("Fish"))
            {
                var _dis = Vector3.Distance(touchPos, hit.collider.transform.position);
                if (_dis < mindist)
                {
                    var f = hit.collider.GetComponentInParent<Fish>();
                    if (f is Shark && f.canInteract)
                    {                      
                        return f;
                    }                        
                       
                    if (f.canbeClick)
                    {
                        mindist = _dis;
                        hitFish = f;
                    }

                }
            }
        }

        return hitFish;
    }


    public static PPShark FindClosestShark(Vector3 touchPos)
    {
        PPShark hitShark = null;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        Debug.DrawRay(ray.origin, ray.direction * 2000, Color.red,5);
        RaycastHit2D[] hit2D = Physics2D.GetRayIntersectionAll(ray);
        if (hit2D.Length == 0)
            return null;
      

        foreach (var hit in hit2D)
        {
            if (hit.collider.CompareTag("Shark"))
            {
                Debug.Log(hit.collider.transform.parent.name);
                hitShark = hit.collider.GetComponentInParent<PPShark>();
                return hitShark;
            }

        }
        return null ;

    }



}
