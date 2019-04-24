using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDebris : MonoBehaviour {

    public void SetIdentity(string picURL) {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Items/" + picURL) as Sprite;
        transform.localPosition = new Vector3(0f, -18f);
        StartCoroutine(Debris());
    }

    IEnumerator Debris() {
        Image myImage = GetComponent<Image>();
        for (; myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 10f)) {
            transform.localScale += new Vector3(1 / 5f, 1 / 5f, 0f);
            yield return new WaitForSeconds(1 / 60f);
        }

        Destroy(gameObject);
    }
}
