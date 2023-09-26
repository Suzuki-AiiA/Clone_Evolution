using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (GM.PlayerDataSaver.PlayerData.intData["level"] >= GManager.instance.levels.Count)
        {
            gameObject.SetActive(false);
        }
    }
}
