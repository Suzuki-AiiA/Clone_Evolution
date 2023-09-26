using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawn : MonoBehaviour
{
    public int gameNum;
    public Gate[] gates;
    private void Start()
    {
        Instantiate(gates[GManager.Data.gates[gameNum]], transform.position, transform.rotation);
    }
}
