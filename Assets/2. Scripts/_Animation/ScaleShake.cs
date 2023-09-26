using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleShake : MonoBehaviour
{
    public float force;
    public float duration;
    float initScale;
    private void Start() {
        initScale = transform.localScale.x;
    }
    public void Shake()
    {
        // transform.DOPunchScale(Vector3.one * force, duration);
        transform.DOScale(Vector3.one * (initScale+force), duration/2).OnComplete(() =>
        {
            transform.DOScale(Vector3.one*initScale, duration/2);
        });
    }
}
