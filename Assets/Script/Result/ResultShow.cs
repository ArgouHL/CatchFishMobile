using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ResultShow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasGroup orderUI;
    [SerializeField] private TMP_Text[] orderTexts;
    [SerializeField] private CanvasGroup[] orderChecks;
    [SerializeField] private CanvasGroup moneyexpUI;
    [SerializeField] private TMP_Text money;
    [SerializeField] private TMP_Text exp;
    [SerializeField] private RectTransform FishType;

    private List<FishByType> fishByTypes;

    [SerializeField] private RectTransform fishboarder;
    [SerializeField] private CanvasGroup backBtn;

    void Start()
    {
        fishByTypes = new List<FishByType>(ResultRecord.instance.FishByTypeCount());
        StartCoroutine(PlayAni());
        //StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);
        int count = 10;
        int _count = 0;
        Vector2 orgPoint = Vector2.zero;
        while (_count < count)
        {
            orgPoint.x = 0;
            for (int j = 0; j < 4; j++)
            {
                var f = Instantiate(fishboarder);
                var fq = f.GetComponent<FishSquare>();
                // fq.ShowUP(new FishObject());
                f.SetParent(FishType);
                f.anchoredPosition = orgPoint;
                _count++;

                if (_count == count)
                    break;
                orgPoint.x += 300;
                yield return new WaitForSeconds(0.1f);
            }
            orgPoint.y -= 300;
        }

    }

    private IEnumerator PlayAni()
    {
        yield return new WaitForSeconds(2);
        GetAndPrintOrder();
        StartCoroutine(ResultDataShow());
    }


    private IEnumerator ResultDataShow()
    {
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
        var targetIncome = ResultRecord.instance.income;
        var targetExp = ResultRecord.instance.exp;
        float _income = 0;
        float _exp = 0;
        while(_income< targetIncome)
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
            if (ResultRecord.instance.orderIngame[i].isFinished)
            {
                ShowCheck(i);
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

    private void ShowCheck(int i)
    {
        LeanTween.value(0, 1, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
        {
            orderChecks[i].alpha = val;
        });
    }

    private IEnumerator ShowCatchedFish()
    {
        fishByTypes = ResultRecord.instance.FishByTypeCount();
        int count = fishByTypes.Count;
        int index = 0;
        Vector2 orgPoint = Vector2.zero;
        while (index < count)
        {
            orgPoint.x = 0;
            for (int j = 0; j < 4; j++)
            {
                var f = Instantiate(fishboarder);
                var fq = f.GetComponent<FishSquare>();
                fq.ShowUP(fishByTypes[index]);
                f.SetParent(FishType);
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
}
