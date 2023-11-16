using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempData : MonoBehaviour
{
    public static TempData instance;
    internal FishReagon targetReagon;
    private void Awake()    
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
