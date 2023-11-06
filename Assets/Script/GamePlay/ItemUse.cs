using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse : MonoBehaviour
{
    [SerializeField] private Button shock, bait;
    public void UseShock()
    {
        Debug.Log("UseShock");
        shock.interactable = false;
        GamePlay.instance.StartShockCount();
    }
    public void UseBait()
    {
        Debug.Log("UseBait");
        bait.interactable = false;
        GamePlay.instance.StartShockCount();
    }
}

public enum ItemType {Shock, Bait }
