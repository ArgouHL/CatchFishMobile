using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishUnlockCtr : MonoBehaviour
{
    public static FishUnlockCtr instance;

    private void Awake()
    {

        instance = this;
    }


    internal bool CheckIfUnlocked(string fishID)
    {
        return true;
        //if (PlayerDataControl.instance.playerData.UseForTest)
        //    return true;
        //return PlayerDataControl.instance.playerData.unLockedFishs.Contains(fishID);
    }


}
