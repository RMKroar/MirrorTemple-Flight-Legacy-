using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ephesus : Boss {

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
        int nowPattern = maxPattern - pattern;

        StartCoroutine("Pattern_" + (nowPattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        float[] density = new float[11] { 12.5f, 12f, 11.5f, 11f, 10.5f, 10f, 9.5f, 9f, 8.5f, 8f, 7.5f };
        float savedAngle = ToPlayerAngle();

        for (float i = savedAngle - Random.Range(30f, 40f); i <= savedAngle + 35f; i += density[GameController.rank]) {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, i, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 6f, i, 0.35f, 0.35f,
                 BulletCode.COLOR_VIOLET, Color.white);
        }       
        yield return new WaitForSeconds(0.8f - 0.03f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();      

        float[] density = new float[11] { 11.5f, 11f, 10.5f, 10f, 9.5f, 9f, 8.5f, 8f, 7.5f, 7f, 6.5f };

        float shootEa = 40 + 6 * GameController.rank;
        for (float i = Random.Range(0f, 360f), j = 0; j <= shootEa; 
            i += density[GameController.rank] + Random.Range(-1.5f, 1.5f), j++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.2f, i, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.2f, i + 60, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.2f, i + 120, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.2f, i + 180, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.2f, i + 240, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.2f, i + 300, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);

            yield return new WaitForSeconds(1 / 15f - 1 / 225f * GameController.rank);
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        
        float[] density = new float[11] { 13f, 12.5f, 12f, 11.5f, 11, 10.5f, 10f, 9.5f, 9f, 8.5f, 8f };

        for (int j = 0; j < 3 + GameController.rank; j++) {
            float savedAngle = ToPlayerAngle();
            sct.Play();
            for (float i = savedAngle - Random.Range(30f, 40f); i <= savedAngle + 35f; i += density[GameController.rank])
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 10f, i, 0.35f, 0.35f,
                     Color.white, BulletCode.COLOR_VIOLET);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 7f, i, 0.35f, 0.35f,
                     BulletCode.COLOR_VIOLET, Color.white);
            }

            yield return new WaitForSeconds(0.6f - 0.03f * GameController.rank);
        }
        
        yield return new WaitForSeconds(1f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        float[] density = new float[11] { 20f, 18f, 16f, 15f, 14f, 13f, 12f, 11f, 10f, 9f, 8f };
        Vector3 savedPosition = GetPlayer().transform.localPosition;
        float savedStarter = Random.Range(0f, 360f);
        int randomSeed = Random.Range(0, 2);

        pct.volume = 0.4f; pct.Play();
        if (randomSeed == 0)
        {
            for (float i = savedStarter, j = 0; i < savedStarter + 360; i += density[GameController.rank], j++)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_TEAL);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i + 30, 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i - 30, 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);

                yield return new WaitForSeconds(1 / 240f);
            }
        }
        else {
            for (float i = savedStarter, j = 0; i > savedStarter - 360; i -= density[GameController.rank], j++)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_TEAL);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i + 30, 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.2f, i - 30, 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);

                yield return new WaitForSeconds(1 / 240f);
            }
        }
        
        yield return new WaitForSeconds(3f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_4()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        float[] density = new float[11] { 14f, 13.5f, 13f, 12.5f, 12f, 11.5f, 11f, 10.5f, 10f, 9.5f, 9f };
        float savedAngle = ToPlayerAngle();

        for (float i = savedAngle - Random.Range(30f, 40f); i <= savedAngle + 35f; i += density[GameController.rank])
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ORACLE_2", transform.localPosition, 10f, i, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_VIOLET);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 6f, i, 0.35f, 0.35f,
                 BulletCode.COLOR_VIOLET, Color.white);
        }
        yield return new WaitForSeconds(0.8f - 0.03f * GameController.rank);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_5()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        float[] density = new float[11] { 22f, 21f, 20f, 19f, 18f, 17f, 16f, 15f, 14f, 13f, 12f };

        float shootEa = 20 + 3 * GameController.rank;
        for (float i = Random.Range(0f, 360f), j = 0; j <= shootEa;
            i -= density[GameController.rank] + Random.Range(-1.5f, 1.5f), j++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.4f, i, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.4f, i + 60, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.4f, i + 120, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.4f, i + 180, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 1.5f + j * 0.4f, i + 240, 0.35f, 0.35f,
                 Color.white, BulletCode.COLOR_TEAL);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 1.5f + (shootEa - j) * 0.4f, i + 300, 0.35f, 0.35f,
                 BulletCode.COLOR_TEAL, Color.white);

            yield return new WaitForSeconds(1 / 15f - 1 / 225f * GameController.rank);
        }
        yield return new WaitForSeconds(0.8f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_6()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();

        float[] density = new float[11] { 16f, 15f, 14f, 13.5f, 13f, 12.5f, 12f, 11.5f, 11, 10.5f, 10f };

        for (int j = 0; j < 3 + GameController.rank; j++)
        {
            float savedAngle = ToPlayerAngle();
            sct.Play();
            for (float i = savedAngle - Random.Range(30f, 40f); i <= savedAngle + 35f; i += density[GameController.rank])
            {
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("ORACLE_2", transform.localPosition, 10f, i, 0.35f, 0.35f,
                     Color.white, BulletCode.COLOR_VIOLET);
                ins = GetBullet("CIRCLE");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, 7f, i, 0.35f, 0.35f,
                     BulletCode.COLOR_VIOLET, Color.white);
            }

            yield return new WaitForSeconds(0.6f - 0.03f * GameController.rank);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_7()
    {
        GameController.EraseBullet();
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        float[] density = new float[11] { 32f, 30f, 28f, 26f, 24f, 22f, 20f, 18f, 16f, 15f, 14f };
        Vector3 savedPosition = GetPlayer().transform.localPosition;
        float savedStarter = Random.Range(0f, 360f);

        pct.volume = 0.4f; pct.Play();
        int randomSeed = Random.Range(0, 2);

        if (randomSeed == 0)
        {
            for (float i = savedStarter, j = 0; i > savedStarter - 720; i -= density[GameController.rank], j++)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.15f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_TEAL);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + GameController.rank * 0.25f, i + (12 - 0.5f * GameController.rank), 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + GameController.rank * 0.25f, i - (12 - 0.5f * GameController.rank), 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);

                yield return new WaitForSeconds(1 / 240f);
            }
        }
        else {
            for (float i = savedStarter, j = 0; i < savedStarter + 720; i += density[GameController.rank], j++)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + j * 0.15f, i, 0.4f, 0.4f, Color.white, BulletCode.COLOR_TEAL);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + GameController.rank * 0.25f, i + (12 - 0.5f * GameController.rank), 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS",
                    savedPosition - new Vector3(600f * Mathf.Cos(i * Mathf.PI / 180f), 600f * Mathf.Sin(i * Mathf.PI / 180f)),
                    2f + GameController.rank * 0.25f, i - (12 - 0.5f * GameController.rank), 0.4f, 0.4f, BulletCode.COLOR_TEAL, Color.white);

                yield return new WaitForSeconds(1 / 240f);
            }
        }
        
        yield return new WaitForSeconds(3f);
        StartCoroutine("Launch");
    }

    bool lastFlag = false;
    IEnumerator Pattern_8()
    {
        if (!lastFlag) {
            maxHealth *= 2;
            health *= 2;
            lastFlag = true;
        }

        GameController.EraseBullet();
        GameObject ins;
        AudioSource lct = GameObject.Find("Sound_EnemyLazer").GetComponent<AudioSource>();
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        for (int i = 0; i < 10 + GameController.rank * 3; i++) {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, Random.Range(3f, 9f), ToPlayerAngle() + Random.Range(-60f, 60f), 0.25f, 0.25f,
                 Color.white, Color.green);
            yield return new WaitForSeconds(0.2f - 0.015f * GameController.rank);
        }
        yield return new WaitForSeconds(0.5f);

        lct.volume = 0.4f; lct.Play();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet")) {
            ins = GetBullet("PETAL");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUSLAZZO", obj.transform.localPosition, 0.01f, Random.Range(0f, 360f), 0.3f, 0.3f,
                 Color.white, Color.green);
            obj.GetComponent<Bullet>().UnableWithDebris();
        }

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 40 + GameController.rank * 6; i++)
        {
            cct.Play();
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentityEx("EPHESUS", transform.localPosition, Random.Range(4f, 15f), ToPlayerAngle() + Random.Range(-40f, 40f), 0.35f, 0.35f,
                 BulletCode.COLOR_VIOLET, Color.white);
            yield return new WaitForSeconds(0.05f - 0.003f * GameController.rank);
        }
        yield return new WaitForSeconds(4f);

        StartCoroutine("Launch");
    }
}
