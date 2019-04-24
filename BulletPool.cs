using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public List<GameObject> poolCircle = new List<GameObject>();
    public List<GameObject> poolPetal = new List<GameObject>();
    public List<GameObject> poolThorn = new List<GameObject>();

    private void Awake()
    {
        poolCircle = new List<GameObject>();
        poolPetal = new List<GameObject>();
        poolThorn = new List<GameObject>();
    }

    public GameObject GetChildMin(string code) {
        if(code == "CIRCLE")
        {
            for (int i = 0; i < poolCircle.Count; i++)
            {
                if (!poolCircle[i].activeInHierarchy) return poolCircle[i];
            }
        }
        else if (code == "PETAL")
        {
            for (int i = 0; i < poolPetal.Count; i++)
            {
                if (!poolPetal[i].activeInHierarchy) return poolPetal[i];
            }
        }
        else if (code == "THORN")
        {
            for (int i = 0; i < poolThorn.Count; i++)
            {
                if (!poolThorn[i].activeInHierarchy) return poolThorn[i];
            }
        }
        return null;
    }
}
