using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnivernEncounter : Boss {

    public GameObject EleutheriosEncounter;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Animate");

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        ins = Instantiate(EleutheriosEncounter, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = new Vector3(-450f, 0f);
        ins.GetComponent<EleutheriosEncounter>().flag = false;
        ins.transform.localScale = new Vector3(-1, 1, 1);

        GameObject.Find("DialogPanel").GetComponent<DialogController>().InvokeDialog(5);
    }
}
