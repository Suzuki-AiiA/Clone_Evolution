using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationUpdaateManager))]
public class AnimationRotate : AnimationUpdate
{
    public Vector3 axis;
    public float speed;
    private void Start()
    {
        axis.Normalize();
    }
    public override void OnUpdate()
    {
        transform.localEulerAngles += axis * speed * Time.deltaTime;
    }
}
