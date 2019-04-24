using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avari : Boss {

    public GameObject GoldCoin;

    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator IMove;

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
        IMove = Move();
        StartCoroutine(IMove);
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
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();       

        for (int i = 0; i < 4 + GameController.rank / 2; i++) {
            cct.Play();
            float randAngle = Random.Range(0f, 360f);
            for (float j = 0; j < 360; j += 360f / (6 + GameController.rank * 2)) {
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentityEx("AVARI", 
                        transform.localPosition + new Vector3((50 * i) * Mathf.Cos((randAngle + j) * Mathf.PI / 180f), (20 * i) * Mathf.Sin((randAngle + j) * Mathf.PI / 180f)), 
                        3f, randAngle + j, 0.35f, 0.35f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(1f / (4 + GameController.rank / 2));
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4 + GameController.rank / 2; i++)
        {
            cct.Play();
            float randAngle = Random.Range(0f, 360f);
            for (float j = 0; j < 360; j += 360f / (6 + GameController.rank * 2))
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("AVARI",
                        transform.localPosition + new Vector3((20 * i) * Mathf.Cos((randAngle + j) * Mathf.PI / 180f), (50 * i) * Mathf.Sin((randAngle + j) * Mathf.PI / 180f)),
                        3f, randAngle + j, 0.35f, 0.35f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(1f / (4 + GameController.rank / 2));
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        lct.volume = 0.35f; lct.Play();

        for (float i = Random.Range(-60f, -50f); i < 55f; i += 30 - 2.2f * GameController.rank)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 7f + GameController.rank / 2, ToPlayerAngle() + i,
                0.3f, 0.3f, Color.white, Color.yellow);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("TAR", transform.localPosition, 7f + GameController.rank / 2, ToPlayerAngle() + i,
                0.4f, 0.4f, Color.white, Color.yellow);
        }

        yield return new WaitForSeconds(1.2f - 0.07f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        int dens = 3 + (GameController.rank * 3 / 2);

        for (int i = 0; i < 3 + GameController.rank / 2; i++)
        {
            cct.Play();
            float randAngle = Random.Range(0f, 360f);
            for (float j = 0; j < 360; j += 360f / dens)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("AVARI",
                        transform.localPosition + new Vector3((50 * i) * Mathf.Cos((randAngle + j) * Mathf.PI / 180f), (20 * i) * Mathf.Sin((randAngle + j) * Mathf.PI / 180f)),
                        3f, randAngle + j, 0.35f, 0.35f, Color.white, Color.yellow);
                }
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentityEx("AVARI",
                        transform.localPosition + new Vector3((20 * i) * Mathf.Cos((randAngle + j) * Mathf.PI / 180f), (50 * i) * Mathf.Sin((randAngle + j) * Mathf.PI / 180f)),
                        1f, randAngle + j + (180f / dens), 0.35f, 0.35f, Color.white, new Color(1f, 0.5f, 0f));
                }
            }
            yield return new WaitForSeconds(1f / (3 + GameController.rank / 2));
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        Color[] colors = new Color[6] { Color.red, new Color(1f, 0.5f, 0f), Color.yellow, Color.green, BulletCode.COLOR_SKY, BulletCode.COLOR_VIOLET };
        int dens = 4 + GameController.rank * 2;

        for (int i = 0; i < 4 + GameController.rank / 4; i++) {
            float randValue = Random.Range(0f, 360f);
            lct.volume = 0.35f; lct.Play();

            for (float j = 0; j < 360; j += 360f / dens) {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", 
                    transform.localPosition + new Vector3((40 * (i + 1)) * Mathf.Cos((randValue + j) * Mathf.PI / 180f), (40 * (i + i)) * Mathf.Sin((randValue + j) * Mathf.PI / 180f)),
                    11f + GameController.rank / 2 - i, randValue + j + (60 - 8 * i), 0.3f, 0.3f, Color.white, colors[i]);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", 
                    transform.localPosition + new Vector3((40 * (i + 1)) * Mathf.Cos((randValue + j) * Mathf.PI / 180f), (40 * (i + i)) * Mathf.Sin((randValue + j) * Mathf.PI / 180f)), 
                    11f + GameController.rank / 2 - i, randValue + j - (60 - 8 * i), 0.3f, 0.3f, Color.white, colors[i]);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1.3f - 0.05f * GameController.rank);

        StartCoroutine("Launch");
    }

    bool flag = false;
    IEnumerator Pattern_4()
    {
        if (!flag) {
            StopCoroutine(IMove);
            flag = true;
        }
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();
        immortal = true;

        Transform trigTr = GameObject.Find("TriggerPanel").transform;
        ArrayList Coins = new ArrayList();
        for (int i = 0; i < 3 + GameController.rank / 2; i++) {
            Vector3 randPos = new Vector3(Random.Range(-550f, 550f), Random.Range(-250f, 250f));
            ins = Instantiate(GoldCoin, trigTr);
            ins.transform.localPosition = randPos;
            Coins.Add(ins);

            yield return new WaitForSeconds(0.3f - 0.02f * GameController.rank);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject target in Coins) {
            if (target != null) {
                StartCoroutine(ToPointMove(target.transform.localPosition));
                yield return new WaitForSeconds(0.5f);
                target.GetComponent<GoldCoin>().InvokeUnable();
            }

            cct.Play();
            for (int i = 0; i < 5 + GameController.rank * 2; i++) {
                ins = GetBullet("CIRCLE");
                if (ins != null) {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 8f + GameController.rank / 2), Random.Range(0f, 360f),
                        0.35f, 0.35f, Color.red, Color.red);
                }
            }
            health -= maxHealth * 1 / (4 * (3 + GameController.rank / 2));
            CheckHealth();
            yield return new WaitForSeconds(1f / (3 + GameController.rank / 2));            
        }
        StartCoroutine(ToPointMove(new Vector3(450f, 0f)));

        yield return new WaitForSeconds(1f);
        StartCoroutine("Launch");
    }

    IEnumerator ToPointMove(Vector3 pos)
    {
        float mag = ToPointMagnitude(pos);
        float rad = ToPointAngleRAD(pos);
        for (int i = 0; i < 15; i++)
        {
            transform.localPosition += new Vector3(mag * Mathf.Cos(rad) / 15, mag * Mathf.Sin(rad) / 15);
            yield return new WaitForSeconds(1 / 120f);
        }
        transform.localPosition = pos;
    }

    public float ToPointAngleRAD(Vector3 targetPosition)
    {
        Vector3 moveDirection = targetPosition - transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x);
            return angle;
        }
        else return 0;
    }

    public float ToPointMagnitude(Vector3 targetPosition)
    {
        Vector3 moveDirection = transform.localPosition - targetPosition;
        return moveDirection.magnitude;
    }
}
