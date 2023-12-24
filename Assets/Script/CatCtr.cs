using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCtr : MonoBehaviour
{
    public static CatCtr instance;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Animator cathcEffect;
   

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GetSkin();
    }

    internal void Atk()
    {
  
        animator.SetTrigger("Attack");
        animator.ResetTrigger("Pre");

    }
    internal void PreAtk()
    {
        Debug.Log("Ptr");
        animator.SetTrigger("Pre");
        animator.ResetTrigger("Attack");
    }

    private void GetSkin()
    {
        PlayerDataControl.instance.Load();
        var skinID = PlayerDataControl.instance.playerData.currentSkin;
        Debug.Log(skinID);
        var _skin = SkinController.instance.GetSkin(skinID);
        sp.sprite = _skin.skinImg;
        animator.runtimeAnimatorController = _skin.overrideController;
        cathcEffect.runtimeAnimatorController = _skin.catchEffectController;


    }

    internal void BackIdle()
    {
        animator.SetTrigger("Idle");
       
    }
}
