using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCoin : MonoBehaviour {

    Image myImage;

	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
        myImage.color = new Color(1f, 1f, 1f, 0f);

        StartCoroutine(Enable());
        StartCoroutine(Animate());
	}

    IEnumerator Enable() {
        for (; myImage.color.a < 0.5; myImage.color += new Color(0f, 0f, 0f, 1 / 30f)) {
            yield return new WaitForSeconds(1 / 30f);
        }
    }

    IEnumerator Animate() {
        float rot = 0;
        while (true) {
            rot += 1;
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, rot));

            yield return new WaitForSeconds(1 / 30f);
        }      
    }

    IEnumerator Unable() {
        for (; myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 15f))
        {
            yield return new WaitForSeconds(1 / 30f);
        }

        Destroy(gameObject);
    }

    public void InvokeUnable() {
        StartCoroutine(Unable());
    }
}
