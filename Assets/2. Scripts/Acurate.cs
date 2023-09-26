using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acurate : MonoBehaviour
{
    public CardDragManager cardDragManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("character"))
        {
            Character c = other.GetComponent<Character>();
            cardDragManager.selectedChara.Add(c);
            c.OnEventEnter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("character"))
        {
            Character c = other.GetComponent<Character>();
            c.OnEventExit();
            cardDragManager.selectedChara.Remove(c);
        }
    }
}
