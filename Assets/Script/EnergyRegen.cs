using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRegen : MonoBehaviour
{
    // Start is called before the first frame update

    public static Coroutine regenCoro;

    public static EnergyRegen instance;
    [SerializeField] private float regenCD;


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

    private void Start()
    {
        RegenOffline();
        StartRegen();
    }

    public void StartRegen()
    {
        if (regenCoro != null)
            return;
        regenCoro = StartCoroutine(EnergyRegenIE());
    }

    private IEnumerator EnergyRegenIE()
    {
        while (PlayerDataControl.instance.playerData.Player_Energy < PlayerDataControl.instance.playerData.Player_MaxEnergy)
        {
            yield return new WaitForSeconds(regenCD);
            PlayerDataControl.instance.playerData.Player_Energy++;
            if (MainUICtr.instance != null)
                MainUICtr.instance.UpdateShownData();
            PlayerDataControl.instance.playerData.UpdateLastTime();
            PlayerDataControl.instance.Save();
        }
    }

    



    private void RegenOffline()
    {
        TimeSpan duration = DateTime.Now- PlayerDataControl.instance.playerData.lastEnergyChangeTime;
        var energyToRegen = (float)duration.TotalSeconds / regenCD;
        if(PlayerDataControl.instance.playerData.Player_Energy + energyToRegen > PlayerDataControl.instance.playerData.Player_MaxEnergy)
        {
            PlayerDataControl.instance.playerData.Player_Energy = PlayerDataControl.instance.playerData.Player_MaxEnergy;
        }
        else
        {
            PlayerDataControl.instance.playerData.Player_Energy += (int)energyToRegen;
        }
        if (MainUICtr.instance != null)
            MainUICtr.instance.UpdateShownData();
        PlayerDataControl.instance.playerData.UpdateLastTime();
        PlayerDataControl.instance.Save();
    }
}
