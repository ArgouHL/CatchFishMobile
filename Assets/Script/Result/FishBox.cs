using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class FishBox : MonoBehaviour
{
    public static FishBox instance;
    [SerializeField] private Transform spwanTransform;
    [SerializeField] private GameObject jumpFishPre;
    [SerializeField] private float upSpeed = 35;
    [SerializeField] private float downAcc = -15f;
    [SerializeField] private float maxHorizontalSpeed = 20;
    
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

        yield return SpawnAllJumpFish(fishByTypes);

        List<Coroutine> jumpCoroutines = new List<Coroutine>();
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < fishList.Count; i++)
        {
            var jump = StartCoroutine(FishJumpSingle(i));
            jumpCoroutines.Add(jump);
            float factor = (Mathf.Cos(360 * Mathf.Deg2Rad * (float)i / (float)fishList.Count) + 1) / 2;
            Debug.Log(factor);
            yield return new WaitForSeconds(0.1f+0.5f*factor);
        }
        yield return StartCoroutine(WaitForAllCoroutines(jumpCoroutines));

        Debug.Log("JumpEnd");

    }


    private IEnumerator SpawnAllJumpFish(List<FishByType> fishByTypes)
    {
        fishList = new List<GameObject>();

        foreach(var f in fishByTypes)
        {
            for(int i =0;i<f.count;i++)
            {
                var _f = Instantiate(jumpFishPre, spwanTransform);
                _f.GetComponent<SpriteRenderer>().sprite = FishData.instance.GetFishIcon(f.fishID);
                fishList.Add(_f);
            }
           
        }
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

    private IEnumerator FishJumpSingle(int i)
    {
        var fish = fishList[i];
        float _upSpeed = upSpeed;
        LeanTween.delayedCall(0.5f, () => fish.GetComponent<SpriteRenderer>().sortingOrder = 5);
        way *= -1;
        float horizontalSpeed = Mathf.Pow(Random.Range(0.2f, 1f), 2) * way * maxHorizontalSpeed;
        while (fish.transform.position.y < 18f && fish.transform.position.y > -18f)
        {
            
            _upSpeed += downAcc * Time.deltaTime;
            fish.transform.position += new Vector3(horizontalSpeed, _upSpeed, 0) * Time.deltaTime;
            yield return null;
        }
    }
}
