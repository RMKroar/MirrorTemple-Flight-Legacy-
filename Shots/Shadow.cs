using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shadow : MonoBehaviour {

    public GameObject Shadower;
    public Sprite[] img;
    public int imgIndex = 0;

    float speed = 0;

    public void Start()
    {
        gameObject.transform.SetParent(GameObject.Find("TriggerPanel").transform);
        gameObject.transform.SetSiblingIndex(0);

        StartCoroutine(Animate());
        StartCoroutine(Move());
        StartCoroutine(SpeedPolling());
    }

    IEnumerator Animate()
    {
        gameObject.GetComponent<Image>().sprite = img[imgIndex];
        yield return new WaitForSeconds(1 / 4f);
        imgIndex = (imgIndex == img.Length - 1) ? 0 : imgIndex + 1;
        StartCoroutine("Animate");
    }

    IEnumerator Move()
    {
        CircleCollider2D ShadowerCollider = Shadower.GetComponent<CircleCollider2D>();
        while (true)
        {
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
                ShadowerCollider.offset = new Vector2(Shadower.transform.localPosition.x - transform.localPosition.x, transform.localPosition.y - Shadower.transform.localPosition.y);
            }
            else {
                ShadowerCollider.offset = Vector2.zero;
            }
            yield return new WaitForSeconds(1 / 120f);
        }
    }

    IEnumerator SpeedPolling()
    {
        while (true)
        {
            speed = Shadower.GetComponent<Player>().shiftSpeed;
            yield return new WaitForSeconds(1f);
        }
    }
}
