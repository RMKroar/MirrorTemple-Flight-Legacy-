using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oracle : Boss {

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
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        cct.Play();
        ins = GetBullet("CIRCLE");
        if (ins != null) {
            ins.GetComponent<Bullet>().SetIdentityEx("ORACLE_1", transform.localPosition, Random.Range(8f + 0.5f * GameController.rank, 12f + 0.7f * GameController.rank),
                ToPlayerAngle() + Random.Range(-40f, 40f), 0.5f, 0.5f, Color.white, Color.white);
        }
        yield return new WaitForSeconds(0.3f - 0.024f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        cct.Play();
        ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            ins.GetComponent<Bullet>().SetIdentityEx("ORACLE_2", transform.localPosition + new Vector3(-50f, 0f), Random.Range(6f, 11f),
                ToPlayerAngle() + Random.Range(-50f, 50f), 0.35f, 0.35f, Color.white, Color.white);
        }
        ins = GetBullet("CIRCLE");
        if (ins != null)
        {
            ins.GetComponent<Bullet>().SetIdentityEx("ORACLE_2", transform.localPosition + new Vector3(50f, 0f), Random.Range(6f, 11f),
                ToPlayerAngle() + Random.Range(-50f, 50f), 0.35f, 0.35f, Color.white, Color.white);
        }
        yield return new WaitForSeconds(0.5f - 0.045f * GameController.rank);
        StartCoroutine("Launch");
    }
}
