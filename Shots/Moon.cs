using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moon : MonoBehaviour {
    
    public float direction;

    Image myImage;
    Transform player;

    // Use this for initialization
    void Start () {
        myImage = GetComponent<Image>();

        myImage.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(StartAnimation());      
        player = GetPlayer().transform;
        StartCoroutine(Move());
    }

    IEnumerator StartAnimation() {
        for (transform.localScale = new Vector3(3f, 3f, 1f); transform.localScale.x > 1; transform.localScale -= new Vector3(1 / 30f, 1 / 30f)) {
            myImage.color += new Color(0f, 0f, 0f, 1 / 120f);
            yield return new WaitForSeconds(1 / 120f);
        }
    }

    IEnumerator Move() {
        direction -= 3f;
        transform.localPosition = new Vector3(player.localPosition.x + 100f * Mathf.Cos(direction * Mathf.PI / 180f), player.localPosition.y + 100f * Mathf.Sin(direction * Mathf.PI / 180f));

        yield return new WaitForSeconds(1 / 120f);
        StartCoroutine(Move());
    }

    public GameObject GetPlayer()
    {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            returnObj = obj;
        }

        return returnObj;
    }
}
