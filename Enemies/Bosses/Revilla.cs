using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Revilla : Boss {
    public GameObject revillaSword;

    float _speed = 0;
    float direction = 0;
    float radDir;

    IEnumerator Move()
    {
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

        yield return null;
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
        StopCoroutine("Move");
        StartCoroutine("Pattern_" + (maxPattern - pattern));
        immortal = false;
        yield return null;
    }

    IEnumerator Pattern_0()
    {
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        GameObject ins = Instantiate(revillaSword, transform);
        ins.transform.localPosition = new Vector3(0f, -20f, 0f);
        ins.GetComponent<RevillaSword>().moveCode = 0;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("Move");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1() {
        GameObject ins;
        StopCoroutine("Pattern_1_Support");
        StartCoroutine("Pattern_1_Support");
        yield return new WaitForSeconds(4f);

        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();
        pct.volume = 0.35f; pct.Play();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet")) {
            Bullet objData = obj.GetComponent<Bullet>();
            if (obj.transform.localScale.x >= 0.7f && objData.moveCode != "DEBRIS") {
                objData.speed = 0;
                ins = Instantiate(revillaSword, GameObject.Find("BulletPool").transform);
                ins.transform.localPosition = obj.transform.localPosition;
                ins.GetComponent<RevillaSword>().direction = Random.Range(0f, 360f);
                ins.GetComponent<RevillaSword>().moveCode = 1;

                objData.BulletAddon.GetComponent<Image>().color = Color.black;
            }
        }

        yield return new WaitForSeconds(0.8f);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Bullet objData = obj.GetComponent<Bullet>();
            if (obj.transform.localScale.x >= 0.7f && objData.speed == 0)
            {
                for (float i = Random.Range(0f, 360f / (GameController.rank + 2)); i < 360; i += 360f / (GameController.rank + 2)) {
                    ins = GetBullet("CIRCLE");
                    ins.GetComponent<Bullet>().SetIdentity(obj.transform.localPosition, 5 + GameController.rank * 0.5f, i, 0.35f, 0.35f,
                        Color.white, Color.black);
                }

                objData.UnableWithDebris();
            }           
        }

        StartCoroutine("Move");
        StartCoroutine("Launch");
    }

    IEnumerator Pattern_1_Support() {
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();
        cct.Play();

        GameObject ins = GetBullet("CIRCLE");
        ins.GetComponent<Bullet>().SetIdentity(new Vector3(Random.Range(-550f, 550f), 420f), Random.Range(2f, 9f), Random.Range(260f, 280f), 0.7f, 0.7f,
            Color.white, Color.red);

        yield return new WaitForSeconds(1.1f - GameController.rank * 0.07f);
        StartCoroutine("Pattern_1_Support");
    }

    int moveDirection = 1;

    IEnumerator Pattern_2()
    {
        StopCoroutine("Pattern_1_Support");       
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        float dir = 180f - moveDirection * 40f;
        float sp = 10f;
        float _direction = Random.Range(165f, 195f);

        StopCoroutine("Animate");
        for (float _speed = 21, cnt = 0; _speed > 0; _speed -= 0.4f, cnt++)
        {
            transform.localPosition += new Vector3(_speed * Mathf.Cos(_direction * Mathf.PI / 180f), _speed * Mathf.Sin(_direction * Mathf.PI / 180f));

            PatternSupport2(cnt, sp, dir);
            dir -= moveDirection * 37.7f;
            sp -= 0.12f;
            yield return new WaitForSeconds(1 / 90f);
        }
        StartCoroutine("Animate");
        yield return new WaitForSeconds(0.5f);

        pct.volume = 0.35f; pct.Play();
        GameObject ins = Instantiate(revillaSword, transform);
        ins.transform.localPosition = new Vector3(0f, -20f, 0f);
        ins.GetComponent<RevillaSword>().moveCode = 2;

        StopCoroutine("Animate");
        for (float _speed = 21, cnt = 0; _speed > 0; _speed -= 0.4f, cnt++)
        {
            transform.localPosition -= new Vector3(_speed * Mathf.Cos(_direction * Mathf.PI / 180f), _speed * Mathf.Sin(_direction * Mathf.PI / 180f));
            yield return new WaitForSeconds(1 / 90f);
        }
        StartCoroutine("Animate");
        yield return new WaitForSeconds(2f);
        StartCoroutine("Launch");
        moveDirection *= -1;
    }

    private void PatternSupport2(float cnt, float sp, float dir)
    {
        GameObject ins;
        AudioSource cct = GameObject.Find("Sound_EnemyCluster").GetComponent<AudioSource>();

        if (GameController.rank <= 1)
        {
            if (cnt % 8 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir + 18.3f, 0.25f, 0.25f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 2, dir - 18.3f, 0.25f, 0.25f, Color.white, Color.blue);
            }
        }
        else if (GameController.rank <= 4)
        {
            if (cnt % 6 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 12.7f, 0.25f, 0.25f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 12.7f, 0.25f, 0.25f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3, dir + 25.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3, dir - 25.4f, 0.25f, 0.25f, Color.white, Color.yellow);
            }
        }
        else if (GameController.rank <= 7)
        {
            if (cnt % 4 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.35f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 11.7f, 0.25f, 0.25f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 11.7f, 0.25f, 0.25f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir + 23.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir - 23.4f, 0.25f, 0.25f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir + 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir - 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 46.8f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 46.8f, 0.25f, 0.25f, Color.white, Color.green);
            }
        }
        else
        {
            if (cnt % 2 == 0)
            {
                cct.Play();
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 5, dir, 0.35f, 0.45f, Color.white, Color.red);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 11.7f, 0.35f, 0.35f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 11.7f, 0.35f, 0.35f, Color.white, Color.blue);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir + 23.4f, 0.3f, 0.3f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 3f, dir - 23.4f, 0.3f, 0.3f, Color.white, Color.yellow);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir + 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp - 1f, dir - 35.1f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir + 46.8f, 0.25f, 0.25f, Color.white, Color.green);
                ins = GetBullet("THORN");
                if (ins != null) ins.GetComponent<Bullet>().SetIdentity(transform.localPosition, sp + 1, dir - 46.8f, 0.25f, 0.25f, Color.white, Color.green);
            }
        }
    }

    IEnumerator Pattern_3()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();

        for (int i = 0; i < 5; i++) {
            pct.volume = 0.35f; pct.Play();
            float rand = Random.Range(0f, 360f);
            ins = Instantiate(revillaSword, GameObject.Find("BulletPool").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition - new Vector3(100 * Mathf.Cos(rand * Mathf.PI / 180f), 100 * Mathf.Sin(rand * Mathf.PI / 180f));
            ins.GetComponent<RevillaSword>().direction = rand;
            ins.GetComponent<RevillaSword>().moveCode = 3;

            yield return new WaitForSeconds(1.5f - GameController.rank * 0.07f);
        }
        yield return new WaitForSeconds(2f);

        StartCoroutine("Move");
        pct.volume = 0.35f; pct.Play();
        for (float i = Random.Range(0f, 45f); i < 360; i += 45)
        {
            ins = Instantiate(revillaSword, GameObject.Find("BulletPool").transform);
            ins.transform.localPosition = GetPlayer().transform.localPosition - new Vector3(100 * Mathf.Cos(i * Mathf.PI / 180f), 100 * Mathf.Sin(i * Mathf.PI / 180f));
            ins.GetComponent<RevillaSword>().direction = i;
            ins.GetComponent<RevillaSword>().moveCode = 3;
        }

        yield return new WaitForSeconds(3f - GameController.rank * 0.1f);

        StartCoroutine("Launch");
    }

    bool flag = false;

    IEnumerator Pattern_4()
    {
        GameObject ins;
        AudioSource pct = GameObject.Find("Sound_EnemyPetal").GetComponent<AudioSource>();       

        if (!flag) {
            for (float _speed = 12, cnt = 0; _speed > 0; _speed -= 0.2f, cnt++)
            {
                transform.localPosition += new Vector3(_speed * Mathf.Cos(ToCenterAngle() * Mathf.PI / 180f), _speed * Mathf.Sin(ToCenterAngle() * Mathf.PI / 180f));
                yield return new WaitForSeconds(1 / 90f);
            }
            flag = true;
        }

        pct.volume = 0.35f; pct.Play();
        for (float i = Random.Range(0f, 72f); i < 360; i += 72)
        {
            ins = Instantiate(revillaSword, GameObject.Find("BulletPool").transform);
            ins.transform.localPosition = transform.localPosition - new Vector3(200 * Mathf.Cos(i * Mathf.PI / 180f), 200 * Mathf.Sin(i * Mathf.PI / 180f));
            ins.GetComponent<RevillaSword>().direction = i + 108;
            ins.GetComponent<RevillaSword>().moveCode = 4;
        }

        yield return new WaitForSeconds(5f - GameController.rank * 0.2f);
        pct.volume = 0.35f; pct.Play();
        for (float i = Random.Range(0f, 72f); i < 360; i += 72)
        {
            ins = Instantiate(revillaSword, GameObject.Find("BulletPool").transform);
            ins.transform.localPosition = transform.localPosition - new Vector3(200 * Mathf.Cos(i * Mathf.PI / 180f), 200 * Mathf.Sin(i * Mathf.PI / 180f));
            ins.GetComponent<RevillaSword>().direction = i - 108;
            ins.GetComponent<RevillaSword>().moveCode = 4;
        }
        yield return new WaitForSeconds(5f - GameController.rank * 0.15f);

        StartCoroutine("Launch");
    }

    public float ToCenterAngle()
    {
        Vector3 moveDirection = -transform.localPosition;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            return angle;
        }
        else return 0;
    }
}