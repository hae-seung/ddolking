using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class MineMachinePbData
{
    public ItemData oreData;
    public int oilPrice;
}


public class MineMachine
{
    //여기서는 수정가능한 데이터 자료
    private List<MineMachinePbData> oreList;
    
    
    //객체가 채굴하고 나온 광물 결과
    private List<Item> mineResult;
    
    public MineMachine(List<MineMachinePbData> datas)
    {
        oreList = new List<MineMachinePbData>(datas);
        mineResult = new List<Item>();
        CurConcentrationIndex = 0;
        CurMineAmount = 0;
        StoreAmount = 0;
    }

    //현재 집중채굴중인 광물의 인덱스
    public int CurConcentrationIndex { get; private set; }
    public int CurMineAmount { get; private set; }//400 / 400
    public int StoreAmount { get; private set; } // 2000 / 2000

    public List<MineMachinePbData> GetOreList => oreList;
    public List<Item> GetMineResult => mineResult;

    public void ChangeIndex(int idx)
    {
        CurConcentrationIndex = idx;
    }

    public int GetOilPrice()
    {
        return oreList[CurConcentrationIndex].oilPrice;
    }

    public void RemoveItems(int amount)
    {
        StoreAmount -= amount;
        mineResult.RemoveAll(item => item == null);
    }

    public void StartNewMine() => CurMineAmount = 0;
    
    //결과 1개씩 추가
    public void AddOre(Item newOre)
    {
        CurMineAmount++;
        StoreAmount++;

        // CountableItem 타입인지 확인
        if (newOre is not CountableItem newCountable)
        {
            mineResult.Add(newOre);
            return;
        }

        for (int i = 0; i < mineResult.Count; i++)
        {
            if (mineResult[i].itemData.ID == newOre.itemData.ID)
            {
                var exist = (CountableItem)mineResult[i];

                int excess = exist.AddAmountAndGetExcess(newCountable.Amount);
                if (excess > 0)
                {
                    CountableItem extraItem = exist.Clone();
                    extraItem.SetAmount(excess);
                    mineResult.Add(extraItem);
                    //1개씩 추가되는거라 바로 리스트에 추가해도됨.
                    //애초에 excess가 0 아니면 1만 나올거임.
                }

                return; // 찾았으면 바로 종료
            }
        }

        // 없으면 새로 추가
        mineResult.Add(newCountable.Clone());
    }

}














public class MineMachineBehaviour : InteractionBehaviour
{
    //여기서는 절대 수정 불가능한 자료
    [SerializeField] private List<MineMachinePbData> mineMachinePbDatas;
    [SerializeField] private Animator animator;

    private bool isOperate;
    private int mineAmount = 400;
    private int storeAmount = 2000;
    
    //객체로 만들어서 이 안에서 작업
    private MineMachine mineMachine;

    private float nextMineTime = 10f;
    private float timer;

    private int basicPb;
    private int totalPb;
    private Coroutine mineRoutine;
    
    private void Awake()
    {
        mineMachine = new MineMachine(mineMachinePbDatas);
        basicPb = Mathf.RoundToInt(50 / (mineMachinePbDatas.Count - 1));
        totalPb = 50 + basicPb * (mineMachinePbDatas.Count - 1);
        isOperate = false;
    }

    private void OnDisable()
    {
        if(isOperate)
            StopCoroutine(mineRoutine);
    }

    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.OpenMineMachineUI(mineMachine, OperateMachine, RetrieveResult);
    }


    
    //이미 중간에 캐고 있더라도 새로운 광물을 다시 처음부터 캐는것.
    private void OperateMachine()
    {
        StartAnimation();
        mineMachine.StartNewMine();
        
        if(isOperate)
            StopCoroutine(mineRoutine);
        
        mineRoutine = StartCoroutine(MineOre());
    }

    private IEnumerator MineOre()
    {
        isOperate = true;
        
        while(true)
        {
            int ran = Random.Range(3, 10);
            
            timer = 0f;

            // 10초 대기
            while (timer < nextMineTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // ran 개수만큼 채굴
            for (int i = 0; i < ran; i++)
            {
                if (mineMachine.CurMineAmount >= mineAmount ||
                    mineMachine.StoreAmount >= storeAmount)
                {
                    StopAnimation();
                    isOperate = false;
                    mineRoutine = null;
                    yield break; //채굴중지
                }

                int roll = Random.Range(1, totalPb + 1); // 1 ~ totalPb 사이 난수
                int currentPb = 0;

                // 확률 체크
                for (int j = 0; j < mineMachinePbDatas.Count; j++)
                {
                    if (j == mineMachine.CurConcentrationIndex)
                    {
                        currentPb += 50;
                    }
                    else
                    {
                        currentPb += basicPb;
                    }

                    if (roll <= currentPb)
                    {
                        // 해당 광물 채굴 성공 → mineResult에 추가
                        Item ore = mineMachinePbDatas[j].oreData.CreateItem();
                        mineMachine.AddOre(ore);
                        break;
                    }
                }
            }

            // UI 갱신
            UIManager.Instance.UpdateUI(mineMachine);
        }
    }


    //회수
    private void RetrieveResult()
    {
        List<Item> list = mineMachine.GetMineResult;

        int sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            CountableItem citem = list[i] as CountableItem;
            int amount = citem.Amount;//인벤토리에 넣기전 현재 슬롯의 아이템 갯수

            int remain = Inventory.Instance.Add(list[i], amount);//인벤토리에 회수 후 남은 슬롯의 아이템 갯수
            if (remain <= 0)
            {
                list[i] = null;
                sum += amount;
            }
            else
            {
                sum += amount - remain;
                break;
            }
        }

        mineMachine.RemoveItems(sum);
        UIManager.Instance.UpdateUI(mineMachine);

        
        //회수버튼을 누르는데 현재 400개를 다 못캤는데 정지한 경우(==2000개 모두 참) 남은 갯수 다 채우도록 재실행.
        if (!isOperate && mineMachine.CurMineAmount < mineAmount)
        {
            StartAnimation();
            mineRoutine = StartCoroutine(MineOre());
        }
    }



    private void StartAnimation()
    {
        animator.SetBool("make", true);
    }

    private void StopAnimation()
    {
        animator.SetBool("make", false);
    }
    
}
