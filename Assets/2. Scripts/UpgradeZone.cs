using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class UpgradeZone : MonoBehaviour
{
    public static UpgradeZone instance;
    public Transform playerTarget;
    public float duration;
    public Transform cardTarget;
    bool isUpgrade = false;
    public CinemachineVirtualCamera vCam;
    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isUpgrade)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            isUpgrade = true;
            Player.instance.ReadyUpgrade();
            vCam.Priority = 11;
            Player.instance.transform.DOMoveZ(playerTarget.position.z, duration).OnComplete(() =>
            {
                TransLine.instance.CallCard(cardTarget);
                Player.instance.ReadyUPPP();
                if (TransLine.instance.cards.Count == 0)
                {
                    Resume();
                }
            });
            Player.instance.moveBase.DOMoveX(0, duration);

        }
    }
    public void Resume()
    {
        vCam.Priority = 0;
        Player.instance.Resume();
    }
}
