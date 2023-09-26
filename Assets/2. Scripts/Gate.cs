using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Gate : MonoBehaviour
{
    public bool canUp;
    public int value;
    public int max;
    public int plus;
    public GateType type;
    public TextMeshProUGUI stepTxt;
    public TextMeshProUGUI valueTxt;
    public ScaleShake valueTxtShake;
    bool isUse = false;
    public float delay;
    public bool useCage = false;
    public Image image;
    public ScaleShake shake;
    public int cageTarget;
    int cageCunt = 0;
    bool isOver = false;
    private void Start()
    {
        GateData data = type == GateType.Damage ? GManager.Data.damageGate :
                        type == GateType.Speed ? GManager.Data.speedGate :
                        type == GateType.Range ? GManager.Data.rangeGate :
                        type == GateType.Size ? GManager.Data.sizeGate : GManager.Data.yearGate;
        plus = data.step;
        value = data.startValue;
        max = data.max;
        if(useCage){
            cageTarget = data.max;
        }

        stepTxt.text = "+" + plus;
        valueTxt.text = "+" + value;
        if (useCage)
        {
            image.fillAmount = 0;
        }
    }
    public void Collide()
    {
        if (!Player.instance.isUpgraded)
        {
            return;
        }
        if (useCage)
        {
            cageCunt++;
            image.fillAmount = (float)cageCunt / cageTarget;
            if (cageCunt >= cageTarget)
            {
                isOver = true;
                image.fillAmount = 1;
            }
            else
                shake.Shake();

        }
        else if (canUp)
        {
            value += plus;
            if (value > max)
            {
                value = max;
                canUp = false;
            }
            valueTxt.text = "+" + value;
            valueTxtShake.Shake();
        }
    }
    public bool CheckUse()
    {
        if (!isUse)
        {
            if (useCage)
            {
                if (isOver)
                {
                    isUse = true;
                    Player.instance.counter.AddCounter(() =>
                    {
                        gameObject.SetActive(false);
                    }, delay);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                isUse = true;
                Player.instance.counter.AddCounter(() =>
                {
                    gameObject.SetActive(false);
                }, delay);
                return false;
            }
        }
        return isUse;
    }
}
