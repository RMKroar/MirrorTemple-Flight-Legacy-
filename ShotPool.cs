using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPool : MonoBehaviour {
    public List<GameObject> poolMain = new List<GameObject>();
    public List<GameObject> poolSub = new List<GameObject>();
    public List<GameObject> poolSupport = new List<GameObject>();
    public List<GameObject> poolSpecial = new List<GameObject>();

    private void Awake()
    {
        poolMain = new List<GameObject>();
        poolSub = new List<GameObject>();
        poolSupport = new List<GameObject>();
        poolSpecial = new List<GameObject>();
    }

    public GameObject GetChildMin(string code)
    {
        if (code == "MAIN")
        {
            for (int i = 0; i < poolMain.Count; i++)
            {
                if (!poolMain[i].activeInHierarchy) return poolMain[i];
            }
        }
        else if (code == "SUB")
        {
            for (int i = 0; i < poolSub.Count; i++)
            {
                if (!poolSub[i].activeInHierarchy) return poolSub[i];
            }
        }
        else if (code == "SUPPORT")
        {
            for (int i = 0; i < poolSupport.Count; i++)
            {
                if (!poolSupport[i].activeInHierarchy) return poolSupport[i];
            }
        }
        else if (code.Contains("SPECIAL")) {
            string shotName = code.Replace("SPECIAL_", "");
            for (int i = 0; i < poolSpecial.Count; i++)
            {
                if (poolSpecial[i].name.Contains(shotName) && !poolSpecial[i].activeInHierarchy) return poolSpecial[i];
            }
        }
        return null;
    }
}
