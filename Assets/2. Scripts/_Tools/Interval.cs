using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interval : MonoBehaviour
{
    Coroutine main;
    List<IntervalData> IntervalDatas = new List<IntervalData>();
    private void Start()
    {
        main = StartCoroutine(MainCounter());
    }

    public IntervalData AddInterval(UnityAction action, float target)
    {
        IntervalData data = new IntervalData();
        data.action = action;
        data.target = target;
        action();
        IntervalDatas.Add(data);
        return data;
    }
    public class IntervalData
    {
        public UnityAction action;
        float count = 0;
        public float target;
        bool isStop = false;
        public void SetCount(float c){
            count = c;
        }
        public void CheckDelay()
        {
            if (isStop) return;
            count += Time.deltaTime;
            if (count >= target)
            {
                action();
                count = 0;
            }
        }
        public void SetTarget(float value){
            target = value;
        }
        public void Stop()
        {
            isStop = true;
        }
        public void Start()
        {
            isStop = false;
        }
    }
    IEnumerator MainCounter()
    {
        while (true)
        {
            for (int i = 0; i < IntervalDatas.Count; i++)
            {
                IntervalDatas[i].CheckDelay();
            }
            yield return 0;
        }
    }
}
