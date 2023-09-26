using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    MergedUpdate.UpdateData update;
    Counter.CounterData counter;
    public int[] damage;
    public float[] rotateSpeed;
    public Vector3[] dir;
    public float[] speed;
    public GameObject[] levels;
    int currentLv = -1;
    Weapon w;
    public void SetWeapon(Weapon we)
    {
        w = we;
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("card"))
        {
            other.gameObject.GetComponent<Card>().Attack(damage[currentLv]);
            Off();
        }
        if (other.gameObject.CompareTag("gate"))
        {
            other.GetComponent<Gate>().Collide();
            Off();
        }
        if (other.gameObject.CompareTag("block"))
        {
            other.GetComponent<Blocker>().Collide((int)(damage[currentLv] * w.character.damage.status));
            Off();
        }
    }
    public void OnUpdate()
    {
        transform.position += w.transform.forward * speed[currentLv] * Time.deltaTime;
        transform.Rotate(dir[currentLv] * rotateSpeed[currentLv] * Time.deltaTime);
    }
    public void SetLevel(int lv)
    {
        if (currentLv >= 0)
        {
            levels[currentLv].SetActive(false);
        }
        transform.eulerAngles = w.transform.eulerAngles;
        currentLv = lv;
        levels[currentLv].SetActive(true);
    }
    public void Off()
    {
        // Player.instance.mergedUpdate.Remove(w.updateData);
        // Player.instance.counter.Remove(w.countData);
        gameObject.SetActive(false);
    }
}
