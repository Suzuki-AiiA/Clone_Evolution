using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    Coroutine main;
    List<CounterData> counterDatas = new List<CounterData>();
    private void Start()
    {
        main = StartCoroutine(MainCounter());
    }

    public CounterData AddCounter(UnityAction action, float target)
    {
        CounterData data = new CounterData();
        data.action = action;
        data.target = target;
        counterDatas.Add(data);
        return data;
    }
    public void Remove(CounterData data)
    {
        if (counterDatas.Contains(data))
            counterDatas.Remove(data);
    }
    public class CounterData
    {
        public UnityAction action;
        float count = 0;
        public float target;
        public bool CheckDelay()
        {
            count += Time.deltaTime;
            if (count >= target)
            {
                action();
                return true;
            }
            return false;
        }
    }
    IEnumerator MainCounter()
    {
        while (true)
        {
            for (int i = 0; i < counterDatas.Count; i++)
            {
                if (counterDatas[i].CheckDelay())
                {
                    counterDatas.RemoveAt(i);
                    i--;
                }
            }
            yield return 0;
        }
    }
}
