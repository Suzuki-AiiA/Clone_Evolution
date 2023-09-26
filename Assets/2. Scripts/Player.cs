using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, IPlayerMove
{
    public static Player instance;
    public Transform moveBase;
    public float moveOffset;
    public float moveStartDelay;
    public float moveSpeed;
    public bool isUpdate = false;
    public List<Character> characters = new List<Character>();
    public Interval interval;
    public Interval.IntervalData intervalData;
    public float shootDelay;
    public MergedUpdate mergedUpdate;
    public Counter counter;
    public float lifeTime;
    public Character characterPrefab;
    public float distance;
    public int initCharacterCount;
    public PositionGet getPos;
    public int rowNum;
    bool overInit = false;
    public float mergeDelayTime;
    public int[] targetAges;
    public bool isEnd = false;
    public bool isUpgraded = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        lifeTime = GManager.Data.bulletLifeTime;
        Character c = PoolingManager.instance.characterPooling.GetPoolingObject();
        c.Init(0);
        RePositionCharas();
        moveSpeed = GManager.Data.playerSpeed;
        targetAges = GManager.Data.playerAges;
    }
    private void Update()
    {
        if (!isUpdate) return;
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
    }
    public void GameStart()
    {
        foreach (var item in characters)
        {
            item.MoveStart();
        }
        isUpdate = true;
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].GameStart();
        }
        // counter.AddCounter(() =>
        // {

        //     intervalData = interval.AddInterval(() =>
        //     {
        //         for (int i = 0; i < characters.Count; i++)
        //         {
        //             characters[i].Shoot();
        //         }
        //     }, shootDelay);
        // }, moveStartDelay);
    }
    public void UpgradeWithGate(Gate gate)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].UpgradeGate(gate);
        }
    }
    public void Move(Vector2 dir)
    {
        if (!isUpdate) return;
        float x = Mathf.Clamp(moveBase.localPosition.x + dir.x, -moveOffset, moveOffset);
        moveBase.localPosition = new Vector3(x, 0, 0);
    }
    public Vector3 GetCharacterPos(int num, float distance)
    {
        if (num == 0)
        {
            return Vector3.zero;
        }
        int a = num / 6 + 1;
        int b = (num - 1) % 6;

        return Vector3.zero;
    }

    public void ReadyUpgrade()
    {
        isUpdate = false;
    }
    public void ReadyUPPP()
    {
        // foreach (var item in characters)
        // {
        //     item.MoveStop();
        // }
        // intervalData.Stop();
    }


    public void Resume()
    {
        isUpdate = true;
        isUpgraded = true;
        // foreach (var item in characters)
        // {
        //     item.MoveStart();
        // }
        // intervalData.Start();
    }

    public void RePositionCharas()
    {
        if (characters.Count <= 0)
        {
            return;
        }
        if (characters.Count < rowNum)
        {
            getPos.num1 = characters.Count;
            getPos.width = getPos.lineDis * (characters.Count);
            getPos.Init();
        }

        else
        {
            if (!overInit)
            {
                overInit = true;
                getPos.num1 = rowNum;
                getPos.num2 = rowNum - 1;
                getPos.width = getPos.lineDis * (getPos.num1);
                getPos.Init();
            }
        }
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.DOLocalMove(getPos.GetPos(i + 1) , 0.5f);
        }
    }
    public void GameOver()
    {
        isUpdate = false;
        if (isEnd)
        {
            GManager.instance.GameWin();
        }
        else
        {
            GManager.instance.GameFail();
        }
    }
}
