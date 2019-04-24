using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcadrobotB : Enemy
{

    public float mov_startSpeed;
    public float mov_friction;
    public float mov_endSpeed;
    public GameObject AlcadrobotC;

    float direction;
    int launchCnt = 0;

    IEnumerator Move()
    {
        transform.localPosition += new Vector3(-mov_startSpeed, 0, 0);
        if (mov_startSpeed >= mov_friction)
        {
            mov_startSpeed -= mov_friction;
            yield return new WaitForSeconds(1 / 60f);
            StartCoroutine("Move");
        }
        else
        {
            if (transform.localPosition.y > 0) direction = Random.Range(180f, 200f);
            else direction = Random.Range(160f, 180f);
            yield return new WaitForSeconds(1f);
            StartCoroutine("EndMove");
        }
    }

    IEnumerator EndMove()
    {
        launchCnt++;
        mov_endSpeed += mov_friction;
        transform.localPosition += new Vector3(mov_endSpeed * Mathf.Cos(direction * Mathf.PI / 180f), mov_endSpeed * Mathf.Sin(direction * Mathf.PI / 180f));

        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (GameController.rank >= 9)
        {
            if (launchCnt % 2 == 0)
            {
                cct.Play();

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                    0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                    0.2f, 0.2f, Color.white, BulletCode.COLOR_SKY);
            }
        }
        else if (GameController.rank >= 7)
        {
            if (launchCnt % 2 == 0)
            {
                cct.Play();

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                    0.25f, 0.25f, Color.white, Color.yellow);
            }
        }
        else if (GameController.rank >= 5)
        {
            if (launchCnt % 3 == 0)
            {
                cct.Play();

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                    0.25f, 0.25f, Color.white, Color.yellow);
            }
        }
        else if (GameController.rank >= 3) {
            if (launchCnt % 5 == 0)
            {
                cct.Play();

                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                    0.25f, 0.25f, Color.white, Color.yellow);
            }
        }
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("EndMove");
    }

    IEnumerator Launch()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();

        if (GameController.rank <= 1)
        {
            lct.volume = 0.35f; lct.Play();

            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f, ToPlayerAngle() + Random.Range(-40f, 40f),
                0.4f, 0.4f, Color.white, Color.red);
        }
        else if (GameController.rank <= 6)
        {
            for (int i = 0; i <= 1; i++)
            {
                lct.volume = 0.35f; lct.Play();

                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f - i, ToPlayerAngle() + Random.Range(-40f, 40f),
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(0.3f);
            }
        }
        else
        {
            for (int i = 0; i <= 2; i++)
            {
                lct.volume = 0.35f; lct.Play();

                float tmp = ToPlayerAngle() + Random.Range(-30f, 30f);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZO", transform.localPosition, 12f - i, tmp,
                    0.4f, 0.4f, Color.white, Color.red);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return null;
    }

    private void OnDestroy()
    {
        GameObject ins = Instantiate(AlcadrobotC, GameObject.Find("TriggerPanel").transform);
        if (ins != null) ins.transform.localPosition = new Vector3(660f, Random.Range(-305f, 305f), 0f);
        Collapse();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Main")
        {
            Destroy(gameObject);
        }
    }
}
