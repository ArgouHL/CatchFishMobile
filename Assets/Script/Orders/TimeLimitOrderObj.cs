using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderObj", menuName = "NewOrderObj/TimeLimitCountOrder")]
public class TimeLimitOrderObj : CountOrderObj
{
    public float timeLimit;
}
