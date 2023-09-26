using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Blocker : MonoBehaviour
{
    public bool canBreak;
    public int hp;
    public TextMeshProUGUI hpTxt;
    public int money;
    public bool isEnemy;

    private void Start()
    {
        if (canBreak)
        {
            Reload();
        }
    }
    public void Set(int value, int m)
    {
        hp = value;
        money = m;
        Reload();
    }
    public void Reload()
    {
        hpTxt.text = hp.ToString();
    }
    public void Collide(int damage)
    {
        if (canBreak)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Destroy();
            }
            Reload();
        }
    }
    public void Destroy()
    {
        if (!isEnemy)
        {
            gameObject.SetActive(false);
            GManager.instance.DestroyPartcle(transform.position);
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.DOColor(new Color(0.7f, 0.7f, 0.7f), 0.4f);
            GetComponent<Animator>().SetInteger("die", Random.Range(1, 7));
            GetComponent<Collider>().enabled =  false;
            hpTxt.gameObject.SetActive(false);
        }
        MoneyManager.instance.AddMoney(money, Camera.main.WorldToScreenPoint(transform.position));
    }
}
