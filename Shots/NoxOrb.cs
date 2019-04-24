using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoxOrb : MonoBehaviour {

    public GameObject Noxier;
    public GameObject Nox;
    public float mov_amplitude;
    public float mov_frequency;

    Image myImage;
    float frequency = 0f;
    float speed = 0;
    float charge = 0;

    public void Start()
    {
        myImage = GetComponent<Image>();
        myImage.color = new Color(1f, 1f, 1f, charge + 0.5f);

        gameObject.transform.SetParent(GameObject.Find("TriggerPanel").transform);
        StartCoroutine(Working());
        StartCoroutine(SpeedPolling());
    }

    IEnumerator Working() {
        while (true) {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (transform.localPosition.y < 325f) transform.localPosition += new Vector3(0f, speed, 0f);
                    else
                    {
                        Vector3 pos = transform.localPosition;
                        transform.localPosition = new Vector3(pos.x, 325f, 0f);
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (transform.localPosition.y > -325f) transform.localPosition += new Vector3(0f, -speed, 0f);
                    else
                    {
                        Vector3 pos = transform.localPosition;
                        transform.localPosition = new Vector3(pos.x, -325f, 0f);
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (transform.localPosition.x > -610f) transform.localPosition += new Vector3(-speed, 0f, 0f);
                    else
                    {
                        Vector3 pos = transform.localPosition;
                        transform.localPosition = new Vector3(-610f, pos.y, 0f);
                    }
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (transform.localPosition.x < 610f) transform.localPosition += new Vector3(speed, 0f, 0f);
                    else
                    {
                        Vector3 pos = transform.localPosition;
                        transform.localPosition = new Vector3(610f, pos.y, 0f);
                    }
                }
            }
            else {
                frequency += mov_frequency;
                transform.localPosition += new Vector3(0, mov_amplitude * Mathf.Cos(frequency));
            }
            yield return new WaitForSeconds(1 / 60f);
        }
    }

    IEnumerator SpeedPolling() {
        while (true) {
            speed = Noxier.GetComponent<Player>().shiftSpeed * 2;
            yield return new WaitForSeconds(1f);
        }
    }

    private void ChangeChargeStatus() {      
        charge += 0.1f;
        if (charge >= 0.5f) {
            DarkInstantiate();
            charge = 0f;
        }
        myImage.color = new Color(1f, 1f, 1f, 0.5f + charge);      
    }

    public void DarkInstantiate() {
        GameObject ins;
        ins = Instantiate(Nox, GameObject.Find("TriggerPanel").transform);
        if (ins != null)
        {
            ins.GetComponent<Shot>().SetIdentity(transform.localPosition, 0.01f, 0f, 1f, 1f, Noxier.GetComponent<Player>().subShotDamage / 4f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("bat")) {
            ChangeChargeStatus();
            collision.GetComponent<Shot>().Unable();
        }
    }
}
