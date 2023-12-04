using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSkinShow : MonoBehaviour
{
    public static CatSkinShow instance;
    [SerializeField] private Animator ani;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        GetSkin();
    }

    internal void ChangeSkin(string id)
    {
        var skinID = id;
        var _skin = SkinController.instance.GetSkin(skinID);
        ani.runtimeAnimatorController = _skin.overrideController;
    }

    private void GetSkin()
    {
        var skinID = PlayerDataControl.instance.playerData.currentSkin;
        ChangeSkin(skinID);

    }
}
