using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollapsedEnemy : MonoBehaviour {

    Color EffectColor;

    public void SetIdentity(Sprite sprite, float width, float height, float scaleX, float scaleY, Color color) {
        if(sprite != null) GetComponent<Image>().sprite = sprite;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
        EffectColor = color;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(Fade());
        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleCollapse, GameObject.Find("ParticlePanel").transform);
        Vector2 mySize = GetComponent<RectTransform>().sizeDelta;
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = EffectColor;
        if (mySize.x == mySize.y) ins.GetComponent<RectTransform>().sizeDelta = mySize * 3;
        else {
            float tempSc = (mySize.x > mySize.y) ? mySize.x : mySize.y;
            ins.GetComponent<RectTransform>().sizeDelta = new Vector2(tempSc, tempSc) * 3;
        }

    }

    IEnumerator Fade() {
        Image myImage = GetComponent<Image>();
        for (myImage.color = new Color(1f, 1f, 1f, 1f); myImage.color.a > 0; myImage.color -= new Color(0f, 0f, 0f, 1 / 30f)) {
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(gameObject);
    }
}
