using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tria : Boss {

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

        if (GameController.rank <= 2)
        {
            for (int i = 0; i < 2; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA1", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.2f - 0.05f * GameController.rank);
        }
        else if (GameController.rank <= 5) {
            for (int i = 0; i < 4; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA1", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }        
            yield return new WaitForSeconds(0.2f - 0.04f * (GameController.rank - 2));
        }
        else if (GameController.rank <= 8)
        {
            for (int i = 0; i < 8; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA1", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.18f - 0.03f * (GameController.rank - 5));
        }
        else
        {
            for (int i = 0; i < 13; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA1", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.18f - 0.03f * (GameController.rank - 8));
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 1)
        {
            for (float i = Random.Range(0, 360 / (4 + GameController.rank * 4)); i < 360; i += 360 / (4 + GameController.rank * 4))
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA2", transform.localPosition, 5, i, 0.3f,
                    0.3f, BulletCode.COLOR_TEAL, BulletCode.COLOR_TEAL);
            }
            yield return new WaitForSeconds(1f);
        }
        else if (GameController.rank <= 6)
        {
            for (float i = Random.Range(0, 360 / (3 + GameController.rank * 4)); i < 360; i += 360 / (-1 + GameController.rank * 3))
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA2", transform.localPosition, 5, i, 0.3f,
                    0.3f, BulletCode.COLOR_TEAL, BulletCode.COLOR_TEAL);
            }
            yield return new WaitForSeconds(1f);
        }
        else {
            for (float i = Random.Range(0, 360 / (45 + (GameController.rank - 7) * 15)); i < 360; i += 360 / (15 + GameController.rank * 3))
            {
                ins = GetBullet("CIRCLE");
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA2", transform.localPosition, 3, i, 0.3f,
                    0.3f, BulletCode.COLOR_TEAL, BulletCode.COLOR_TEAL);
            }
            yield return new WaitForSeconds(2.4f);
        }
        
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        if (GameController.rank <= 2)
        {
            for (int i = 0; i < 2; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA3", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.2f - 0.05f * GameController.rank);
        }
        else if (GameController.rank <= 5)
        {
            for (int i = 0; i < 4; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA3", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.2f - 0.04f * (GameController.rank - 2));
        }
        else if (GameController.rank <= 8)
        {
            for (int i = 0; i < 8; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA3", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.18f - 0.03f * (GameController.rank - 5));
        }
        else
        {
            for (int i = 0; i < 13; i++)
            {
                ins = GetBullet("CIRCLE");
                float sc = Random.Range(0.2f, 0.45f);
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA3", transform.localPosition, 1f, Random.Range(0f, 360f),
                        sc, sc, Color.white, BulletCode.COLOR_SKY);
            }
            yield return new WaitForSeconds(0.18f - 0.03f * (GameController.rank - 8));
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.25f; pct.Play();

        if (GameController.rank <= 2)
        {
            Vector3 pos = new Vector3(Random.Range(-580f, 580f), 400f);
            float dir = Random.Range(240f, 300f);

            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos, 1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(50f * Mathf.Cos(45f * Mathf.PI / 180f), 50f * Mathf.Sin(45f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(50f * Mathf.Cos(135f * Mathf.PI / 180f), 50f * Mathf.Sin(135f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(100f * Mathf.Cos(30f * Mathf.PI / 180f), 100f * Mathf.Sin(30f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(100f * Mathf.Cos(150f * Mathf.PI / 180f), 100f * Mathf.Sin(150f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            yield return new WaitForSeconds(0.9f - 0.2f * GameController.rank);
        }
        else if (GameController.rank <= 4)
        {
            Vector3 pos = new Vector3(Random.Range(-580f, 580f), 400f);
            float dir = Random.Range(240f, 300f);

            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos, 1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(40f * Mathf.Cos(45f * Mathf.PI / 180f), 40f * Mathf.Sin(45f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(40f * Mathf.Cos(135f * Mathf.PI / 180f), 40f * Mathf.Sin(135f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(80f * Mathf.Cos(30f * Mathf.PI / 180f), 80f * Mathf.Sin(30f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(80f * Mathf.Cos(150f * Mathf.PI / 180f), 80f * Mathf.Sin(150f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(15f * Mathf.PI / 180f), 120f * Mathf.Sin(15f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(165f * Mathf.PI / 180f), 120f * Mathf.Sin(165f * Mathf.PI / 180f)),
                1, dir, 0.4f, 0.4f, Color.black, Color.black);
            yield return new WaitForSeconds(0.9f - 0.15f * GameController.rank);
        }
        else if (GameController.rank <= 8)
        {
            Vector3 pos = new Vector3(Random.Range(-580f, 580f), 400f);
            float dir = Random.Range(267f, 273f);

            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos, 1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(60f * Mathf.Cos(45f * Mathf.PI / 180f), 60f * Mathf.Sin(45f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(60f * Mathf.Cos(135f * Mathf.PI / 180f), 60f * Mathf.Sin(135f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(30f * Mathf.PI / 180f), 120f * Mathf.Sin(30f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(150f * Mathf.PI / 180f), 120f * Mathf.Sin(150f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            yield return new WaitForSeconds(0.55f - 0.06f * (GameController.rank - 5));
        }
        else { // INSANE
            Vector3 pos = new Vector3(Random.Range(-580f, 580f), 400f);
            float dir = Random.Range(267f, 273f);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos, 1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(60f * Mathf.Cos(45f * Mathf.PI / 180f), 60f * Mathf.Sin(45f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(60f * Mathf.Cos(135f * Mathf.PI / 180f), 60f * Mathf.Sin(135f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(30f * Mathf.PI / 180f), 120f * Mathf.Sin(30f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(120f * Mathf.Cos(150f * Mathf.PI / 180f), 120f * Mathf.Sin(150f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(180f * Mathf.Cos(20f * Mathf.PI / 180f), 180f * Mathf.Sin(20f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            ins = GetBullet("CIRCLE");
            ins.GetComponent<Bullet>().SetIdentityEx("CODE_TRIA4", pos + new Vector3(180f * Mathf.Cos(160f * Mathf.PI / 180f), 180f * Mathf.Sin(160f * Mathf.PI / 180f)),
                1, dir, 0.25f, 0.25f, Color.black, Color.black);
            yield return new WaitForSeconds(0.42f - 0.08f * (GameController.rank - 9));
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_4()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        int[] density = new int[] { 72, 80, 88, 95, 102, 108, 114, 120, 130, 140, 160 }; 
        float del = Random.Range(0f, 360f);
   
        for (float i = 0; i <= 360; i += (360f/density[GameController.rank])) {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 8f + GameController.rank * 0.7f, del + i, 0.2f, 0.2f,
                 Color.white, new Color(0, Random.Range(0f, 0.75f), 1f));
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
    }
}
