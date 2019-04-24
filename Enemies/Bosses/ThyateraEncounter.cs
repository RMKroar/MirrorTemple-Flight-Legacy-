using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThyateraEncounter : Boss {

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Animate");
        StartCoroutine(FadeoutBGM());

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        GameObject.Find("DialogPanel").GetComponent<DialogController>().InvokeDialog(3);
    }

    IEnumerator FadeoutBGM()
    {
        AudioSource audio = GameObject.Find("Stage_Thyatera(Clone)").GetComponent<AudioSource>();

        for (; audio.volume > 0; audio.volume -= 0.01f)
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        audio.Stop();
    }
}
