using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAni : MonoBehaviour
{
    public float duration;
    public Ease ease;
    public float easeForce;
    private void Start()
    {
        transform.DOScale(1, duration).From(0).SetEase(ease, easeForce);
    }
}
