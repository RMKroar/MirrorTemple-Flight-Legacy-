using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoraAridum : Boss {

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

    IEnumerator Pattern_0() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        ins = GetBullet("CIRCLE");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_ARIDUM1", transform.localPosition, 3f, 180f + Random.Range(-10f, 10f), 2f, 2f,
             BulletCode.COLOR_BROWN, BulletCode.COLOR_DARKBROWN);
        yield return new WaitForSeconds(6f - 0.2f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 4)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_CACTUS", transform.localPosition, Random.Range(6f, 11f), Random.Range(150f, 210f), 0.8f, 0.8f,
                 Color.green, BulletCode.COLOR_TEAL);
            yield return new WaitForSeconds(2f - 0.2f * GameController.rank);
            StartCoroutine("Launch");
        }
        else {
            for (float i = 120; i < 240; i += (180f / GameController.rank))
            {
                ins = GetBullet("CIRCLE");                
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_CACTUS", transform.localPosition, 10f, i, 0.8f, 0.8f,
                     Color.green, BulletCode.COLOR_TEAL);
            }
            yield return new WaitForSeconds(2.4f);
            StartCoroutine("Launch");
        }
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        ins = GetBullet("CIRCLE");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_ARIDUM2", transform.localPosition, 3f, 180f + Random.Range(-20f, 20f), 1.2f, 1.2f,
             BulletCode.COLOR_BROWN, BulletCode.COLOR_DARKBROWN);
        yield return new WaitForSeconds(3f - 0.15f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        ins = GetBullet("CIRCLE");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_ARIDUM3", transform.localPosition, 6f + 0.6f * GameController.rank, 180f + Random.Range(-10f, 10f), 1.2f, 1.2f,
             Color.yellow, Color.red);
        yield return new WaitForSeconds(6f);
        StartCoroutine("Launch");
    }

    float col_freq = 0;
    int cnt4 = 0;

    IEnumerator Pattern_4() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        CircleCollider2D circol = GetComponent<CircleCollider2D>();
        Image im = GetComponent<Image>();
        yield return new WaitForSeconds(1 / 30f);
        cnt4++;
        if (cnt4 % 3 == 0) {
            cct.Play();
            for (int i = 0; i <= GameController.rank + 2; i++)
            {
                ins = GetBullet("CIRCLE");
                float randF = Random.Range(0f, 360f);
                float randS = Random.Range(0.15f, 0.3f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_MIRAGE", transform.localPosition + new Vector3(100 * Mathf.Cos(randF * Mathf.PI / 180f), 100 * Mathf.Sin(randF * Mathf.PI / 180f)),
                    Random.Range(4f, 4f + 0.5f * GameController.rank), randF + Random.Range(-170f, 190f), randS, randS, Color.white, Color.black);
            }
        }     
        circol.offset = new Vector3(0f, 150 * Mathf.Sin(col_freq));
        col_freq += 0.015f;

        im.color = new Color(1f, 1f, 1f, (2f - Mathf.Abs(Mathf.Sin(col_freq))) * 0.5f);
        StartCoroutine("Launch");
    }
}
