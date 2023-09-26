using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class NewPooling<T> where T : MonoBehaviour
{
    public List<T> pool = new List<T>();
    public T prefab;
    public int max;
    public Transform parent;
    // public NewPooling(Transform parent)
    // {
    //     this.parent = parent;
    // }
    T Get()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }
        if (pool.Count >= max && max > 0)
        {
            return pool[0];
        }
        T obj = MonoBehaviour.Instantiate(prefab, parent);
        pool.Add(obj);
        return obj;
    }

    public T GetPoolingObject()
    {
        return Get();
    }

}