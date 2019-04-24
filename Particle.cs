using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Particle : MonoBehaviour {

    public Sprite[] Animations;
    Image myImage;

    private void Start()
    {
        myImage = GetComponent<Image>();
        StartCoroutine(Animate());

        if (name.Contains("apocalipse")) { StartCoroutine(DelayedAudio()); }
    }

    IEnumerator Animate() {
        for (int it = 0; it < Animations.Length; it++) {
            if (Animations[it] != null) myImage.sprite = Animations[it];
            else myImage.color = Color.clear;

            if (name.Contains("apocalipse"))
            {
                Color myColor = myImage.color;
                if (it >= 23) myImage.color = new Color(myColor.r, myColor.g, myColor.b, (40 - it) / 18f);
                if (it >= 14) GetComponent<RectTransform>().sizeDelta += new Vector2(100f, 100f);
            }
            yield return new WaitForSeconds(1 / 30f);
        }
        Destroy(gameObject);
    }

    IEnumerator DelayedAudio() {
        yield return new WaitForSeconds(1 / 2f);
        transform.GetChild(1).GetComponent<AudioSource>().Play();
    }
}
