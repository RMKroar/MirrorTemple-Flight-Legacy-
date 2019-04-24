using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalImage : MonoBehaviour {

    public Sprite[] myImages;

    public void SetIdentity(int id) {
        GetComponent<Image>().sprite = myImages[id];
    }
}
