using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEncounter : Boss {

    public Sprite[] sprites;

    // Use this for initialization
    void Start()
    {
        int episodeNumber = MainController.GetGameMode() % 10;
        for (int i = 0; i <= 2; i++) {
            img[i] = sprites[episodeNumber * 3 + i];
        }
        img[3] = sprites[episodeNumber * 3 + 1];
        collapseImage = sprites[episodeNumber * 3];
        MoveImage = sprites[episodeNumber * 3];

        StartCoroutine("Animate");
        StartCoroutine(FadeoutBGM());

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        GameObject.Find("DialogPanel").GetComponent<DialogController>().InvokeDialog(-1);
    }

    IEnumerator FadeoutBGM()
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("Stage").GetComponent<AudioSource>();

        for (; audio.volume > 0; audio.volume -= 0.01f)
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        audio.Pause();
    }
}
