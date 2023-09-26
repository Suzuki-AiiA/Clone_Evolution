using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerate : MonoBehaviour
{
    public float width;
    public GameObject ground;
    public int num;
    public int skillNum;
    public Vector3 rot;
    private void Start()
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(ground, transform);
            g.transform.localPosition = new Vector3(0, 0, i * width);
            g.transform.localRotation = Quaternion.Euler(rot);
            if (i == skillNum)
            {
                TransLine.instance.transform.position = g.GetChild(1).transform.position;
                UpgradeZone.instance.gameObject.transform.position = g.transform.position + Vector3.forward * width / 2;
            }
            if (i > skillNum)
            {
                g.GetChild(1).SetActive(false);
            }
        }
    }
    [ContextMenu("spawn")]
    public void Spawn()
    {
        for (int i = 0; i < num; i++)
        {
            GameObject g = Instantiate(ground, transform);
            g.transform.localPosition = new Vector3(0, 0, i * width);
            g.transform.localRotation = Quaternion.Euler(rot);
            if (i > skillNum)
            {
                g.GetChild(1).SetActive(false);
            }
        }
    }
}
