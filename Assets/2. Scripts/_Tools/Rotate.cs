using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    private void Start()
    {
        axis.Normalize();
    }
    private void Update()
    {
        transform.localEulerAngles += axis * speed * Time.deltaTime;
    }
}
