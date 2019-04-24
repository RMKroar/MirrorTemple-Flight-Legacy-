using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcadrobotC : Enemy {

    public float mov_speed;
    public float mov_friction;

    float direction;
    bool flag = false;

    IEnumerator Move()
    {
        if (!flag) {
            direction = ToPlayerAngle();
            mov_speed += Random.Range(-3f, 3f);
            flag = true;
        }
        mov_speed += mov_friction;
        transform.localPosition += new Vector3(mov_speed * Mathf.Cos(direction * Mathf.PI / 180f), mov_speed * Mathf.Sin(direction * Mathf.PI / 180f));
        yield return new WaitForSeconds(1 / 60f);
        StartCoroutine("Move");
    }

    IEnumerator Launch() {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        if (GameController.rank >= 9)
        {          
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.red);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.green);
            yield return new WaitForSeconds(1 / 15f);
        }
        else if (GameController.rank >= 7)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.red);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.green);
            yield return new WaitForSeconds(1 / 8f);
        }
        else if (GameController.rank >= 5)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.green);
            yield return new WaitForSeconds(1 / 8f);
        }
        else if (GameController.rank >= 3)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.green);
            yield return new WaitForSeconds(1 / 3f);
        }
        else {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(5f, 2f + GameController.rank), Random.Range(0f, 360f),
                0.15f, 0.15f, Color.white, Color.green);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine("Launch");
    }

    private void OnDestroy()
    {
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        lct.volume = 0.35f; lct.Play();

        if (GameController.rank >= 9)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 12f, ToPlayerAngle(),
                0.3f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 10f, ToPlayerAngle() + 10,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 10f, ToPlayerAngle() - 10,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
        }
        else if (GameController.rank >= 7)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 10f, ToPlayerAngle(),
                0.3f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 8f, ToPlayerAngle() + 20,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 8f, ToPlayerAngle() - 20,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
        }
        else if (GameController.rank >= 5)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 9f, ToPlayerAngle(),
                0.3f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 7f, ToPlayerAngle() + 30,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 7f, ToPlayerAngle() - 30,
                0.3f, 0.3f, Color.white, BulletCode.COLOR_SKY);
        }
        else if (GameController.rank >= 3)
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 8f, ToPlayerAngle(),
                0.3f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
        }
        else
        {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ALCADROLAZZOSHORT", transform.localPosition, 3 + GameController.rank, ToPlayerAngle(),
                0.3f, 0.3f, Color.white, BulletCode.COLOR_VIOLET);
        }

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
