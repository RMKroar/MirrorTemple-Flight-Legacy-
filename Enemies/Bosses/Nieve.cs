using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nieve : Boss {

    public GameObject snowRevolution;
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
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.2f; pct.Play();

        GameObject ins;
        ins = Instantiate(snowRevolution, GameObject.Find("TriggerPanel").transform);
        if (ins != null)
        {
            ins.transform.localPosition = transform.localPosition;
            ins.GetComponent<SnowRevolution>().SetIdentity(Random.Range(5, 7), ToPlayerAngle());
        }

        float launchDir = 200;
        float randScale = Random.Range(0.25f, 0.4f + 0.02f * GameController.rank);
        for (float i = -660f + Random.Range(-launchDir, launchDir); i <= 660; i += launchDir) {
            ins = GetBullet("CIRCLE");
            if (ins != null) {
                ins.GetComponent<Bullet>().SetIdentity(new Vector3(i + Random.Range(-launchDir, launchDir), 400f), Random.Range(2f, 5f + GameController.rank * 0.8f), Random.Range(260f, 280f),
                    randScale, randScale, Color.white, BulletCode.COLOR_SKY);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(new Vector3(i + Random.Range(-launchDir, launchDir), 400f), Random.Range(2f, 5f + GameController.rank * 0.8f), Random.Range(260f, 280f),
                    randScale, randScale, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(1 / 30f);
        }

        yield return new WaitForSeconds(3f - 0.21f * GameController.rank);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_1() {
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.3f; pct.Play();
        GameObject ins;
        Vector3 playerPos = GetPlayer().transform.localPosition;
        int distDelta = 0;
        if (GameController.rank >= 4) distDelta += 25;
        if (GameController.rank >= 7) distDelta += 25;
        if (GameController.rank >= 9) distDelta += 25;

        for (float dist = 225 - distDelta, rot = Random.Range(0f, 360f), rot2 = rot + 180; dist <= 1100; dist += 300 / ((GameController.rank + 1) * 0.8f)) {
            for (float j = 0; j < 360; j += 120) {
                for (float i = Random.Range(0f, 60f); i < 360; i += 60)
                {
                    Vector3 createPos = playerPos + new Vector3(dist * Mathf.Cos((rot + j) * Mathf.PI / 180f), dist * Mathf.Sin((rot + j) * Mathf.PI / 180f));
                    if (Mathf.Abs(createPos.x) <= 660 && Mathf.Abs(createPos.y) <= 420) {
                        ins = GetBullet("PETAL");
                        ins.GetComponent<Bullet>().SetIdentityEx("SNOWFLOWER2", createPos, 0.5f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_SKY);
                    }                  
                }
            }
            for (float j = 0; j < 360; j += 120)
            {
                for (float i = Random.Range(0f, 60f); i < 360; i += 60)
                {
                    Vector3 createPos = playerPos + new Vector3(dist * Mathf.Cos((rot2 + j) * Mathf.PI / 180f), dist * Mathf.Sin((rot2 + j) * Mathf.PI / 180f));
                    if (Mathf.Abs(createPos.x) <= 660 && Mathf.Abs(createPos.y) <= 420)
                    {
                        ins = GetBullet("PETAL");
                        ins.GetComponent<Bullet>().SetIdentityEx("SNOWFLOWER2", createPos, 0.5f, i, 0.4f, 0.4f, Color.white, Color.blue);
                    }
                }
            }

            rot += 30 / ((GameController.rank + 1) * 0.3f);
            rot2 -= 30 / ((GameController.rank + 1) * 0.3f);
            yield return new WaitForSeconds(1 / 60f);
        }

        yield return new WaitForSeconds(3.2f);     
        GameController.EraseBullet();

        yield return new WaitForSeconds(1f);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_2()
    {
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        GameObject ins;

        for (int j = 0; j <= GameController.rank + 1; j++) {
            pct.volume = 0.2f; pct.Play();
            float launchDir = 200;
            float randScale = Random.Range(0.25f, 0.35f + 0.01f * GameController.rank);
            for (float i = -660f + Random.Range(-launchDir, launchDir); i <= 660; i += launchDir)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(new Vector3(i + Random.Range(-launchDir, launchDir), 400f), Random.Range(2f, 5f + GameController.rank * 0.8f), Random.Range(260f, 280f),
                        randScale, randScale, Color.white, BulletCode.COLOR_SKY);
                }
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(new Vector3(i + Random.Range(-launchDir, launchDir), 400f), Random.Range(2f, 5f + GameController.rank * 0.8f), Random.Range(260f, 280f),
                        randScale, randScale, Color.white, BulletCode.COLOR_SKY);
                }
                yield return new WaitForSeconds(1 / 30f);
            }
            yield return new WaitForSeconds(1 / 20f);
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_3() {
        GameObject ins;
        float playerY = GetPlayer().transform.localPosition.y;
        float deltaY = 350;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.2f; pct.Play();

        for (float j = -660; j <= 660; j += 300f / (GameController.rank + 2))
        {
            ins = GetBullet("PETAL");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("SNOWFLOWER", new Vector3(j, playerY + deltaY, 0f), 0.5f,
                    270f + Random.Range(-5f, 5f), 0.4f, 0.4f, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(1 / 120f);
        }

        yield return new WaitForSeconds(1f - 0.09f * GameController.rank);

        pct.volume = 0.2f; pct.Play();
        for (float j = -660; j <= 660; j += 300f / (GameController.rank + 2))
        {
            ins = GetBullet("PETAL");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("SNOWFLOWER", new Vector3(-j, playerY - deltaY, 0f), 0.5f,
                    90f + Random.Range(-5f, 5f), 0.4f, 0.4f, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(1 / 120f);
        }

        yield return new WaitForSeconds(2f - 0.05f * GameController.rank);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_4() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        cct.Play();
        ins = GetBullet("CIRCLE");
        if (ins != null) {
            ins.GetComponent<Bullet>().SetIdentityEx("SNOWPETAL", transform.localPosition, Random.Range(10f, 13f + GameController.rank * 0.5f),
                ToPlayerAngle() + Random.Range(-30f, 30f), 0.5f, 0.5f, Color.white, new Color(0f, Random.Range(0f, 1f), 1f));
        }
        yield return new WaitForSeconds(0.8f - GameController.rank * 0.04f);

        StartCoroutine(Launch());
    }

    public float ToGameObjectAngle(Vector3 pos)
    {
        Vector3 moveDirection = pos - transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }
}
