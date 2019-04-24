using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forever : Boss {

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

        float savedAngle = ToPlayerAngle();
        for (float j = Random.Range(-50f, -40f); j < 45f; j += 80f / (4 + GameController.rank))
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10 + GameController.rank * 0.5f, savedAngle + j, 0.35f, 0.35f, Color.white, Color.black);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 15 + GameController.rank * 0.5f, savedAngle + j + 40f / (4 + GameController.rank), 0.35f, 0.35f, Color.white, Color.black);
            }
            yield return new WaitForSeconds(1 / 10f - GameController.rank / 150f);
        }
        for (float j = Random.Range(40f, 50f); j > -45f; j -= 90f / (4 + GameController.rank))
        {
            cct.Play();
            ins = GetBullet("THORN");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL", transform.localPosition, 12 + GameController.rank, savedAngle + j, 0.4f, 0.4f, Color.white, Color.black);
            }
            yield return new WaitForSeconds(1 / 12f);
        }

        yield return new WaitForSeconds(1f - 0.09f * GameController.rank);
        StartCoroutine(Launch());
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        float savedAngle = ToPlayerAngle();
        for (float j = Random.Range(50f, 60f); j > -55f; j -= 210f / (4 + GameController.rank))
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL2", transform.localPosition, 15 + GameController.rank, savedAngle + j, 0.35f, 0.35f, Color.white, Color.black);
            }
            yield return new WaitForSeconds(1 / 12f);
        }
        yield return new WaitForSeconds(0.5f);
        for (float j = Random.Range(-50f, -60f); j < 55f; j += 210f / (4 + GameController.rank))
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("DOLL2", transform.localPosition, 15 + GameController.rank, savedAngle + j, 0.35f, 0.35f, Color.white, Color.black);
            }
            yield return new WaitForSeconds(1 / 12f);
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Launch());
    }

}
