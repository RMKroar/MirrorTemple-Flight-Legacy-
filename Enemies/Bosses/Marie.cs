using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marie : Boss
{
    public GameObject Pasen;
    public GameObject[] hex;

    GameObject Betaria_Pasen;

    float _speed = 0;
    float direction = 0;
    float radDir;

    bool flag = false;

    IEnumerator Move()
    {
        if (!flag) {
            if (GameController.stageNum <= 4) {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (obj != gameObject)
                    {
                        obj.transform.localPosition = new Vector3(720, 0);
                        Destroy(obj);
                    }
                }
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpecialBullet"))
                {
                    Destroy(obj);
                }
            }

            Betaria_Pasen = Instantiate(Pasen, GameObject.Find("TriggerPanel").transform);
            Betaria_Pasen.transform.localPosition = new Vector3(250, -120);
            Betaria_Pasen.GetComponent<Pasen>().Alpharia_Marie = gameObject;
            flag = true;
        }
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

    int pat0Cnt = 0;

    IEnumerator Pattern_0() {
        GameObject ins;
        if (pat0Cnt % 2 == 0)
        {
            for (int i = 0; i < 360; i += 120)
            {
                ins = Instantiate(hex[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(120, i);
                ins.GetComponent<Hexagram>().moveCode = 5;
                ins.transform.localPosition = transform.localPosition + new Vector3(120 * Mathf.Cos(i * Mathf.PI / 180f), 120 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = gameObject;
            }
        }
        else {
            for (int i = 0; i < 360; i += 120)
            {
                ins = Instantiate(hex[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(120, i);
                ins.GetComponent<Hexagram>().moveCode = 6;
                ins.transform.localPosition = transform.localPosition + new Vector3(120 * Mathf.Cos(i * Mathf.PI / 180f), 120 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = gameObject;
            }
        }
        yield return new WaitForSeconds(6f - 0.25f * GameController.rank);
        pat0Cnt++;
        StartCoroutine("Launch");
    }

    bool pat1Flag = false;

    IEnumerator Pattern_1() {
        if (!pat1Flag) {
            pat1Flag = true;
            GameObject ins;
            ins = Instantiate(hex[2], GameObject.Find("TriggerPanel").transform);
            ins.transform.localPosition = Vector3.zero;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine("Launch");
    }

    int pat2Cnt = 0;

    IEnumerator Pattern_2() {
        GameObject ins;
        if (pat2Cnt % 2 == 0)
        {
            for (int i = Random.Range(0, 180); i < 360; i += 180)
            {
                ins = Instantiate(hex[0], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(350, i);
                ins.GetComponent<Hexagram>().moveCode = 5;
                ins.transform.localPosition = GetPlayer().transform.localPosition + new Vector3(350 * Mathf.Cos(i * Mathf.PI / 180f), 350 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = GetPlayer();
            }
        }
        else
        {
            for (int i = Random.Range(0, 180); i < 360; i += 180)
            {
                ins = Instantiate(hex[1], GameObject.Find("TriggerPanel").transform);
                ins.GetComponent<Hexagram>().SetIdentity(350, i);
                ins.GetComponent<Hexagram>().moveCode = 6;
                ins.transform.localPosition = GetPlayer().transform.localPosition + new Vector3(350 * Mathf.Cos(i * Mathf.PI / 180f), 350 * Mathf.Sin(i * Mathf.PI / 180f));
                ins.GetComponent<Hexagram>().lord = GetPlayer();
            }
        }
        yield return new WaitForSeconds(7f - 0.2f * GameController.rank);
        pat2Cnt++;
        StartCoroutine("Launch");
    }

    bool pat3Flag = false;

    IEnumerator Pattern_3()
    {
        if (!pat3Flag)
        {
            pat3Flag = true;
            GameObject ins;
            ins = Instantiate(hex[2], GameObject.Find("TriggerPanel").transform);
            ins.GetComponent<Hexagram>().moveCode = 11;
            ins.transform.localPosition = Vector3.zero;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_4() {
        Stack playerPosition = new Stack();
        Stack myHealth = new Stack();
        for (int i = 0; i < 60; i++) {
            playerPosition.Push(GetPlayer().transform.localPosition);
            myHealth.Push(health);
            yield return new WaitForSeconds(1/120f);
        }
        GetStage().GetComponent<AudioSource>().pitch = -1;
        GameObject.Find("Back1Panel").GetComponent<Image>().color = Color.black;
        GameObject.Find("Back2Panel").GetComponent<Image>().color = Color.black;

        GameObject ins;
        ins = Instantiate(hex[3], GameObject.Find("TriggerPanel").transform);
        ins.transform.localPosition = Vector3.zero;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet")) {
            Bullet objData = obj.GetComponent<Bullet>();
            objData.speed *= -1;
        }

        immortal = true; Betaria_Pasen.GetComponent<Pasen>().immortal = true;
        for (int i = 0; i < 60; i++) {
            health = (float)myHealth.Pop();
            Betaria_Pasen.GetComponent<Pasen>().ShareHealth(health);
            GetPlayer().transform.localPosition = (Vector3)playerPosition.Pop();

            yield return new WaitForSeconds(1 / 120f);
        }

        immortal = false; Betaria_Pasen.GetComponent<Pasen>().immortal = false;
        GetStage().GetComponent<AudioSource>().pitch = 1;
        GameObject.Find("Back1Panel").GetComponent<Image>().color = Color.white;
        GameObject.Find("Back2Panel").GetComponent<Image>().color = Color.white;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Bullet objData = obj.GetComponent<Bullet>();
            if(objData.speed < 0) objData.speed *= -1;
        }

        yield return new WaitForSeconds(6f);
        StartCoroutine("Launch");
    }

    public void ShareHealth(float health) {
        this.health = health;
        CheckHealth();
    }

    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && (!immortal && !Betaria_Pasen.GetComponent<Pasen>().immortal))
        {
            float dam = collision.GetComponent<Shot>().damage;
            health -= dam;
            Betaria_Pasen.GetComponent<Pasen>().ShareHealth(health);
            if ("KUNAI".Equals(collision.gameObject.GetComponent<Shot>().moveCode))
            {
                GameObject tmp = GameController.ShotPool.GetComponent<ShotPool>().GetChildMin("SUPPORT");
                if (tmp != null) tmp.GetComponent<Shot>().SetIdentity(transform.localPosition, 30f, Random.Range(175f, 185f), 1f, 1f, 0);
            }
            if( !collision.GetComponent<Shot>().moveCode.Contains("#") ) collision.GetComponent<Shot>().Unable();

            CheckHealth();
        }
    }

    public new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Shot" && (!immortal && !Betaria_Pasen.GetComponent<Pasen>().immortal) && collision.GetComponent<Shot>().moveCode.Contains("#"))
        {
            health -= collision.GetComponent<Shot>().damage;
            Betaria_Pasen.GetComponent<Pasen>().ShareHealth(health);
            CheckHealth();
        }
    }

    public GameObject GetStage()
    {
        GameObject returnObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stage"))
        {
            returnObj = obj;
        }

        return returnObj;
    }
}
