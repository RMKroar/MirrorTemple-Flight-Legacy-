using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlcadroLastorder : Boss
{
    public GameObject AlcadrobotB;
    public GameObject AlcadrobotC;
    public GameObject AlcadrobotD;

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
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        float launchDir = 3 + GameController.rank;
        for (float i = 90 + Random.Range(0f, 180f / launchDir); i < 270; i += (180f / launchDir)) {           
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                0.4f, 0.4f, Color.white, Color.red);

            lct.volume = 0.35f; lct.Play();
            yield return new WaitForSeconds(0.2f - 0.016f * GameController.rank);
        }
        for (int i = 0; i < 30 + 7 * GameController.rank; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(7f, 8f + 0.5f * GameController.rank), Random.Range(0f, 360f),
                0.25f, 0.25f, Color.white, Color.yellow);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(7f, 8f + 0.5f * GameController.rank), Random.Range(0f, 360f),
                0.25f, 0.25f, Color.white, BulletCode.COLOR_SKY);
            yield return new WaitForSeconds(0.05f - 0.004f * GameController.rank);
        }
        for (int i = 0; i < (GameController.rank + 2) / 2; i++) {
            ins = Instantiate(AlcadrobotC, GameObject.Find("TriggerPanel").transform);
            if (ins != null) ins.transform.localPosition = new Vector3(660f, GetPlayer().transform.localPosition.y + Random.Range(-35f, 35f), 0f);
            yield return new WaitForSeconds(1f / (GameController.rank + 1));
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        for (int i = -300; i <= 300; i += 150) {
            ins = Instantiate(AlcadrobotD, GameObject.Find("TriggerPanel").transform);
            if (ins != null)
            {
                ins.transform.localPosition = new Vector3(660f, i, 0f);
            }
        }
        yield return new WaitForSeconds(2.5f - GameController.rank * 0.1f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2() {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        float launchDir = 3 + GameController.rank;
        for (float i = 90 + Random.Range(0f, 180f / launchDir); i < 270; i += (180f / launchDir))
        {
            lct.volume = 0.35f; lct.Play();

            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                0.4f, 0.4f, Color.white, Color.red);
            yield return new WaitForSeconds(0.2f - 0.016f * GameController.rank);
        }
        yield return new WaitForSeconds(0.3f);
        launchDir = 3 + GameController.rank;
        for (float i = 270 - Random.Range(0f, 180f / launchDir); i > 90; i -= (180f / launchDir))
        {
            lct.volume = 0.35f; lct.Play();

            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                0.4f, 0.4f, Color.white, Color.blue);
            yield return new WaitForSeconds(0.2f - 0.016f * GameController.rank);
        }
        yield return new WaitForSeconds(0.3f);

        ins = Instantiate(AlcadrobotB, GameObject.Find("TriggerPanel").transform);
        if (ins != null) ins.transform.localPosition = new Vector3(660f, GetPlayer().transform.localPosition.y + Random.Range(-35f, 35f), 0f);

        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }

    bool pattern3flag = false;
    IEnumerator Pattern_3() {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        if (!pattern3flag) {
            pattern3flag = true;
            StopCoroutine(Move());
            StartCoroutine(Pattern_3_Support1());
            if (GameController.rank >= 3) StartCoroutine(Pattern_3_Support2());
            if (GameController.rank >= 6) StartCoroutine(Pattern_3_Support3());
        }

        if (GameController.rank <= 5)
        {
            for (int i = 90; i <= 160; i += 5)
            {
                lct.volume = 0.25f; lct.Play();

                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                    0.4f, 0.4f, Color.white, Color.red);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, 360 - i,
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(1 / 10f);
            }
            for (int i = 160; i >= 90; i -= 5)
            {
                lct.volume = 0.25f; lct.Play();

                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                    0.4f, 0.4f, Color.white, Color.red);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, 360 - i,
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(1 / 10f);
            }
        }
        else {
            for (int i = 90; i <= 170; i += 4)
            {
                lct.volume = 0.25f; lct.Play();

                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                    0.4f, 0.4f, Color.white, Color.red);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, 360 - i,
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(1 / 10f);
            }
            for (int i = 170; i >= 90; i -= 4)
            {
                lct.volume = 0.25f; lct.Play();

                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, i,
                    0.4f, 0.4f, Color.white, Color.red);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, 360 - i,
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(1 / 10f);
            }
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3_Support1() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        sct.Play();

        ins = GetBullet("CIRCLE");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7f, ToPlayerAngle(),
            0.4f, 0.4f, Color.white, Color.blue);
        float launchDir = 6 + GameController.rank * 3;
        for (float i = Random.Range(0f, 360f / launchDir); i < 360f; i += (360f / launchDir)) {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7f + 0.3f * GameController.rank, i,
                0.3f, 0.3f, Color.white, Color.yellow);
        }
        yield return new WaitForSeconds(1.5f - 0.07f * GameController.rank);
        StartCoroutine(Pattern_3_Support1());
    }

    IEnumerator Pattern_3_Support2()
    {
        GameObject ins;

        ins = GetBullet("PETAL");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT",transform.localPosition, 4f + 0.4f * GameController.rank, Random.Range(90f, 270f),
            0.3f, 0.3f, Color.white, Color.green);

        yield return new WaitForSeconds(0.25f - 0.01f * GameController.rank);
        StartCoroutine(Pattern_3_Support2());
    }

    IEnumerator Pattern_3_Support3()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        ins = GetBullet("PETAL");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", new Vector3(GetPlayer().transform.localPosition.x, 360f), 7f, 90,
            1f, 1f, Color.white, BulletCode.COLOR_VIOLET);

        yield return new WaitForSeconds(6f - 0.3f * GameController.rank);
        StartCoroutine(Pattern_3_Support3());
    }
}
