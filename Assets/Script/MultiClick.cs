using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MultiClick : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float requireTime;
    [SerializeField] private UnityEvent buttonFunction;
    [SerializeField] private int count = 0;

    public Coroutine CheckingCoro;

    public void Click()
    {
        count++;
        if (CheckingCoro == null)
            CheckingCoro = StartCoroutine(MultiClickInTime());
    
        if (count >= requireTime)
        {
            if (CheckingCoro != null)
                StopCoroutine(CheckingCoro);
            CheckingCoro = null;
            buttonFunction.Invoke();
        }

    }


    public IEnumerator MultiClickInTime()
    {
        yield return new WaitForSeconds(duration);       
        count = 0;
        CheckingCoro = null;
        Debug.Log("End");
    }



}
