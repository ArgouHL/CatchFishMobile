using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class FishBox : MonoBehaviour
{
    public static FishBox instance;
    [SerializeField] private Transform spwanTransform;
    [SerializeField] private GameObject jumpFishPre;
    [SerializeField] private float upSpeed = 35;
    [SerializeField] private float downAcc = -15f;
    [SerializeField] private float maxHorizontalSpeed = 20;
    [SerializeField]
    private SpriteRenderer[] box;
    [SerializeField] private Animator boxAni;
    private List<FishByType> fakefishByTypes;

    private int way = 1;
    public List<GameObject> fishList;

    public Sprite testIcon;


    private void Awake()
    {
        instance = this;
    }

 

    internal IEnumerator PlayBoxAni(List<FishByType> fishByTypes)
    {
       
        yield return new WaitForSeconds(1);
        yield return SpawnAllJumpFish(fishByTypes);
        
        
        float dissTime = 1;
        LeanTween.value(0, 1, dissTime).setOnUpdate((float val) =>
          {
              foreach(var b in box)
              {
                  b.color = new Color(1, 1, 1, val);
              }
          });
        yield return new WaitForSeconds(dissTime);
        yield return new WaitForSeconds(0.5f);
        boxAni.Play("box");
        SfxControl.instance.BoxOpenPlay();
        List<Coroutine> jumpCoroutines = new List<Coroutine>();
        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < fishList.Count; i++)
        {
            Coroutine jumpCoro=null;
            jumpCoro = StartCoroutine(FishJumpSingle(i, jumpCoro));

            jumpCoroutines.Add(jumpCoro);
            float factor = (Mathf.Cos(360 * Mathf.Deg2Rad * Mathf.Lerp(0.3f,1,(float)i / (float)fishList.Count)) + 1) / 2;
            Debug.Log(factor);
            yield return new WaitForSeconds(0.1f+0.5f*factor);  
        }
        yield return new WaitForSeconds(3);

        // yield return WaitForAllCoroutines(jumpCoroutines);
        LeanTween.value(1, 0, dissTime).setOnUpdate((float val) =>
        {
            foreach (var b in box)
            {
                b.color = new Color(1, 1, 1, val);
            }
        });
        yield return new WaitForSeconds(1f);
        Debug.Log("JumpEnd");

    }


    private IEnumerator SpawnAllJumpFish(List<FishByType> fishByTypes)
    {
        var _fishList = new List<GameObject>();

        foreach(var f in fishByTypes)
        {
            for(int i =0;i<f.count;i++)
            {
                var _f = Instantiate(jumpFishPre, spwanTransform);
                _f.GetComponent<SpriteRenderer>().sprite = FishData.instance.GetFishIcon(f.fishID);
                _fishList.Add(_f);
            }
           
        }
        fishList = _fishList.OrderBy(x => Guid.NewGuid()).ToList();
        yield return null;
    }

    internal IEnumerator WaitForAllCoroutines(List<Coroutine> coroutines)
    {
        foreach (var coroutine in coroutines)
        {
            while (coroutine != null)
            {
                yield return null;
            }
        }
    }

    private IEnumerator FishJumpSingle(int i,Coroutine jumpCoro)
    {

        var fish = fishList[i];
        fish.GetComponent<SpriteRenderer>().color = Color.white;
        float _upSpeed = upSpeed;
        LeanTween.delayedCall(0.5f, () => fish.GetComponent<SpriteRenderer>().sortingOrder = 5);
        way *= -1;
        float horizontalSpeed = Mathf.Pow(UnityEngine.Random.Range(0.2f, 1f), 2) * way * maxHorizontalSpeed;
        SfxControl.instance.FishJumpPlay();
        while (fish.transform.position.y < 18f && fish.transform.position.y > -18f)
        {
            
            _upSpeed += downAcc * Time.deltaTime;
            fish.transform.position += new Vector3(horizontalSpeed, _upSpeed, 0) * Time.deltaTime;
            yield return null;
            Debug.Log("soloEnd");
        }
        jumpCoro = null;
    }
}
