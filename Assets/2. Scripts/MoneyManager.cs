using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public float distance;
    public Ease ease;
    public float duration;
    public Transform moneyTarget;
    private void Awake() {
        instance = this;
    }
    public void AddMoney(int a, Vector2 pos)
    {
        for (int i = 0; i < a; i++)
        {
            Money m = PoolingManager.instance.moneyPooling.GetPoolingObject();
            float angle = Random.Range(0.0f, 360f);
            Vector2 targetPos = pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            m.transform.DOMove(targetPos, duration).SetEase(ease).OnComplete(() =>
            {
                m.transform.DOMove(moneyTarget.position, duration).OnComplete(() =>
                {
                    m.gameObject.SetActive(false);
                    GManager.instance.AddMoney(1);
                });
            }).From(pos);
        }
    }
}
