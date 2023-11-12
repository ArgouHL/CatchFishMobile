using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    
    [SerializeField] private Button shock, bait;
    [SerializeField] private float shockTime = 5;
    public void UseShock()
    {
        Debug.Log("UseShock");
        shock.interactable = false;
        GamePlay.instance.StartShockCount();
        FishControl.instance.ShockAllFish(shockTime);
    }
    public void UseBait()
    {
        Debug.Log("UseBait");
        bait.interactable = false;
        GamePlay.instance.StartShockCount();
    }
}

public enum ItemType {Shock, Bait }
