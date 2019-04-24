using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infinite : Boss {

    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator IBackground;

    IEnumerator Move()
    {
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

        yield return null;
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
        StopCoroutine("Move");
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        for (float i = -100, count = 0; i <= 100; i += 15 - 1f * GameController.rank, count++) {
            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentityEx("INFINITE", transform.localPosition + new Vector3(-(i + 100), i * 2),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.white, Color.white);
            }
            if (count % (1 + GameController.rank / 3) == 0) yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(0.5f - 0.045f * GameController.rank);

        pct.volume = 0.35f; pct.Play();
        for (float i = -100, count = 0; i <= 100; i += 15 - 1f * GameController.rank, count++)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("INFINITE", transform.localPosition + new Vector3(-(i + 100), -i * 2),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.black, Color.black);
            }
            if (count % (1 + GameController.rank / 3) == 0) yield return new WaitForSeconds(1 / 120f);
        }
        yield return new WaitForSeconds(1f - 0.07f * GameController.rank);

        sct.Play();
        float density = 360 / (8 + GameController.rank * 3);
        for (float i = Random.Range(0f, density); i < 360; i += density)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("INFINITE", transform.localPosition, 1f, i, 0.35f, 0.35f,
                    Color.white, Color.white);
            }
        }
        StartCoroutine("Move");
        yield return new WaitForSeconds(2f - 0.1f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        StartCoroutine(ToPointMove(GetPlayer().transform.localPosition));
        yield return new WaitForSeconds(1.5f);

        float[] density = new float[11] { 11.5f, 11f, 10.5f, 10f, 9.5f, 9f, 8.5f, 8f, 7.5f, 7f, 6.5f };

        float shootEa = 8 + 2 * GameController.rank;
        for (float i = Random.Range(0f, 360f), j = 0; j <= shootEa;
            i += density[GameController.rank] + Random.Range(-1.5f, 1.5f), j++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4f + j * 0.5f, i, 0.35f, 0.35f,
                 Color.white, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 4f + (shootEa - j) * 0.5f, i + 60, 0.35f, 0.35f,
                 Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4f + j * 0.5f, i + 120, 0.35f, 0.35f,
                 Color.white, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 4f + (shootEa - j) * 0.5f, i + 180, 0.35f, 0.35f,
                 Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 4f + j * 0.5f, i + 240, 0.35f, 0.35f,
                 Color.white, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 4f + (shootEa - j) * 0.5f, i + 300, 0.35f, 0.35f,
                 Color.black, Color.black);

            yield return new WaitForSeconds(1 / 15f - 1 / 225f * GameController.rank);
        }

        yield return new WaitForSeconds(4f - 0.1f * GameController.rank);
        StartCoroutine(Launch());
    }

    bool flag = false;
    IEnumerator Pattern_2()
    {
        if (!flag) {
            StartCoroutine(ToPointMove(new Vector3(450f, 0f)));
            yield return new WaitForSeconds(1f);
            flag = true;
        }
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        for (float i = -100, count = 0; i <= 100; i += 15 - 0.9f * GameController.rank, count++)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("INFINITE", transform.localPosition + new Vector3(-(i + 100), i * 2),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.black, Color.black);
            }

            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("INFINITE", transform.localPosition + new Vector3(-(i + 100), -i * 2),
                                 0.01f, ToPlayerAngle() + Random.Range(-60f, 60f), 0.35f, 0.35f, Color.white, Color.white);
            }

            if (count % (1 + GameController.rank / 3) == 0) yield return new WaitForSeconds(1 / 120f);
        }

        yield return new WaitForSeconds(1f - 0.07f * GameController.rank);

        lct.volume = 0.35f; lct.Play();
        float density = 360 / (8 + GameController.rank * 3);
        for (float i = Random.Range(0f, density); i < 360; i += density)
        {
            ins = GetBullet("PETAL");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 10f + GameController.rank * 0.8f, i, 0.3f, 0.3f,
                    Color.black, Color.black);
            }
        }
        StartCoroutine("Move");
        yield return new WaitForSeconds(3f - 0.05f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        sct.Play();
        float density = 360 / (16 + GameController.rank * 2);
        for (int j = 0; j < 2 + GameController.rank / 4; j++) {
            for (float i = Random.Range(0f, density); i < 360; i += density)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7 + 1.5f * j, i, 0.35f, 0.35f,
                        Color.black, Color.black);
                }
            }
        }    

        IBackground = Background();
        StartCoroutine(IBackground);

        yield return new WaitForSeconds(1f);

        for (int j = 0; j < 1 + GameController.rank / 4; j++) {
            lct.volume = 0.35f; lct.Play();
            for (float i = Random.Range(0f, density); i < 360; i += density)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 7f + GameController.rank / 2, i,
                    0.3f, 0.3f, Color.white, Color.white);
            }

            yield return new WaitForSeconds(1f / (1 + GameController.rank / 4));
        }

        StartCoroutine("Move");
        yield return new WaitForSeconds(2f - 0.05f * GameController.rank);
        StartCoroutine(Launch());      
    }

    IEnumerator Background() {
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();
        for (Back2Panel.color = Color.white; Back2Panel.color.r > 0; Back2Panel.color -= new Color(1 / 30f, 1 / 30f, 1 / 30f, 0f)) {
            yield return new WaitForSeconds(1 / 60f);
        }
        Back2Panel.color = Color.black;
        yield return new WaitForSeconds(1.2f);
        for (Back2Panel.color = Color.black; Back2Panel.color.r < 1; Back2Panel.color += new Color(1 / 30f, 1 / 30f, 1 / 30f, 0f))
        {
            yield return new WaitForSeconds(1 / 60f);
        }
        Back2Panel.color = Color.white;
    }

    IEnumerator ToPointMove(Vector3 pos)
    {
        float mag = ToPointMagnitude(pos);
        float rad = ToPointAngleRAD(pos);
        for (int i = 0; i < 15; i++)
        {
            transform.localPosition += new Vector3(mag * (15 - i) * Mathf.Cos(rad) / 120, mag * (15 - i) * Mathf.Sin(rad) / 120);
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

    public void OnDestroy() // Overrided from 'Enemy'
    {
        Image Back2Panel = GameObject.Find("Back2Panel").GetComponent<Image>();
        Back2Panel.color = Color.white;

        Collapse();
    }
}
