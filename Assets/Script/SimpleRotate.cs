using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    private float r = 0;
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, r);
        r += 10;
    }
}
