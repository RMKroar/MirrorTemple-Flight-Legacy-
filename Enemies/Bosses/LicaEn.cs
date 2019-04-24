using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LicaEn : Boss {

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
        else {
            direction = (direction == 270) ? Random.Range(270f, 280f) : Random.Range(80f, 90f);
        }
        radDir = direction * Mathf.PI / 180f;

        StartCoroutine("MoveSupport");
        StopCoroutine("Animate");
        StartCoroutine("Move");
    }

    IEnumerator MoveSupport() {
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

    IEnumerator Launch() {
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0()
    {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 1)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 10; i < 50; i += 10)
            {
                for (float j = -i; j <= i; j += 10)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 8) + Mathf.Abs(j / 8), ToPlayerAngle() + j,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(3.5f);
        }
        else if (GameController.rank <= 4)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 8; i < 48; i += 8)
            {
                for (float j = -i; j <= i; j += 8)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 8) + Mathf.Abs(j / 8), ToPlayerAngle() + j,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(3f);
        }
        else if (GameController.rank <= 7)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 6; i < 54; i += 6)
            {
                for (float j = -i; j <= i; j += 6)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 6) + Mathf.Abs(j / 8), ToPlayerAngle() + j,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(3.5f);
        }
        else if (GameController.rank <= 9)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 5; i < 50; i += 5)
            {
                for (float j = -i; j <= i; j += 5)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 5) + Mathf.Abs(j / 5), ToPlayerAngle() + j,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(3.1f);
        }
        else {
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle(),
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 5; i < 50; i += 5)
            {
                for (float j = -i; j <= i; j += 5)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 5) + Mathf.Abs(j / 5), ToPlayerAngle() + j,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(2.5f);
        }
        StartCoroutine("Launch");        
    }

    float angle = 0;
    IEnumerator Pattern_1() {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.3f; pct.Play();

        if (GameController.rank <= 1)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 60 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir), 360 * Mathf.Sin(dir), 0f), 1.5f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
            }
            angle += 30;
            yield return new WaitForSeconds(0.8f);
        }
        else if (GameController.rank <= 3)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 30 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir), 360 * Mathf.Sin(dir), 0f), 2f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
            }
            angle += 15;
            yield return new WaitForSeconds(0.3f);
        }
        else if (GameController.rank <= 5)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir), 360 * Mathf.Sin(dir), 0f), 2.5f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir + 180), 360 * Mathf.Sin(dir + 180), 0f), 2.5f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 10;
            yield return new WaitForSeconds(0.45f);
        }
        else if (GameController.rank <= 7)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir), 360 * Mathf.Sin(dir), 0f), 3f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir + 180), 360 * Mathf.Sin(dir + 180), 0f), 3f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 10;
            yield return new WaitForSeconds(0.25f);
        }
        else if (GameController.rank <= 9)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir), 360 * Mathf.Sin(dir), 0f), 3.5f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(360 * Mathf.Cos(dir + 180), 360 * Mathf.Sin(dir + 180), 0f), 3.5f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 10;
            yield return new WaitForSeconds(0.2f);
        }
        else {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(270 * Mathf.Cos(dir), 270 * Mathf.Sin(dir), 0f), 4f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(GetPlayer().transform.localPosition + new Vector3(270 * Mathf.Cos(dir + 180), 270 * Mathf.Sin(dir + 180), 0f), 4f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 10;
            yield return new WaitForSeconds(0.15f);
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_2() {
        GameObject ins;
        AudioSource sct = GameObject.Find("Sound_EnemyAttack").GetComponent<AudioSource>();
        sct.Play();

        if (GameController.rank <= 0)
        {
            float rand = Random.Range(-40f, 40f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 5; i < 10; i += 5)
            {
                for (float j = -i; j <= i; j += 5)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 5) + Mathf.Abs(j / 5), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        else if (GameController.rank <= 2)
        {
            float rand = Random.Range(-40f, 40f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 5; i < 10; i += 5)
            {
                for (float j = -i; j <= i; j += 5)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 5) + Mathf.Abs(j / 5), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
        else if (GameController.rank <= 4)
        {
            float rand = Random.Range(-40f, 40f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 2; i < 8; i += 2)
            {
                for (float j = -i; j <= i; j += 2)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 2) + Mathf.Abs(j / 2), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
        else if (GameController.rank <= 6)
        {
            float rand = Random.Range(-36f, 36f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 2; i < 10; i += 2)
            {
                for (float j = -i; j <= i; j += 2)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 2) + Mathf.Abs(j / 2), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        else if (GameController.rank <= 8)
        {
            float rand = Random.Range(-40f, 40f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 8; i < 48; i += 8)
            {
                for (float j = -i; j <= i; j += 8)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 8) + Mathf.Abs(j / 8), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(1.2f);
        }
        else {
            float rand = Random.Range(-40f, 40f);
            ins = GetBullet("CIRCLE");
            if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f, ToPlayerAngle() + rand,
                    0.25f, 0.25f, Color.white, Color.yellow);
            for (float i = 6; i < 42; i += 6)
            {
                for (float j = -i; j <= i; j += 6)
                {
                    ins = GetBullet("CIRCLE");
                    if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, 12f - (i / 6) + Mathf.Abs(j / 6), ToPlayerAngle() + j + rand,
                        0.25f, 0.25f, Color.white, Color.yellow);
                }
            }
            yield return new WaitForSeconds(0.7f);
        }
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        if (GameController.rank <= 0)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 60 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir), 120 * Mathf.Sin(dir), 0f), 7f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
            }
            angle += 30;
            yield return new WaitForSeconds(0.4f);
        }
        else if (GameController.rank <= 2)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 30 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir), 120 * Mathf.Sin(dir), 0f), 8f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
            }
            angle += 15;
            yield return new WaitForSeconds(0.15f);
        }
        else if (GameController.rank <= 4)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir), 120 * Mathf.Sin(dir), 0f), 7f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir + 180), 120 * Mathf.Sin(dir + 180), 0f), 7f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 15;
            yield return new WaitForSeconds(0.2f);
        }
        else if (GameController.rank <= 6)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir), 120 * Mathf.Sin(dir), 0f), 10f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir + 180), 120 * Mathf.Sin(dir + 180), 0f), 10f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 15;
            yield return new WaitForSeconds(0.16f);
        }
        else if (GameController.rank <= 8)
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir), 120 * Mathf.Sin(dir), 0f), 10f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(120 * Mathf.Cos(dir + 180), 120 * Mathf.Sin(dir + 180), 0f), 10f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 15;
            yield return new WaitForSeconds(0.12f);
        }
        else
        {
            float dir = angle * Mathf.PI / 180f;
            Color temp_col = (angle % 20 == 0) ? BulletCode.COLOR_VIOLET : Color.magenta;
            Color temp_col2 = (angle % 20 == 0) ? Color.red : Color.yellow;

            for (float i = Random.Range(0f, 72f); i < 360f; i += 72)
            {
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(200 * Mathf.Cos(dir), 200 * Mathf.Sin(dir), 0f), 13f, i, 0.4f, 0.4f,
                    Color.white, temp_col);
                ins = GetBullet("PETAL");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition + new Vector3(200 * Mathf.Cos(dir + 180), 200 * Mathf.Sin(dir + 180), 0f), 13f, i, 0.4f, 0.4f,
                    Color.white, temp_col2);
            }
            angle += 15;
            yield return new WaitForSeconds(0.09f);
        }
        StartCoroutine("Launch");
    }

    float dist;
    float dirc;

    IEnumerator Pattern_4() {
        // TODO: CODE_SEED
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        float randVal = Random.Range(-10f, 10f);
        Color temp_color = Random.Range(0, 2) == 1 ? Color.red : Color.magenta;

        cct.Play();
        
        if (GameController.rank >= 6)
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_SEED", transform.localPosition, Random.Range(8f, 20f),
                    ToPlayerAngle() + randVal, 0.4f, 0.4f, Color.white, temp_color);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_SEED", transform.localPosition, Random.Range(8f, 20f),
                    ToPlayerAngle() + randVal + 20f, 0.4f, 0.4f, Color.white, temp_color);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_SEED", transform.localPosition, Random.Range(8f, 20f),
                    ToPlayerAngle() + randVal - 20f, 0.4f, 0.4f, Color.white, temp_color);
            }

            yield return new WaitForSeconds(0.75f - (GameController.rank - 6) * 0.05f);
        }
        else
        {
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_SEED", transform.localPosition, Random.Range(6f, 8f + GameController.rank * 2),
                    ToPlayerAngle() + randVal + 10f, 0.4f, 0.4f, Color.white, temp_color);
            }
            ins = GetBullet("CIRCLE");
            if (ins != null)
            {
                ins.GetComponent<Bullet>().SetIdentityEx("CODE_SEED", transform.localPosition, Random.Range(6f, 8f + GameController.rank * 2),
                    ToPlayerAngle() + randVal - 10f, 0.4f, 0.4f, Color.white, temp_color);
            }
            yield return new WaitForSeconds(1.5f - GameController.rank * 0.15f);
        }
        StartCoroutine("Launch");
    }
}
