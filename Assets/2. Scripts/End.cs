using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public float distance;
    public int num;
    public int step;
    public int startValue;
    public int startMoney;
    public int moneyStep;
    public Gradient grad;
    private void Start()
    {
        SetRows(transform.GetChild(0), 0);
        for (int i = 1; i < num; i++)
        {
            Instantiate(transform.GetChild(0).gameObject, transform).transform.localPosition = new Vector3(0, 0, i * distance);
            SetRows(transform.GetChild(i), i);
        }
    }
    void SetRows(Transform tr, int n)
    {
        for (int i = 0; i < tr.childCount; i++)
        {
            tr.GetChild(i).GetComponent<Renderer>().material.color = grad.Evaluate((float)n / num);
            tr.GetChild(i).GetComponent<Blocker>().Set(startValue + n * step, startMoney + n * moneyStep);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.isEnd = true;
        }
    }
}
