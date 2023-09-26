using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationUpdate : MonoBehaviour
{
    public abstract void OnUpdate();
    private void OnEnable()
    {
        GetComponent<AnimationUpdaateManager>().updates.Add(this);
    }
    private void OnDisable()
    {
        GetComponent<AnimationUpdaateManager>().updates.Remove(this);
    }
}
