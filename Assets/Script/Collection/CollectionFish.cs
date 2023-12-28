using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class CollectionFish : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text count;
    [SerializeField] private Image border,fishIcon,shinyEffect;
    private string fishID;

    private void Start()
    {
        btn.interactable = false;
      
     //   UIHelper.ShowAndClickable(canvasGroup, false);
    }

    public void Show(FishColletRecord record)
    {

        setInfo(record);
        UIHelper.ShowAndClickable(canvasGroup, true);
        Debug.Log("Show");
    }


    private void setInfo(FishColletRecord record)
    {
        shinyEffect.enabled = false;
        Debug.Log("setInfo"+ record.fishID);
        fishID = record.fishID;
        var fishObject = FishData.instance.GetFishObject(record.fishID);     

        if (record.count <= 0)
        {
            fishIcon.sprite = CollectionShow.instance.GetUnknow() ;
            return;
        }
           
        border.sprite = CollectionShow.instance.GetBoarder(fishObject.rarity);
        fishIcon.rectTransform.sizeDelta = new Vector2(200, 200);
        fishIcon.sprite = fishObject.fishIcon;
        fishName.text = fishObject.fishName;
        count.text = record.count + "/"+ fishObject.collectionRequire;
        if (record.count < fishObject.collectionRequire|| record.hasGetReward)
        {            
            return;
        }

  

        Debug.Log("Shine");
        btn.interactable = true;
        shinyEffect.enabled = true;
       var s= shinyEffect.GetComponent<ShineEffectCtr>();
        s.GetReward += GetReward;
        s.Shining();
    }

    public void GetReward()
    {       
        PlayerDataControl.instance.playerData.GetCollectReward(fishID);
        CollectionShow.instance.GetReward();
    }
}
