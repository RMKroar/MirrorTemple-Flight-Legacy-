using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Limit : Boss {
    public GameObject limitSword;
    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator Move()
    {
        yield return new WaitForSeconds(moveFrequency);
        _speed = moveSpeed;
        float judgeDirection = transform.localPosition.y + Random.Range(-90f, 90f);

        direction = (judgeDirection >= 0) ? 270 : 90;
        if (transform.localPosition.x >= 450)
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

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        if (GameController.rank <= 5)
        {
            ins = Instantiate(limitSword, GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(GetPlayer().transform.localPosition.x, 1770f, 0f);
            ins.GetComponent<LimitSword>().moveCode = 0;
            yield return new WaitForSeconds(6.5f - GameController.rank * 0.2f);
        }
        else {
            ins = Instantiate(limitSword, GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(
                GetPlayer().transform.localPosition.x > 350f? GetPlayer().transform.localPosition.x - 1000f : GetPlayer().transform.localPosition.x + 500f, 
                1770f, 0f);
            ins.GetComponent<LimitSword>().moveCode = 0;

            ins = Instantiate(limitSword, GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = new Vector3(
                GetPlayer().transform.localPosition.x < -350f ? GetPlayer().transform.localPosition.x + 1000f : GetPlayer().transform.localPosition.x - 500f,
                1770f, 0f);
            ins.GetComponent<LimitSword>().moveCode = 0;
            yield return new WaitForSeconds(9f - GameController.rank * 0.4f);
        }     
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        ins = Instantiate(limitSword, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = new Vector3(Random.Range(-1500f, -1200f), 1400f, 0f);
        ins.GetComponent<LimitSword>().moveCode = 3;
        yield return new WaitForSeconds(6.5f - GameController.rank * 0.05f);

        pct.Play();
        ins = Instantiate(limitSword, GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = new Vector3(Random.Range(1200f, 1500f), 1400f, 0f);
        ins.GetComponent<LimitSword>().moveCode = 1;
        yield return new WaitForSeconds(6.5f - GameController.rank * 0.05f);

        StartCoroutine("Launch");
    }
}
