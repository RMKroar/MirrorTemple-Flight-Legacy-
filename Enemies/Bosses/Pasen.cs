using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pasen : Boss {

    public GameObject[] hex;

    public GameObject Alpharia_Marie;

    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator Move()
    {
        yield return new WaitForSeconds(moveFrequency);
        _speed = moveSpeed;
        float judgeDirection = transform.localPosition.y + Random.Range(-90f, 90f);

        direction = (judgeDirection >= 0) ? 270 : 90;
        if (transform.localPosition.x >= 250)
        {
            direction = (direction == 270) ? Random.Range(260f, 270f) : Random.Range(90f, 100f);
        }
        else
        {
            direction = (direction == 270) ? Random.Range(270f, 280f) : Random.Range(80f, 90f);
        }
        radDir = direction * Mathf.PI / 180f;

        StartCoroutine("MoveSupport");
        StopCoroutine("Animate");
        StartCoroutine("Move");
    }

    IEnumerator MoveSupport()
    {
        GetComponent<Image>().sprite = MoveImage;
        transform.localPosition += new Vector3(Mathf.Cos(radDir) * _speed, Mathf.Sin(radDir) * _speed, 0);
        yield return new WaitForSeconds(1 / 60f);
        _speed -= moveFriction;
        if (_speed <= 0)
        {
            StartCoroutine("Animate");
        }
        else StartCoroutine("MoveSupport");
    }

    IEnumerator Launch()
    {
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    int pat0Cnt = 0;
    IEnumerator Pattern_0()
    {
        GameObject ins;
        if (pat0Cnt % 2 == 0)
        {
            for (int i = 0; i < 360; i += 120)
            {
                ins = Instantiate(hex[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(120, i);
                ins.GetComponent<Hexagram>().moveCode = 7;
                ins.transform.localPosition = transform.localPosition + new Vector3(120 * Mathf.Cos(i * Mathf.PI / 180f), 120 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = gameObject;
            }
        }
        else
        {
            for (int i = 0; i < 360; i += 120)
            {
                ins = Instantiate(hex[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(120, i);
                ins.GetComponent<Hexagram>().moveCode = 8;
                ins.transform.localPosition = transform.localPosition + new Vector3(120 * Mathf.Cos(i * Mathf.PI / 180f), 120 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = gameObject;
            }
        }
        yield return new WaitForSeconds(6f - 0.25f * GameController.rank);
        pat0Cnt++;
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        ins = Instantiate(hex[2], GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));
        ins.transform.localPosition = transform.localPosition;
        ins = Instantiate(hex[2], GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));
        ins.transform.localPosition = Alpharia_Marie.transform.localPosition;
        yield return new WaitForSeconds(6f - 0.25f * GameController.rank);
        StartCoroutine("Launch");
    }

    int pat2Cnt = 0;
    IEnumerator Pattern_2()
    {
        GameObject ins;
        if (pat2Cnt % 2 == 0)
        {
            for (int i = Random.Range(0, 180); i < 360; i += 180)
            {
                ins = Instantiate(hex[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(500, i);
                ins.GetComponent<Hexagram>().moveCode = 7;
                ins.transform.localPosition = GetPlayer().transform.localPosition + new Vector3(500 * Mathf.Cos(i * Mathf.PI / 180f), 500 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = GetPlayer();
            }
        }
        else
        {
            for (int i = Random.Range(0, 180); i < 360; i += 180)
            {
                ins = Instantiate(hex[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(500, i);
                ins.GetComponent<Hexagram>().moveCode = 8;
                ins.transform.localPosition = GetPlayer().transform.localPosition + new Vector3(500 * Mathf.Cos(i * Mathf.PI / 180f), 500 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = GetPlayer();
            }
        }
        yield return new WaitForSeconds(7f - 0.2f * GameController.rank);
        pat2Cnt++;
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        ins = Instantiate(hex[3], GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Hexagram>().SetIdentity(7, 0);
        ins.transform.localPosition = new Vector3(-640f, -360f);

        ins = Instantiate(hex[3], GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Hexagram>().SetIdentity(7, 180);
        ins.transform.localPosition = new Vector3(640f, 360f);
        yield return new WaitForSeconds(6f - 0.25f * GameController.rank);
        StartCoroutine("Launch");
    }

    int pat4Cnt = 0;
    IEnumerator Pattern_4() {
        GameObject ins;
        ins = Instantiate(Alpharia_Marie.GetComponent<Marie>().hex[pat4Cnt % 2], GameObject.Find("TriggerPanel").transform);
        ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));
        ins.transform.localPosition = Alpharia_Marie.transform.localPosition;

        ins = Instantiate(hex[pat4Cnt % 4], GameObject.Find("TriggerPanel").transform);
        if (pat4Cnt % 4 != 3)
        {
            ins.GetComponent<Hexagram>().SetIdentity(1, Random.Range(0f, 360f));
            ins.transform.localPosition = transform.localPosition;
        }
        else {
            ins.GetComponent<Hexagram>().SetIdentity(10, 180f);
            ins.transform.localPosition = new Vector3(640f, -360f);
        }

        yield return new WaitForSeconds(4f - 0.1f * GameController.rank);
        pat4Cnt++;
        StartCoroutine("Launch");
    }

    public void ShareHealth(float health)
    {
        this.health = health;
        CheckHealth();
    }

    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && (!immortal && !Alpharia_Marie.GetComponent<Marie>().immortal))
        {
            float dam = collision.GetComponent<Shot>().damage;
            health -= dam;
            Alpharia_Marie.GetComponent<Marie>().ShareHealth(health);
            if ("KUNAI".Equals(collision.gameObject.GetComponent<Shot>().moveCode))
            {
                GameObject tmp = GameController.ShotPool.GetComponent<ShotPool>().GetChildMin("SUPPORT");
                if (tmp != null) tmp.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, Random.Range(175f, 185f), 1f, 1f, 0);
            }
            if (!collision.GetComponent<Shot>().moveCode.Contains("#")) collision.GetComponent<Shot>().Unable();

            CheckHealth();
        }
    }

    public new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && (!immortal && !Alpharia_Marie.GetComponent<Marie>().immortal) && collision.GetComponent<Shot>().moveCode.Contains("#"))
        {
            health -= collision.GetComponent<Shot>().damage;
            Alpharia_Marie.GetComponent<Marie>().ShareHealth(health);
            CheckHealth();
        }
    }
}
