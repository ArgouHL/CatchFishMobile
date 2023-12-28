using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    
    [SerializeField] private Button shock, bait;
    [SerializeField] private float shockTime = 5;
    [SerializeField] private float shockItemCD = 10;
    public void UseShock()
    {
        Debug.Log("UseShock");
        shock.interactable = false;
        GamePlay.instance.StartShockCount();
        FishControl.instance.ShockAllFish(shockTime);
        CatchDeter.instance.FishBeShocked();
        SfxControl.instance.ThunderPlay();
        LeanTween.delayedCall(shockItemCD, () => shock.interactable = true);
    }
    public void UseBait()
    {
        Debug.Log("UseBait");
        bait.interactable = false;
        GamePlay.instance.StartShockCount();
    }
}

public enum ItemType {Shock, Bait }
