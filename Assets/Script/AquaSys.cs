using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AquaSys : MonoBehaviour
{
    [SerializeField]private Transform aquaPool;


    private void Start()
    {
        SpawnFish();
    }

    private void SpawnFish()
    {
        List<string> fishIDs = PlayerDataControl.instance.playerData.GetColletedFish();
        foreach( var id in fishIDs)
        {
            Vector3 pos = new Vector3(Random.Range(-5f, 5f), Random.Range(2f, 13f), 0);
            var fish = Instantiate(FishData.instance.GetFishObject(id).fishObj,pos,Quaternion.identity, aquaPool);
            fish.GetComponent<Fish>().AquaSwim();
        }

        
    }
}
