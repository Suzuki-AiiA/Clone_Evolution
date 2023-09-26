using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MergedUpdate : MonoBehaviour
{
    List<UpdateData> actions = new List<UpdateData>();
    public int count = 0;
    public UpdateData AddUpdate(UnityAction action)
    {
        UpdateData a = new UpdateData(action);
        actions.Add(a);
        count++;
        return a;
    }
    public void Remove(UpdateData data)
    {
        if (actions.Contains(data))
        {
            actions.Remove(data);
            count--;
        }
    }
    public class UpdateData
    {
        public UnityAction action;
        public UpdateData(UnityAction action)
        {
            this.action = action;
        }

    }
    void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].action();
        }
    }
}
