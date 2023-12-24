using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.UIElements;

public class ResultShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasGroup title;
    [SerializeField] private CanvasGroup orderUI;
    [SerializeField] private TMP_Text[] orderTexts;
    [SerializeField] private CanvasGroup[] orderChecks;
    [SerializeField] private CanvasGroup moneyexpUI;
    [SerializeField] private TMP_Text money;
    [SerializeField] private TMP_Text exp;
    [SerializeField] private RectTransform FishType;
    [SerializeField] private Sprite doneIcon, notDoneIcon;
    private List<FishByType> fishByTypes;

    [SerializeField] private RectTransform fishboarder;
    [SerializeField] private CanvasGroup backBtn;
    private int targetIncome;
    private int targetExp;
    [SerializeField]
    private bool test;
    Vector2 screenSize;
    void Start()
    {

        List<FishByType> fakeList;
        if (test)
        {
            fakeList = FakeList();
            fishByTypes = new List<FishByType>(fakeList);
        }
        else
        {
            fishByTypes = new List<FishByType>(ResultRecord.instance.FishByTypeCount());
        }

        AddToPlayer();
        StartCoroutine(PlayResultAni());

    }

    private List<FishByType> FakeList()
    {
        var l = new List<FishByType>();
        l.Add(new FishByType("16", 30));
        return l;
    }

    private void AddToPlayer()
    {
        if (test)
            return;
        targetIncome = ResultRecord.instance.income;
        targetExp = ResultRecord.instance.exp;
        PlayerDataControl.instance.playerData.GetMoneyAndExp(targetIncome, targetExp);      
        foreach(var f in fishByTypes)
        {
            FishData.instance.AddRecord(f.fishID, f.count);
        }
        PlayerDataControl.instance.Save();
    }

    private IEnumerator PlayResultAni()
    {
        yield return FishBox.instance.PlayBoxAni(fishByTypes);


        yield return new WaitForSeconds(2);
        GetAndPrintOrder();
        StartCoroutine(ResultDataShow());
    }

    private IEnumerator ResultDataShow()
    {

        LeanTween.value(0, 1, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            title.alpha = val;
        });
        yield return new WaitForSeconds(1.5f);
        LeanTween.value(0, 1, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            orderUI.alpha = val;
        });
        yield return new WaitForSeconds(2);
        yield return OrderCheckShow();
        yield return new WaitForSeconds(1);
        yield return ShowCatchedFish();
        yield return new WaitForSeconds(1);
        LeanTween.value(0, 1, 0.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            moneyexpUI.alpha = val;
        });
        yield return ShowMoneyAndExp();
    }

    private IEnumerator ShowMoneyAndExp()
    {
        float _income = 0;
        float _exp = 0;
        while (_income < targetIncome)
        {
            _income += 150 * Time.deltaTime;
            money.text = "x" + Mathf.Ceil(_income).ToString();
            yield return null;
        }
        money.text = "x" + targetIncome;
        yield return new WaitForSeconds(1f);
        while (_exp < targetExp)
        {
            _exp += 100 * Time.deltaTime;
            exp.text = "x" + Mathf.Ceil(_exp).ToString();
            yield return null;
        }
        exp.text = "x" + targetExp;


        LeanTween.value(0, 1, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            backBtn.alpha = val;
        })
          .setOnComplete(() => { backBtn.interactable = true; });
    }

    private IEnumerator OrderCheckShow()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i);

            ShowCheck(i);


            yield return new WaitForSeconds(0.5f);
        }

    }

    private void ShowCheck(int i)
    {
        LeanTween.value(0, 1, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            if (ResultRecord.instance.orderIngame[i].isFinished)
                orderChecks[i].gameObject.GetComponent<Image>().sprite = doneIcon;
            else
                orderChecks[i].gameObject.GetComponent<Image>().sprite = notDoneIcon;
            orderChecks[i].alpha = val;
        });
    }

    private IEnumerator ShowCatchedFish()
    {

        int count = fishByTypes.Count;
        int index = 0;
        Vector2 orgPoint = Vector2.zero;
        while (index < count)
        {
            orgPoint.x = 0;
            for (int j = 0; j < 4; j++)
            {
                var f = Instantiate(fishboarder, FishType, false);

                var fq = f.GetComponent<FishSquare>();
                fq.ShowUP(fishByTypes[index]);
                //    f.SetParent(FishType);  
                f.anchoredPosition = orgPoint;
                index++;

                if (index == count)
                    break;
                orgPoint.x += 300;
                yield return new WaitForSeconds(0.1f);
            }
            orgPoint.y -= 300;
        }





    }

    private void GetAndPrintOrder()
    {
        for (int i = 0; i < orderTexts.Length; i++)
        {
            orderTexts[i].text = ResultRecord.instance.orderIngame[i].orderInfo;
        }
    }

    public void BackLobby()
    {
        SceneManager.LoadScene(1);
    }


    private void GetScreenRatio()
    {

        screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
#if UNITY_EDITOR
        screenSize = new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);

#endif

    }
}
