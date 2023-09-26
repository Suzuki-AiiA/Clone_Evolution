using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    Transform tr;
    void Start()
    {
        tr = Camera.main.transform;
    }
    private void Update()
    {
        transform.forward = transform.position - tr.position;
    }
}
