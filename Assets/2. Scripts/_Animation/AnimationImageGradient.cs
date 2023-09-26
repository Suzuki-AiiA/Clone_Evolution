using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationImageGradient : AnimationUpdate
{
    Image image;
    public Gradient gradient;
    public float speed;
    private void Start()
    {
        image = GetComponent<Image>();
    }

    public override void OnUpdate()
    {
        image.color = gradient.Evaluate(Mathf.PingPong(Time.time * speed, 1));
    }
}
