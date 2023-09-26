using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public Transform target;

    Vector3 offset;
    public UnityAction action;
    public UnityAction followCanvas;
    private void Awake()
    {
        followCanvas = ()=>{};
        instance = this;
        offset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        transform.position = target.position + offset;
        action?.Invoke();
        followCanvas?.Invoke();
    }
}
