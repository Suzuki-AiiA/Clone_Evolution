using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUpdaateManager : MonoBehaviour
{
    [HideInInspector]
    public List<AnimationUpdate> updates = new List<AnimationUpdate>();
    public bool asyncUpdate;
    int count = 0;
    void Update()
    {
        if (asyncUpdate)
        {
            updates.ForEach((AnimationUpdate update) =>
            {
                update.OnUpdate();
            });
        }
        else
        {
            updates[count % updates.Count].OnUpdate();
            count++;
        }
    }
}
