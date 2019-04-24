using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lavetica : Boss {

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

        int[] density = new int[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70 };
        float del = ToPlayerAngle() - Random.Range(25f, 35f);

        for (int j = 0; j < 60; j++) {
            cct.Play();
            for (float i = del; i < del + 60; i += (360f / density[GameController.rank]))
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 15 + 2 * GameController.rank, i, 0.4f, 0.4f,
                     Color.white, new Color(1, Random.Range(0f, 0.75f), 0f));
            }
            yield return new WaitForSeconds(1 / 10f - 1/200f * GameController.rank);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        float tmpSpd = 0;
        if (GameController.rank <= 3) tmpSpd = 1f + GameController.rank;
        else tmpSpd = 3f + (GameController.rank - 3) * 0.5f;

        for (int i = 0; i < (12 + GameController.rank) / 3; i++) {
            sct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("LAVABALL", transform.localPosition, 1f + GameController.rank, ToPlayerAngle() + Random.Range(-40f, 40f), 0.5f, 0.5f, Color.white, Color.red);
                tmpSpd += 2;
            }
            yield return new WaitForSeconds(0.5f - GameController.rank * 0.02f);
        } 
        
        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        int[] density = new int[] { 15, 19, 23, 27, 31, 35, 39, 43, 47, 51, 55 };
        int[] delay = new int[] { 10, 10, 5, 5, 4, 4, 3, 3, 2, 2, 1 };
        float del = ToPlayerAngle() - Random.Range(25f, 35f);

        for (int j = 0; j < 60; j++)
        {
            cct.Play();
            for (float i = del; i < del + 60; i += (360f / density[GameController.rank]))
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 15 + 2 * GameController.rank, i, 0.4f, 0.4f,
                     Color.white, new Color(1, Random.Range(0f, 0.75f), 0f));
            }

            if (j % delay[GameController.rank] == 0) {
                ins = GetBullet("CIRCLE");
                if (ins != null)
                {
                    ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + Random.Range(-20f, 20f), 0.3f, 0.3f, Color.white, Color.red);
                }
            }

            yield return new WaitForSeconds(1 / 10f - 1 / 200f * GameController.rank);
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        int ea = 6 + GameController.rank * 4;

        for (int i = 0; i < ea; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("MAGMA", new Vector3(GetPlayer().transform.localPosition.x + Random.Range(-350f, 350f), -480f),
                    Random.Range(8f, 27f), 90f + Random.Range(-20f, 20f), 0.35f, 0.35f, Color.red, Color.red);
            }

            yield return new WaitForSeconds(1 / 20f - (1 / 300f * GameController.rank));
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < ea; i++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("MAGMA", new Vector3(GetPlayer().transform.localPosition.x + Random.Range(-350f, 350f), 480f),
                    Random.Range(8f, 27f), 270f + Random.Range(-20f, 20f), 0.35f, 0.35f, Color.red, Color.red);
            }

            yield return new WaitForSeconds(1 / 20f - (1 / 300f * GameController.rank));
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }
}
