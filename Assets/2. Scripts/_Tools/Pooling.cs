using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    public List<GameObject> pool = new List<GameObject>();
    public GameObject prefab;
    public int max;
    GameObject Get<T>()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (typeof(T) == typeof(ParticleSystem))
            {
                if (!pool[i].GetComponent<ParticleSystem>().isPlaying)
                {
                    return pool[i];
                }
            }
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        if (pool.Count >= max && max > 0)
        {
            return pool[0];
        }
        GameObject obj = Instantiate(prefab, transform);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetPoolingObject()
    {
        return Get<GameObject>();
    }

    public T GetPoolingObject<T>()
    {
        return Get<T>().GetComponent<T>();
    }
}
