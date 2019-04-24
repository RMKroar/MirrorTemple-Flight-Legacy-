using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EleutheriosEncounter : Boss {

    public bool flag;

	// Use this for initialization
	void Start () {
        StartCoroutine("Animate");

        ParticleController Par = GameObject.Find("ParticleController").GetComponent<ParticleController>();
        GameObject ins = Instantiate(Par.ParticleAscend, GameObject.Find("ParticlePanel").transform);
        ins.transform.localPosition = transform.localPosition;
        ins.GetComponent<Image>().color = Color.white;

        if(flag) GameObject.Find("DialogPanel").GetComponent<DialogController>().InvokeDialog(0);
    }
}
