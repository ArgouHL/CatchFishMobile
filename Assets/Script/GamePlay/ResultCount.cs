using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCount : MonoBehaviour
{
    public static ResultCount instance;
    int catchedFish = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddCatchedFish(fishType type)
    {
        catchedFish++;
        GameInformationShow.instance.UpdateCatchedCount(catchedFish);
    }


}
