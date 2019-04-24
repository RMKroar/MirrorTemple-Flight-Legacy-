using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorpion : Enemy {

    int count = 0;

    IEnumerator Move()
    {
        count++;
        
        if (count == 45)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            immortal = false;
            Emerge();
        }

        if (count == 1)
        {
            GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            immortal = true;
            yield return new WaitForSeconds(1 / 30f);
            StartCoroutine("Move");
        }
        else if (count > 1 && count <= 45)
        {
            GetComponent<Image>().color += new Color(0f, 0f, 0f, 1 / 45f);
            GameObject ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("DEBRIS", transform.localPosition + new Vector3(Random.Range(-50, 50), Random.Range(-50, 50)), 0f, 0,
                0.4f, 0.4f, BulletCode.COLOR_BROWN, BulletCode.COLOR_BROWN);
            yield return new WaitForSeconds(1 / 30f);
            StartCoroutine("Move");
        }
        else
        {
            yield return new WaitForSeconds(4f);
            StartCoroutine("EndMove");
            count = 0;
        }
    }

    IEnumerator EndMove()
    {
        count++;
        GameObject ins = GetBullet("CIRCLE");
        if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("DEBRIS", transform.localPosition + new Vector3(Random.Range(-50, 50), Random.Range(-50, 50)), 0f, 0,
            0.4f, 0.4f, BulletCode.COLOR_BROWN, BulletCode.COLOR_BROWN);
        GetComponent<Image>().color -= new Color(1/45f, 1 / 45f, 1 / 45f, 1 / 45f);
        yield return new WaitForSeconds(1 / 30f);
        if (count >= 45) Destroy(gameObject);
        StartCoroutine("EndMove");
    }

    void Emerge() {
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank >= 2) StartCoroutine("Launch");

        GameObject ins;
        if (GameController.rank <= 4)
        {
            float dir = 360f / (6 + GameController.rank * 3f);
            for (float i = Random.Range(0, dir); i < 360; i += dir)
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7f, i,
                        0.25f, 0.25f, Color.white, BulletCode.COLOR_BROWN);
            }
        }
        else if (GameController.rank <= 8)
        {
            float dir = 360f / (GameController.rank * 2f);
            for (int j = 0; j <= 1; j++) {
                for (float i = Random.Range(0, dir); i < 360; i += dir)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f + j * 2, i,
                            0.25f, 0.25f, Color.white, BulletCode.COLOR_BROWN);
                }
            }          
        }
        else
        {
            float dir = 360f / (GameController.rank * 2f);
            for (int j = 0; j <= 2; j++)
            {
                for (float i = Random.Range(0, dir); i < 360; i += dir)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 6f + j * 2, i,
                            0.25f, 0.25f, Color.white, BulletCode.COLOR_BROWN);
                }
            }
        }
    }

    IEnumerator Launch() {
        yield return new WaitForSeconds(2f);
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        GameObject ins;
        for (int i = 0; i < 3 + GameController.rank * 0.8f; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 7f + GameController.rank * 0.9f, ToPlayerAngle() + Random.Range(-3f, 3f), 0.25f, 0.25f, BulletCode.COLOR_LIGHTVIOLET, BulletCode.COLOR_VIOLET);
            yield return new WaitForSeconds(0.2f - GameController.rank * 0.015f);
        }
        
        StartCoroutine("Launch");
    }
}
