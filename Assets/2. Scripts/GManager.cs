using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using GM.SceneSwitch;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using GameManagerData;
using GM.PlayerDataSaver;

namespace GameManagerData
{
    [System.Serializable]
    public class UIItems
    {
        public GameObject startUI;
        public GameObject gameUI;
        public GameObject winUI;
        public GameObject failUI;
        public GameObject moneyUI;
        public GameObject levelUI;
    }
    [System.Serializable]
    public class Particles
    {

    }
    [System.Serializable]
    public class GameDatas
    {
    }
    [System.Serializable]
    public class Texts
    {
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI moneyText;
    }
    [System.Serializable]
    public class OtherObjects
    {
    }
    [System.Serializable]
    public class Poolings
    {
        public Pooling lv1RockPooling;
    }
    [System.Serializable]
    public class Cameras
    {
        public CinemachineVirtualCamera updateCam;
    }
}

public class GManager : MonoBehaviour
{
    public static GManager instance;

    public GState gState = GState.Start;

    public UIItems uIItems;
    public Particles particles;
    public GameDatas gameDatas;
    public Texts texts;
    public Poolings poolings;
    public Cameras cameras;
    public OtherObjects otherObjects;

    ///////////////////////////////////////////////
    public Datas data;
    public static Datas Data{
        get{
            return instance.data;
        }
    }
    public bool useTutorial;
    [Space]
    public List<int> levels = new List<int>();
    public DeviceTypeSettings deviceType;
    public ParticleSystem part;



    private void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        gState = GState.Start;
        QualitySettings.SetQualityLevel(deviceType.GetDeviceType());
    }
    private void Start()
    {
        if (!useTutorial)
        {
            try
            {
                Tutorial.instance.gameObject.SetActive(false);
            }
            catch { }
        }
        texts.levelText.text = "Level "+(PlayerData.intData["level"]+1);
        texts.moneyText.text = PlayerData.intData["money"].ToString();
        if(data.hideUI){
            uIItems.moneyUI.SetActive(false);
            uIItems.levelUI.SetActive(false);
            uIItems.gameUI.SetActive(false);
            cameras.updateCam.transform.localPosition += Vector3.forward * data.hideUIDis;
            UpgradeZone.instance.cardTarget.transform.localPosition += Vector3.forward * data.hideUIDis;
        }
    }
    public void DestroyPartcle(Vector3 pos){
        part.transform.position = pos;
        part.Play();
    }
    public void GameStart()
    {
        if (gState == GState.Start)
        {
            gState = GState.Playing;
            uIItems.startUI.SetActive(false);
            uIItems.gameUI.SetActive(true);
            Player.instance.GameStart();
        }
    }

    public void GameWin()
    {
        if (gState != GState.End)
        {
            PlayerData.intData["level"]++;
            gState = GState.End;
            uIItems.winUI.SetActive(true);
        }
    }
    public void GameFail()
    {
        if (gState != GState.End)
        {
            gState = GState.End;
            uIItems.failUI.SetActive(true);
        }
    }
    public void Next()
    {
        ReloadScene();
    }

    public void ReloadScene()
    {
        SceneSwtich.instance.SwitchScene();
    }


    public static void Delay(UnityAction action, float time)
    {
        instance.StartCoroutine(StartDelay(action, time));
    }

    static IEnumerator StartDelay(UnityAction action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
    public void AddMoney(int a){
        PlayerData.intData["money"] += a;
        texts.moneyText.text = PlayerData.intData["money"].ToString();
    }




#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale /= 10;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Time.timeScale *= 10;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            ReloadScene();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameWin();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameFail();
        }
        if(Input.GetKey(KeyCode.A)){
            Player.instance.Move(Vector2.left / 10);
        }
        if(Input.GetKey(KeyCode.D)){
            Player.instance.Move(Vector2.right / 10);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Player.instance.moveSpeed = 10;
            GameStart();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            Player.instance.moveSpeed = 2;
        }
    }
#endif
}

[System.Serializable]
public class DeviceTypeSettings
{
    public bool forceDeviceType;
    public DeviceManager.DeviceType deviceType;
    public int GetDeviceType()
    {
        if (forceDeviceType)
        {
            return (int)deviceType;
        }
        else
        {
            deviceType = DeviceManager.GetDeviceType();
            return (int)deviceType;
        }
    }
}