using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBigLittle : AnimationUpdate
{
    public Vector2 minMax;
    public float speed;
    Vector3 initScale;
    bool reverse = false;
    float value;
    private void Start()
    {
        initScale = transform.localScale;
        value = 1;
    }
    public override void OnUpdate()
    {
        if (reverse)
        {
            value += speed * Time.deltaTime;
            if (value >= minMax.y)
            {
                value = minMax.y;
                reverse = false;
            }
        }
        else
        {
            value -= speed * Time.deltaTime;
            if (value <= minMax.x)
            {
                value = minMax.x;
                reverse = true;
            }
        }
        transform.localScale = initScale * value;
    }
}
