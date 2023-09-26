using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using DG.Tweening;
public class Card : MonoBehaviour
{
    public int[] hps;
    public Image[] stars;
    public int[] value;
    public CardType type;
    public int attacked = 0;
    int currentLevel = 0;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI hpText;
    public ScaleShake thisShake;
    public ScaleShake textShake;
    public Transform starsParent;
    public ScaleShake[] starShake;
    bool isOver = false;
    public string[] valueTextStr;
    public int currentValue;
    private void Start()
    {
        starShake = starsParent.GetComponentsInChildren<ScaleShake>();
        CardData data = type == CardType.Add ? GManager.Data.sumCard :
                        type == CardType.Multiply ? GManager.Data.multCard :
                        type == CardType.Year ? GManager.Data.yearCard : GManager.Data.shotCard;
        
        hps = data.hps;
        value = data.value;
        

        UpdateHp();
        SetValue();
    }
    public void SetValue()
    {
        switch (type)
        {
            case CardType.Add:
                valueText.text = "+" + value[currentLevel];
                break;
            case CardType.Multiply:
                valueText.text = "X" + value[currentLevel];
                break;
            case CardType.Year:
                valueText.text = "+" + value[currentLevel];
                break;
            case CardType.Shot:
                valueText.text = valueTextStr[currentLevel] + " shoot";
                break;
            default:
                break;
        }
        currentValue = value[currentLevel];
    }
    public void Attack(int damage)
    {
        if (isOver)
        {
            return;
        }
        attacked += damage;
        thisShake.Shake();
        if (attacked >= hps[currentLevel])
        {
            attacked = 0;
            stars[currentLevel].fillAmount = 1;
            currentLevel++;
            textShake.Shake();
            SetValue();
            starShake[currentLevel - 1].Shake();
            if (currentLevel >= hps.Length)
            {
                Complete();
                return;
            }
        }
        UpdateHp();
    }
    void UpdateHp()
    {
        hpText.text = attacked + "/" + hps[currentLevel];
        stars[currentLevel].fillAmount = (float)attacked / hps[currentLevel];
    }
    public void Complete()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;
        TransLine.instance.AddToLine(this);
        GetComponent<Collider>().isTrigger = false;
    }
}
