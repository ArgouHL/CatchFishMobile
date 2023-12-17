using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionFish : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button btn;
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text count;
    [SerializeField] private Image border,fishIcon,shinyEffect;

    private void Start()
    {
        btn.interactable = false;
        shinyEffect.enabled = false;
        UIHelper.ShowAndClickable(canvasGroup, false);
    }

    public void Show(FishColletRecord record)
    {

        setInfo(record);
        UIHelper.ShowAndClickable(canvasGroup, true);
    }


    private void setInfo(FishColletRecord record)
    {
        var fishObject = FishData.instance.GetFishObject(record.fishID);
      

        if (record.count <= 0)
            return;
        border.sprite = CollectionShow.instance.GetBoarder(fishObject.rarity);
        fishIcon.sprite = fishObject.fishIcon;
        fishName.text = fishObject.fishName;
        count.text = record.count + "/30";
        if (record.count < 30)
            return;
        btn.interactable = true;
        shinyEffect.enabled = true;

    }

    public void GetReward()
    {
        CollectionShow.instance.GetReward();
    }
}
