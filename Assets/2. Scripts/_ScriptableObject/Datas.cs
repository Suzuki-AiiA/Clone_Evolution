using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Datas", menuName = "Datas")]
public class Datas : ScriptableObject
{
    public float sensutive;
    public float playerSpeed;
    public int[] playerAges;
    public float playerShootRate;
    public float bulletLifeTime;
    public float shootAngle;
    public bool hideUI;
    public float hideUIDis;
    public CardData sumCard, multCard, yearCard, shotCard;
    public GateData damageGate, speedGate, rangeGate, yearGate, sizeGate;
    public int[] gates;
 
}
[System.Serializable]
public class CardData
{
    public int[] hps;
    public int[] value;
}

[System.Serializable]
public class GateData
{
    public int startValue;
    public int step;
    public float multiplier;
    public int max;
}